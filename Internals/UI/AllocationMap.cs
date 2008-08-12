using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SqlInternals.AllocationInfo.Internals.Pages;
using SqlInternals.AllocationInfo.Internals.Renderers;
using System.Drawing.Drawing2D;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    /// <summary>
    /// Allocation Map used to display allocation layers for files
    /// </summary>
    public class AllocationMap : Panel, IDisposable
    {
        private readonly Color defaultPageBorderColour = Color.White;
        private Color borderColour = Color.Gainsboro;
        public static Size Large = new Size(256, 32);
        public static Size Medium = new Size(96, 12);
        public static Size Small = new Size(64, 8);
        private int extentCount;
        private int extentsHorizontal;
        private int extentsRemaining;
        private int extentsVertical;
        private int visibleExtents;
        private int windowPosition;
        private DatabaseFile file;
        private int fileId;
        private bool includeIam;
        private List<AllocationLayer> mapLayers = new List<AllocationLayer>();
        private Size extentSize;
        private MapMode mode;
        private readonly VScrollBar scrollBar;
        private int selectedPage = -1;
        private PageAddress startPage = new PageAddress(1, 0);
        public event EventHandler<PageEventArgs> PageClicked;
        public event EventHandler RangeSelected;
        public event EventHandler<PageEventArgs> PageOver;
        public event EventHandler WindowPositionChanged;
        private readonly PageExtentRenderer pageExtentRenderer;

        private int selectionStartExtent = -1;
        private int selectionEndExtent = -1;
        private int provisionalEndExtent;
        Pen backgroundLine = new Pen(Color.FromArgb(242, 242, 242), 2);

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationMap"/> class.
        /// </summary>
        public AllocationMap()
        {
            SuspendLayout();

            this.BackColor = Color.White;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            Padding = new Padding(1);
            scrollBar = new VScrollBar();
            scrollBar.Enabled = false;
            scrollBar.Dock = DockStyle.Right;

            Controls.Add(scrollBar);

            ResumeLayout(false);

            MouseClick += AllocationMapPanel_MouseClick;
            scrollBar.ValueChanged += ScrollBar_ValueChanged;
            MouseMove += AllocationMapPanel_MouseMove;
            extentSize = AllocationMap.Small;

            pageExtentRenderer = new PageExtentRenderer(Color.WhiteSmoke, Color.FromArgb(234, 234, 234));
            pageExtentRenderer.CreateBrushesAndPens(this.ExtentSize);
        }

        /// <summary>
        /// Draws the pages in the single page slots for the allocations the map displays
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void DrawSinglePages(PaintEventArgs e)
        {
            pageExtentRenderer.ResizePageBrush(this.ExtentSize);

            foreach (AllocationLayer layer in mapLayers)
            {
                if (layer.Visible)
                {
                    Color pageColour;

                    if (layer.UseDefaultSinglePageColour)
                    {
                        pageColour = Color.Salmon;
                    }
                    else
                    {
                        pageColour = layer.Colour;
                    }

                    pageExtentRenderer.SetExtentBrushColour(pageColour, ExtentColour.LightBackgroundColour(pageColour));

                    if (layer.UseBorderColour)
                    {
                        pageExtentRenderer.PageBorderColour = layer.BorderColour;
                    }
                    else
                    {
                        pageExtentRenderer.PageBorderColour = defaultPageBorderColour;
                    }

                    foreach (Allocation allocation in layer.Allocations)
                    {
                        foreach (PageAddress address in allocation.SinglePageSlots)
                        {
                            if (address.FileId == FileId && address.PageId != 0 && CheckPageVisible(address.PageId))
                            {
                                pageExtentRenderer.DrawPage(e.Graphics,
                                                            PagePosition(address.PageId - (WindowPosition * 8)),
                                                            layer.LayerType);
                            }
                        }

                        if (includeIam)
                        {
                            foreach (AllocationPage page in allocation.Pages)
                            {
                                if (CheckPageVisible(page.PageAddress.PageId))
                                {
                                    pageExtentRenderer.DrawPage(e.Graphics,
                                                                PagePosition(page.PageAddress.PageId - (WindowPosition * 8)),
                                                                AllocationLayerType.Standard);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws the extents for each allocation layer
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void DrawExtents(PaintEventArgs e)
        {
            pageExtentRenderer.ResizeExtentBrush(this.ExtentSize);

            for (int extent = windowPosition;
                 extent < extentCount && extent < (visibleExtents + windowPosition);
                 extent++)
            {
                foreach (AllocationLayer layer in mapLayers)
                {
                    if (layer.Visible && !layer.SingleSlotsOnly)
                    {
                        foreach (Allocation chain in layer.Allocations)
                        {
                            int targetExtent = extent + (startPage.PageId / 8);

                            if (Allocation.CheckAllocationStatus(targetExtent, fileId, layer.Invert, chain))
                            {
                                pageExtentRenderer.SetExtentBrushColour(layer.Colour,
                                                                        ExtentColour.BackgroundColour(layer.Colour));

                                pageExtentRenderer.DrawExtent(e.Graphics, ExtentPosition(extent - WindowPosition));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks the page is visible on the map
        /// </summary>
        /// <param name="pageId">The page Id.</param>
        /// <returns></returns>
        private bool CheckPageVisible(int pageId)
        {
            return pageId >= (windowPosition * 8) && pageId <= ((visibleExtents + windowPosition) * 8);
        }

        /// <summary>
        /// Get Rectange for a particular extent
        /// </summary>
        /// <param name="extent">The extent.</param>
        /// <returns></returns>
        private Rectangle ExtentPosition(int extent)
        {
            if (extentsHorizontal > 1)
            {
                return new Rectangle((extent * extentSize.Width) % (extentsHorizontal * extentSize.Width),
                                     (int)Math.Floor((decimal)extent / extentsHorizontal) * extentSize.Height,
                                     extentSize.Width,
                                     extentSize.Height);
            }
            else
            {
                return new Rectangle(0, 0, extentSize.Width, extentSize.Height);
            }
        }

        /// <summary>
        /// Get the position for a particular page
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        private Rectangle PagePosition(int page)
        {
            int pageWidth = extentSize.Width / 8;

            if (page != 0)
            {
                return new Rectangle((page * pageWidth) % ((extentsHorizontal * 8) * pageWidth),
                                     (int)Math.Floor((decimal)page / (extentsHorizontal * 8)) * extentSize.Height,
                                     pageWidth,
                                     extentSize.Height);
            }
            else
            {
                return new Rectangle(0, 0, pageWidth, extentSize.Height);
            }
        }

        /// <summary>
        /// Get the extent at a particular x and y position
        /// </summary>
        /// <param name="x">The x co-ordinate</param>
        /// <param name="y">The y co-ordinate</param>
        private int ExtentPosition(int x, int y)
        {
            return 1 + (y / extentSize.Height * extentsHorizontal) + (x / extentSize.Width);
        }

        /// <summary>
        /// Get the page at a particular x and y position
        /// </summary>
        /// <param name="x">The x co-ordinate</param>
        /// <param name="y">The y co-ordinate</param>
        private int PagePosition(int x, int y)
        {
            return (y / extentSize.Height * (extentsHorizontal * 8)) + (x / (extentSize.Width / 8));
        }

        /// <summary>
        /// Calculates the number (horizontal and vertical) of the visible extents.
        /// </summary>
        internal void CalculateVisibleExtents()
        {
            extentsHorizontal = (int)Math.Floor((decimal)(Width - scrollBar.Width) / extentSize.Width);
            extentsVertical = (int)Math.Ceiling((decimal)Height / extentSize.Height);

            if (extentsHorizontal == 0 | extentsVertical == 0 | extentCount == 0)
            {
                return;
            }
            extentsRemaining = extentCount - (extentsHorizontal * extentsVertical);

            scrollBar.SmallChange = extentsHorizontal;
            scrollBar.LargeChange = (extentsVertical - 1) * extentsHorizontal;

            if (extentsHorizontal == 0)
            {
                extentsHorizontal = 1;
            }

            if (extentsHorizontal * extentsVertical > extentCount)
            {
                VisibleExtents = extentCount;
                scrollBar.Enabled = false;
            }
            else
            {
                scrollBar.Enabled = true;
                VisibleExtents = extentsHorizontal * extentsVertical;
            }

            scrollBar.Maximum = extentCount + extentsHorizontal;

            if (extentsHorizontal > extentCount)
            {
                extentsHorizontal = extentCount;
            }

            if (extentsVertical > (extentCount / extentsHorizontal))
            {
                extentsVertical = (extentCount / extentsHorizontal);
            }

            extentsRemaining = extentCount - (extentsHorizontal * extentsVertical);
        }

        /// <summary>
        /// Draws the selected range.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void DrawSelectedRange(PaintEventArgs e)
        {
            if (selectionStartExtent > 0)
            {
                for (int extent = selectionStartExtent; extent < (selectionEndExtent < 0 ? provisionalEndExtent : selectionEndExtent); extent++)
                {
                    pageExtentRenderer.DrawSelection(e.Graphics, ExtentPosition(extent));
                }
            }
        }

        #region Events

        /// <summary>
        /// Handles the MouseClick event of the AllocationMapPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void AllocationMapPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int newSelectedBlock = ExtentPosition(e.X, e.Y);

                if (newSelectedBlock != SelectedPage)
                {
                    int page = PagePosition(e.X, e.Y) + (WindowPosition * 8);

                    if (page <= (extentCount * 8))
                    {
                        SelectedPage = PagePosition(e.X, e.Y) + (WindowPosition * 8);

                        if (this.Mode == MapMode.RangeSelection)
                        {
                            if (selectionStartExtent <= 0)
                            {
                                selectionStartExtent = newSelectedBlock;
                            }
                            else
                            {
                                selectionEndExtent = newSelectedBlock;

                                EventHandler temp = RangeSelected;
                                if (temp != null)
                                {
                                    temp(this, EventArgs.Empty);
                                }
                            }
                        }
                        else
                        {
                            EventHandler<PageEventArgs> temp = PageClicked;

                            if (temp != null)
                            {
                                bool openInNewWindow = Control.ModifierKeys == Keys.Shift;

                                temp(this, new PageEventArgs(new RowIdentifier(FileId, page + startPage.PageId, 0), openInNewWindow));
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the AllocationMapPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void AllocationMapPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Mode != MapMode.Full)
            {
                int newSelectedBlock = ExtentPosition(e.X, e.Y);

                if (newSelectedBlock != SelectedPage)
                {
                    int page = PagePosition(e.X, e.Y) + (WindowPosition * 8);

                    if (page <= (extentCount * 8))
                    {
                        EventHandler<PageEventArgs> temp = PageOver;

                        if (temp != null)
                        {
                            temp(this, new PageEventArgs(new RowIdentifier(FileId, page + startPage.PageId, 0), false));
                        }

                        if (this.Mode == MapMode.RangeSelection)
                        {
                            if (provisionalEndExtent != newSelectedBlock)
                            {
                                provisionalEndExtent = newSelectedBlock;
                                this.Invalidate();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!e.ClipRectangle.IsEmpty && extentCount > 0 && Visible)
            {
                if (this.Mode != MapMode.Full)
                {
                    for (int l = 0; l < Height; l += 4)
                    {
                        e.Graphics.DrawLine(backgroundLine, 0, l, Width, l);
                    }

                    CalculateVisibleExtents();

                    pageExtentRenderer.DrawBackgroundExtents(e,
                                                             this.ExtentSize,
                                                             extentsHorizontal,
                                                             extentsVertical,
                                                             extentsRemaining);
                }
                else
                {
                    scrollBar.Enabled = false;
                }
                switch (mode)
                {
                    case MapMode.Standard:

                        if (extentCount > 0)
                        {
                            DrawExtents(e);
                        }

                        DrawSinglePages(e);
                        break;

                    case MapMode.Pfs:

                        // DrawPfsPages(e);
                        break;

                    case MapMode.Map:

                        // DrawMapPage(e);
                        break;

                    case MapMode.RangeSelection:

                        DrawSelectedRange(e);
                        break;

                    case MapMode.Full:

                        DrawFullMap(e);
                        break;
                }
            }

            ControlPaint.DrawBorder(e.Graphics,
                                    new Rectangle(0, 0, Width, Height),
                                    SystemColors.ControlDark,
                                    ButtonBorderStyle.Solid);
        }

        private void DrawFullMap(PaintEventArgs e)
        {
            // TODO: Change this so it buffers rather than repainting every time

            float extentHeight;
            float extentWidth;

            double adjustedWidth = this.Width / 8.0D;
            double databaseSize = this.File.Size / 8.0D;
            double initalExtentSize = Math.Sqrt((this.Height * adjustedWidth) / databaseSize);

            int extentsPerLine = (int)Math.Floor(adjustedWidth / initalExtentSize) + 1;

            extentWidth = (float)(adjustedWidth / (float)extentsPerLine);
            extentHeight = (float)(this.Height / Math.Ceiling(databaseSize / extentsPerLine));

            extentWidth *= 8;

            foreach (AllocationLayer layer in mapLayers)
            {
                LinearGradientBrush brush = new LinearGradientBrush(this.Bounds, layer.Colour, ExtentColour.LightBackgroundColour(layer.Colour), 0.45F);

                if (layer.Visible && !layer.SingleSlotsOnly)
                {
                    foreach (Allocation chain in layer.Allocations)
                    {
                        int colPos = 0;
                        float rowPos = 0.0F;

                        for (int i = 0; (i < this.File.Size / 8); i++)
                        {
                            if (colPos >= extentsPerLine)
                            {
                                colPos = 0;
                                rowPos += extentHeight;
                            }

                            if (true)
                            {
                                if (Allocation.CheckAllocationStatus(i, fileId, layer.Invert, chain))
                                {
                                    e.Graphics.FillRectangle(brush, colPos * extentWidth, rowPos, extentWidth, extentHeight);
                                }
                            }

                            colPos++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when [window position changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        internal virtual void OnWindowPositionChanged(object sender, EventArgs e)
        {
            if (WindowPositionChanged != null)
                WindowPositionChanged(sender, e);
        }

        /// <summary>
        /// Handles the ValueChanged event of the ScrollBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            WindowPosition = scrollBar.Value - (scrollBar.Value % extentsHorizontal);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        /// <value>The file id.</value>
        public int FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        /// <summary>
        /// Gets or sets the map layers.
        /// </summary>
        /// <value>The map layers.</value>
        public List<AllocationLayer> MapLayers
        {
            get { return mapLayers; }
            set { mapLayers = value; }
        }

        /// <summary>
        /// Gets or sets the number of visible extents.
        /// </summary>
        /// <value>The number visible extents.</value>
        public int VisibleExtents
        {
            get { return visibleExtents; }
            set { visibleExtents = value; }
        }

        /// <summary>
        /// Gets or sets the border colour.
        /// </summary>
        /// <value>The border colour.</value>
        public Color BorderColour
        {
            get { return pageExtentRenderer.PageBorderColour; }
            set { pageExtentRenderer.PageBorderColour = value; }
        }

        /// <summary>
        /// Gets or sets the selected page.
        /// </summary>
        /// <value>The selected page.</value>
        public int SelectedPage
        {
            get { return selectedPage + startPage.PageId; }
            set { selectedPage = value; }
        }

        /// <summary>
        /// Gets or sets the size of the extent.
        /// </summary>
        /// <value>The size of the extent.</value>
        public Size ExtentSize
        {
            get { return extentSize; }
            set
            {
                extentSize = value;
                CalculateVisibleExtents();
                pageExtentRenderer.ResizeExtentBrush(extentSize);
                // ResizePfs();

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the extent count.
        /// </summary>
        /// <value>The extent count.</value>
        public int ExtentCount
        {
            get { return extentCount; }
            set { extentCount = value; }
        }

        /// <summary>
        /// Gets or sets the window position.
        /// </summary>
        /// <value>The window position.</value>
        public int WindowPosition
        {
            get { return windowPosition; }
            set
            {
                windowPosition = value;
                scrollBar.Value = windowPosition;
                OnWindowPositionChanged(this, EventArgs.Empty);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IAMs are included.
        /// </summary>
        /// <value><c>true</c> if [include iam]; otherwise, <c>false</c>.</value>
        public bool IncludeIam
        {
            get { return includeIam; }
            set { includeIam = value; }
        }

        /// <summary>
        /// Gets or sets the allocation map mode.
        /// </summary>
        /// <value>The allocation map mode.</value>
        public MapMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;

                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the selection start extent.
        /// </summary>
        /// <value>The selection start extent.</value>
        public int SelectionStartExtent
        {
            get { return selectionStartExtent; }
            set { selectionStartExtent = value; }
        }

        /// <summary>
        /// Gets or sets the selection end extent.
        /// </summary>
        /// <value>The selection end extent.</value>
        public int SelectionEndExtent
        {
            get { return selectionEndExtent; }
            set { selectionEndExtent = value; }
        }

        /// <summary>
        /// Gets or sets the start page.
        /// </summary>
        /// <value>The start page.</value>
        [Browsable(false)]
        public PageAddress StartPage
        {
            get { return startPage; }
            set
            {
                startPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        public DatabaseFile File
        {
            get { return file; }
            set { file = value; }
        }

        public bool DrawBorder
        {
            get { return this.pageExtentRenderer.DrawBorder; }
            set { this.pageExtentRenderer.DrawBorder = value; }
        }

        #endregion

        void IDisposable.Dispose()
        {
            backgroundLine.Dispose();
            pageExtentRenderer.Dispose();
        }
    }
}

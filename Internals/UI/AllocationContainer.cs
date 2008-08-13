﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    /// <summary>
    /// Allocation Container containing one of more allocation maps
    /// </summary>
    public partial class AllocationContainer : UserControl
    {
        private readonly Dictionary<int, AllocationMap> allocationMaps = new Dictionary<int, AllocationMap>();
        private Size extentSize = new Size(64, 8);
        private bool includeIam;
        private LayoutStyle layoutStyle;
        private readonly List<AllocationLayer> mapLayers = new List<AllocationLayer>();
        private MapMode mode;
        private bool showFileInformation;

        public event EventHandler<PageEventArgs> PageClicked;
        public event EventHandler<PageEventArgs> PageOver;
        public event EventHandler RangeSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationContainer"/> class.
        /// </summary>
        public AllocationContainer()
        {
            InitializeComponent();

            this.Paint += AllocationContainer_Paint;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        /// <summary>
        /// Creates the allocation maps.
        /// </summary>
        /// <param name="files">The database files.</param>
        public void CreateAllocationMaps(List<DatabaseFile> files)
        {
            SuspendLayout();
            tableLayoutPanel.SuspendLayout();

            tableLayoutPanel.Controls.Clear();
            allocationMaps.Clear();
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Clear();

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 1.0F));

            int fileIndex = 0;

            foreach (DatabaseFile file in files)
            {
                AllocationMap allocationMap = CreateAllocationMap(file);
                allocationMap.ExtentSize = ExtentSize;
                allocationMap.Mode = Mode;

                Panel filePanel = new Panel();
                filePanel.Margin = new Padding(0);
                filePanel.Controls.Add(allocationMap);

                filePanel.Dock = DockStyle.Fill;
                tableLayoutPanel.Controls.Add(filePanel, 1, fileIndex);

                filePanel.Margin = new Padding(0, 0, 0, 4);

                if (fileIndex > 0)
                {
                    tableLayoutPanel.RowCount += 1;
                    tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 1.0F));
                }

                if (!showFileInformation && fileIndex == files.Count - 1)
                {
                    filePanel.Margin = new Padding(0);
                }

                fileIndex++;
            }

            tableLayoutPanel.Invalidate();
            tableLayoutPanel.ResumeLayout();
            ResumeLayout();
        }

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        /// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        public override void Refresh()
        {
            base.Refresh();
            Invalidate();
        }

        /// <summary>
        /// Creates the allocation map.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private AllocationMap CreateAllocationMap(DatabaseFile file)
        {
            AllocationMap allocationMap = new AllocationMap();

            allocationMap.FileId = file.FileId;
            allocationMap.File = file;
            allocationMap.ExtentCount = file.Size / 8;
            allocationMap.Dock = DockStyle.Fill;
            allocationMap.MapLayers = MapLayers;

            allocationMap.PageClicked += AllocationMap_PageClicked;
            allocationMap.PageOver += AllocationMap_PageOver;
            allocationMap.RangeSelected += AllocationMap_RangeSelected;
            allocationMaps.Add(file.FileId, allocationMap);

            return allocationMap;
        }

        /// <summary>
        /// Adds a map layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public void AddMapLayer(AllocationLayer layer)
        {
            mapLayers.Add(layer);

            foreach (AllocationMap allocationMap in allocationMaps.Values)
            {
                allocationMap.Invalidate();
            }
        }

        /// <summary>
        /// Clears all map layers.
        /// </summary>
        public void ClearMapLayers()
        {
            mapLayers.Clear();

            foreach (AllocationMap allocationMap in allocationMaps.Values)
            {
                allocationMap.Invalidate();
            }
        }

        /// <summary>
        /// Removes a layer.
        /// </summary>
        /// <param name="name">The name of the layer to remove</param>
        /// <returns></returns>
        public bool RemoveLayer(string name)
        {
            AllocationLayer existing = mapLayers.Find(delegate(AllocationLayer layer) { return (layer.Name == name); });

            if (existing != null)
            {
                mapLayers.Remove(existing);

                foreach (AllocationMap allocationMap in allocationMaps.Values)
                {
                    allocationMap.Invalidate();
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        public Size CalculateFitSize()
        {
            double maxExtentCount = 0;

            foreach (AllocationMap map in allocationMaps.Values)
            {
                if (map.ExtentCount > maxExtentCount)
                {
                    maxExtentCount = map.ExtentCount;
                }
            }

            double width = this.Width;
            double height = this.Height / 8;

            double extentsPerRow = Math.Sqrt(maxExtentCount / 8);

            Size returnSize = new Size((int)(width / extentsPerRow), (int)(height / extentsPerRow));

            if (returnSize.Height < 1 || returnSize.Width < 1)
            {
                returnSize = new Size(8, 1);
            }

            return returnSize;
        }

        /// <summary>
        /// Handles the Paint event of the AllocationContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        protected void AllocationContainer_Paint(object sender, PaintEventArgs e)
        {
            if (allocationMaps.Count == 0)
            {
                ControlPaint.DrawBorder(e.Graphics,
                                        new Rectangle(0, 0, Width, Height),
                                        SystemColors.ControlDark,
                                        ButtonBorderStyle.Solid);
            }
        }

        /// <summary>
        /// Handles the RangeSelected event of the AllocationMap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AllocationMap_RangeSelected(object sender, EventArgs e)
        {
            EventHandler temp = RangeSelected;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        /// <summary>
        /// Handles the PageOver event of the AllocationMap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlInternals.AllocationInfo.Internals.Pages.PageEventArgs"/> instance containing the event data.</param>
        private void AllocationMap_PageOver(object sender, PageEventArgs e)
        {
            EventHandler<PageEventArgs> temp = PageOver;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        /// <summary>
        /// Handles the PageClicked event of the AllocationMap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlInternals.AllocationInfo.Internals.Pages.PageEventArgs"/> instance containing the event data.</param>
        private void AllocationMap_PageClicked(object sender, PageEventArgs e)
        {
            EventHandler<PageEventArgs> temp = PageClicked;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [show file information].
        /// </summary>
        /// <value><c>true</c> if [show file information]; otherwise, <c>false</c>.</value>
        public bool ShowFileInformation
        {
            get { return showFileInformation; }
            set
            {
                if (value != showFileInformation)
                {
                    showFileInformation = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the allocation map mode.
        /// </summary>
        /// <value>The mode.</value>
        public MapMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                foreach (AllocationMap allocationMap in allocationMaps.Values)
                {
                    allocationMap.Mode = mode;
                }

                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets the map layers collection
        /// </summary>
        /// <value>The map layers.</value>
        public List<AllocationLayer> MapLayers
        {
            get { return mapLayers; }
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

                foreach (AllocationMap allocationMap in allocationMaps.Values)
                {
                    allocationMap.ExtentSize = extentSize;
                }
            }
        }

        /// <summary>
        /// Gets or sets the layout style.
        /// </summary>
        /// <value>The layout style.</value>
        public LayoutStyle LayoutStyle
        {
            get { return layoutStyle; }
            set { layoutStyle = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the IAM is included
        /// </summary>
        /// <value><c>true</c> if [include iam]; otherwise, <c>false</c>.</value>
        public bool IncludeIam
        {
            get { return includeIam; }
            set { includeIam = value; }
        }

        /// <summary>
        /// Gets the allocation map Dictionary collection
        /// </summary>
        /// <value>The allocation maps.</value>
        public Dictionary<int, AllocationMap> AllocationMaps
        {
            get { return allocationMaps; }
        }


        public bool DrawBorder
        {
            get {
                if (this.allocationMaps.Count > 1)
                {
                    return this.allocationMaps[0].DrawBorder;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                foreach (AllocationMap allocationMap in allocationMaps.Values)
                {
                    allocationMap.DrawBorder = value;
                }

            }
        }


        #endregion


        internal void ShowFittedMap()
        {
            foreach (AllocationMap allocationMap in allocationMaps.Values)
            {
                allocationMap.ShowFullMap();
            }
        }
    }

    /// <summary>
    /// Layout style of the container
    /// </summary>
    public enum LayoutStyle
    {
        Horizontal,
        Vertical
    }
}

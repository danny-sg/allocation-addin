using System.Collections.Generic;
using System.Drawing;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    /// <summary>
    /// Contains an Allocation structure to be displayed on the Allocation Map
    /// </summary>
    public class AllocationLayer
    {
        private Color colour;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationLayer"/> class.
        /// </summary>
        public AllocationLayer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationLayer"/> class.
        /// </summary>
        /// <param name="page">The allocation.</param>
        public AllocationLayer(Allocation page)
        {
            Allocations.Add(page);
            Name = page.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationLayer"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="allocation">The allocation.</param>
        /// <param name="colour">The colour of the layer.</param>
        public AllocationLayer(string name, Allocation allocation, Color colour)
        {
            this.Name = name;
            Allocations.Add(allocation);
            this.colour = colour;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationLayer"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="page">The page.</param>
        /// <param name="colour">The colour of the layer.</param>
        public AllocationLayer(string name, AllocationPage page, Color colour)
        {
            this.Name = name;

            if (page.Header.PageType == PageType.Iam)
            {
                Allocations.Add(new IamAllocation(page));
            }
            else
            {
                Allocations.Add(new Allocation(page));
            }

            this.colour = colour;
        }

        /// <summary>
        /// Finds a page.
        /// </summary>
        /// <param name="page">The page address.</param>
        /// <param name="layers">The layers to search.</param>
        /// <returns></returns>
        public static List<string> FindPage(PageAddress page, List<AllocationLayer> layers)
        {
            var layerNames = new List<string>();

            foreach (var layer in layers)
            {
                if (layer.FindPage(page, layer.Invert) != null)
                {
                    layerNames.Add(layer.Name);
                }
            }

            return layerNames;
        }

        /// <summary>
        /// Refreshes the layers.
        /// </summary>
        /// <param name="layers">The layers.</param>
        public static void RefreshLayers(List<AllocationLayer> layers)
        {
            foreach (var layer in layers)
            {
                foreach (var page in layer.Allocations)
                {
                    page.Refresh();
                }
            }
        }

        /// <summary>
        /// Creates and return a layer.
        /// </summary>
        /// <param name="name">The layer name.</param>
        /// <param name="invert">if set to <c>true</c> [invert].</param>
        /// <param name="transparent">if set to <c>true</c> make [transparent].</param>
        /// <param name="colour">The layer colour.</param>
        /// <param name="firstPage">The first page.</param>
        /// <returns></returns>
        public static AllocationLayer CreateLayer(string name,
                                                  bool invert,
                                                  bool transparent,
                                                  Color colour,
                                                  PageAddress firstPage)
        {
            var allocation = new Allocation(ServerConnection.CurrentConnection().CurrentDatabase, firstPage);

            return CreateLayer(name, invert, transparent, colour, allocation);
        }

        /// <summary>
        /// Creates and returns a layer.
        /// </summary>
        /// <param name="name">The layer name.</param>
        /// <param name="invert">if set to <c>true</c> [invert].</param>
        /// <param name="transparent">if set to <c>true</c> make [transparent].</param>
        /// <param name="colour">The layer colour.</param>
        /// <param name="allocation">The allocation to use.</param>
        /// <returns></returns>
        public static AllocationLayer CreateLayer(string name,
                                                  bool invert,
                                                  bool transparent,
                                                  Color colour,
                                                  Allocation allocation)
        {
            var layer = new AllocationLayer(allocation);
            layer.Invert = invert;
            layer.Transparent = transparent;
            layer.Name = name;
            layer.Colour = colour;

            return layer;
        }

        public AllocationLayer FindExtent(int extent, int fileId, bool findInverted)
        {
            foreach (var alloc in Allocations)
            {
                if (Allocation.CheckAllocationStatus(extent, fileId, findInverted, alloc))
                {
                    return this;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a page.
        /// </summary>
        /// <param name="pageAddress">The page address.</param>
        /// <param name="findInverted">if set to <c>true</c> [find inverted].</param>
        /// <returns></returns>
        public AllocationLayer FindPage(PageAddress pageAddress, bool findInverted)
        {
            int extentAddress;

            extentAddress = pageAddress.PageId / 8;

            foreach (var alloc in Allocations)
            {
                // Check if it's the actual IAM
                if (alloc.Pages.Exists(delegate(AllocationPage p) { return p.PageAddress == pageAddress; }))
                {
                    return this;
                }

                if (Allocation.CheckAllocationStatus(extentAddress, pageAddress.FileId, findInverted, alloc))
                {
                    return this;
                }

                if (alloc.SinglePageSlots.Contains(pageAddress))
                {
                    return this;
                }
            }

            return null;
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AllocationLayer"/> is transparent.
        /// </summary>
        /// <value><c>true</c> if transparent; otherwise, <c>false</c>.</value>
        public bool Transparent { get; set; }

        /// <summary>
        /// Gets or sets the type of the layer.
        /// </summary>
        /// <value>The type of the layer.</value>
        public AllocationLayerType LayerType { get; set; } = AllocationLayerType.Standard;

        /// <summary>
        /// Gets or sets the layer colour.
        /// </summary>
        /// <value>The layer colour.</value>
        public Color Colour
        {
            get
            {
                if (Transparent)
                {
                    return Color.FromArgb(Transparency, colour);
                }
                else
                {
                    return colour;
                }
            }

            set
            {
                colour = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the allocations.
        /// </summary>
        /// <value>The allocations.</value>
        public List<Allocation> Allocations { get; } = new List<Allocation>();

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AllocationLayer"/> is invert.
        /// </summary>
        /// <value><c>true</c> if invert; otherwise, <c>false</c>.</value>
        public bool Invert { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the default single page colour.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if use default single page colour; otherwise, <c>false</c>.
        /// </value>
        public bool UseDefaultSinglePageColour { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AllocationLayer"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the border colour.
        /// </summary>
        /// <value>The border colour.</value>
        public Color BorderColour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use border colour.
        /// </summary>
        /// <value><c>true</c> if [use border colour]; otherwise, <c>false</c>.</value>
        public bool UseBorderColour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display single slots only.
        /// </summary>
        /// <value><c>true</c> if [single slots only]; otherwise, <c>false</c>.</value>
        public bool SingleSlotsOnly { get; set; } = false;

        /// <summary>
        /// Gets or sets the transparency level.
        /// </summary>
        /// <value>The transparency level.</value>
        public int Transparency { get; set; } = 40;

        #endregion
    }

    /// <summary>
    /// Types of allocation layers
    /// </summary>
    public enum AllocationLayerType
    {
        /// <summary>
        /// Standard Allocation
        /// </summary>
        Standard,

        /// <summary>
        /// Tag in top left hand corner
        /// </summary>
        TopLeftCorner
    }
}

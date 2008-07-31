using System;
using System.Collections.Generic;
using System.Text;
using SqlInternals.AllocationInfo.Internals.Pages;
using System.Drawing;
using System.Data;
using System.Collections;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    public class AllocationLayer
    {
        private readonly List<Allocation> allocations = new List<Allocation>();
        private Color borderColour;
        private Color colour;
        private bool invert;
        private string name;
        private int order;
        private bool singleSlotsOnly = false;
        private bool transparent;
        private bool useBorderColour;
        private AllocationLayerType layerType = AllocationLayerType.Standard;
        private int transparency = 40;
        private bool useDefaultSinglePageColour = false;
        private bool visible = true;

        public AllocationLayer()
        {
            
        }

        public AllocationLayer(Allocation page)
        {
            allocations.Add(page);
            name = page.ToString();
        }

        public AllocationLayer(string name, Allocation allocation, Color colour)
        {
            this.name = name;
            allocations.Add(allocation);
            this.colour = colour;
        }

        public AllocationLayer(string name, AllocationPage page, Color colour)
        {
            this.name = name;
            if (page.Header.PageType == PageType.Iam)
            {
                allocations.Add(new IamAllocation(page));
            }
            else
            {
                allocations.Add(new Allocation(page));
            }

            this.colour = colour;
        }

        public AllocationLayer FindPage(PageAddress pageAddress, bool findInverted)
        {
            int extentAddress;

            extentAddress = pageAddress.PageId / 8;

            foreach (Allocation alloc in allocations)
            {
                //Check if it's the actual IAM
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

        public static List<string> FindPage(PageAddress page, List<AllocationLayer> layers)
        {
            List<string> layerNames = new List<string>();

            foreach (AllocationLayer layer in layers)
            {
                if (layer.FindPage(page, layer.Invert) != null)
                {
                    layerNames.Add(layer.Name);
                }
            }

            return layerNames;
        }

        public static void RefreshLayers(List<AllocationLayer> layers)
        {
            foreach (AllocationLayer layer in layers)
            {
                foreach (Allocation page in layer.Allocations)
                {
                    page.Refresh();
                }
            }
        }

        public static AllocationLayer CreateLayer(string name,
                                                    bool invert,
                                                    bool transparent,
                                                    Color colour,
                                                    PageAddress firstPage)
        {
            Allocation allocation = new Allocation(ServerConnection.CurrentConnection().CurrentDatabase, firstPage);

            return CreateLayer(name, invert, transparent, colour, allocation);
        }

        public static AllocationLayer CreateLayer(string name,
                                                    bool invert,
                                                    bool transparent,
                                                    Color colour,
                                                    Allocation allocation)
        {
            AllocationLayer layer = new AllocationLayer(allocation);
            layer.Invert = invert;
            layer.Transparent = transparent;
            layer.Name = name;
            layer.Colour = colour;

            return layer;
        }

        #region Properties

        public bool Transparent
        {
            get { return transparent; }
            set { transparent = value; }
        }

        public AllocationLayerType LayerType
        {
            get { return layerType; }
            set { layerType = value; }
        }

        public Color Colour
        {
            get
            {
                if (transparent)
                {
                    return Color.FromArgb(transparency, colour);
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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Allocation> Allocations
        {
            get { return allocations; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public bool Invert
        {
            get { return invert; }
            set { invert = value; }
        }

        public bool UseDefaultSinglePageColour
        {
            get { return useDefaultSinglePageColour; }
            set { useDefaultSinglePageColour = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public Color BorderColour
        {
            get { return borderColour; }
            set { borderColour = value; }
        }

        public bool UseBorderColour
        {
            get { return useBorderColour; }
            set { useBorderColour = value; }
        }

        public bool SingleSlotsOnly
        {
            get { return singleSlotsOnly; }
            set { singleSlotsOnly = value; }
        }

        public int Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }

        #endregion

    }

    public enum AllocationLayerType
    {
        Standard,
        TopLeftCorner
    }
}

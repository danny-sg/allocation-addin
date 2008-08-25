using System.Drawing;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    /// <summary>
    /// Colour range for a KeyImageColumn
    /// </summary>
    internal class ColourRange
    {
        private int from;
        private int to;
        private Color colour;

        internal ColourRange(int from, int to, Color colour)
        {
            this.From = from;
            this.To = to;
            this.Colour = colour;
        }

        public int From
        {
            get { return from; }
            set { from = value; }
        }

        public int To
        {
            get { return to; }
            set { to = value; }
        }

        public Color Colour
        {
            get { return colour; }
            set { colour = value; }
        }

    }
}

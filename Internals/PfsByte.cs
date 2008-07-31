using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// PFS Byte
    /// </summary>
    public class PfsByte
    {
        private bool allocated;
        private bool ghostRecords;
        private bool iam;
        private bool mixed;
        private SpaceFree pageSpaceFree;

        /// <summary>
        /// Initializes a new instance of the <see cref="PfsByte"/> class.
        /// </summary>
        /// <param name="pageByte">The page byte.</param>
        public PfsByte(byte pageByte)
        {
            BitArray bitArray = new BitArray(new byte[] { pageByte });

            ghostRecords = bitArray[3];
            iam = bitArray[4];
            mixed = bitArray[5];
            allocated = bitArray[6];

            pageSpaceFree = (SpaceFree)(pageByte & 7);
        }

        /// <summary>
        /// Gets or sets the page space free.
        /// </summary>
        /// <value>The page space free.</value>
        public SpaceFree PageSpaceFree
        {
            get { return pageSpaceFree; }
            set { pageSpaceFree = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ghost records exist
        /// </summary>
        /// <value><c>true</c> if [ghost records]; otherwise, <c>false</c>.</value>
        public bool GhostRecords
        {
            get { return ghostRecords; }
            set { ghostRecords = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page is an IAM.
        /// </summary>
        /// <value><c>true</c> if iam; otherwise, <c>false</c>.</value>
        public bool Iam
        {
            get { return iam; }
            set { iam = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page is part of a mixed extent.
        /// </summary>
        /// <value><c>true</c> if mixed; otherwise, <c>false</c>.</value>
        public bool Mixed
        {
            get { return mixed; }
            set { mixed = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page is allocated
        /// </summary>
        /// <value><c>true</c> if allocated; otherwise, <c>false</c>.</value>
        public bool Allocated
        {
            get { return allocated; }
            set { allocated = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return
                string.Format(
                    "PFS Status: {0:Allocated ; ;Not Allocated }| {1} Full{2: | IAM Page ; ; }{3:| Mixed Extent ; ; }{4:| Has Ghost ; ; }",
                    allocated ? 1 : 0,
                    SpaceFreeDescription(pageSpaceFree),
                    iam ? 1 : 0,
                    mixed ? 1 : 0,
                    ghostRecords ? 1 : 0);
        }

        /// <summary>
        /// Get the description of the free space
        /// </summary>
        /// <param name="spaceFree">The space free.</param>
        /// <returns></returns>
        public static string SpaceFreeDescription(SpaceFree spaceFree)
        {
            switch (spaceFree)
            {
                case SpaceFree.Empty:
                    return "0%";
                case SpaceFree.FiftyPercent:
                    return "50%";
                case SpaceFree.EightyPercent:
                    return "80%";
                case SpaceFree.NinetyFivePercent:
                    return "95%";
                case SpaceFree.OneHundredPercent:
                    return "100%";
                default:
                    return "Unknown";
            }
        }

        #region SpaceFree enum

        /// <summary>
        /// The space free indicated on the PFS byte
        /// </summary>
        public enum SpaceFree : byte
        {
            /// <summary>
            /// 0%
            /// </summary>
            Empty = 0x00,
            
            /// <summary>
            /// 1%-50%
            /// </summary>
            FiftyPercent = 0x01, 
           
            /// <summary>
            /// 51%-80%
            /// </summary>
            EightyPercent = 0x02, 
            
            /// <summary>
            /// 81%-95%
            /// </summary>
            NinetyFivePercent = 0x03,
            
            /// <summary>
            /// 96-100%
            /// </summary>
            OneHundredPercent = 0x04
        }

        #endregion
    }
}

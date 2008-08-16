using System;
using System.Text;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// Database LSN (Log Sequence Number)
    /// </summary>
    public struct LogSequenceNumber : IComparable<LogSequenceNumber>
    {
        private readonly int fileOffset;
        private readonly int recordSequence;
        private readonly int virtualLogFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSequenceNumber"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public LogSequenceNumber(byte[] value)
        {
            this.virtualLogFile = BitConverter.ToInt32(value, 0);
            this.fileOffset = BitConverter.ToInt32(value, 4);
            this.recordSequence = BitConverter.ToInt16(value, 8);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSequenceNumber"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public LogSequenceNumber(string value)
        {
            StringBuilder sb = new StringBuilder(value);
            sb.Replace("(", string.Empty);
            sb.Replace(")", string.Empty);

            string[] splitAddress = sb.ToString().Split(@":".ToCharArray());

            if (splitAddress.Length != 3)
            {
                throw new ArgumentException(Resources.Exception_InvalidFormat);
            }

            this.virtualLogFile = int.Parse(splitAddress[0]);
            this.fileOffset = int.Parse(splitAddress[1]);
            this.recordSequence = int.Parse(splitAddress[2]);
        }

        /// <summary>
        /// Returns a string representation of the instance
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return string.Format("({0}:{1}:{2})", this.virtualLogFile, this.fileOffset, this.recordSequence);
        }

        /// <summary>
        /// Returns a binary string representation of the instance
        /// </summary>
        /// <returns></returns>
        public string ToBinaryString()
        {
            return string.Format("{0:X8}:{1:X8}:{2:X4}", this.virtualLogFile, this.fileOffset, this.recordSequence);
        }

        /// <summary>
        /// Returns a Decimal representation of the instance
        /// </summary>
        /// <returns></returns>
        public decimal ToDecimal()
        {
            return decimal.Parse(string.Format("{0}{1:0000000000}{2:00000}",
                                               this.virtualLogFile,
                                               this.fileOffset,
                                               this.recordSequence));
        }

        /// <summary>
        /// Returns a Decimal representation (file offset only) of the instance
        /// </summary>
        /// <returns></returns>
        public decimal ToDecimalFileOffsetOnly()
        {
            return decimal.Parse(string.Format("{0}{1:0000000000}", this.virtualLogFile, this.fileOffset));
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>.
        /// </returns>
        int IComparable<LogSequenceNumber>.CompareTo(LogSequenceNumber other)
        {
            return this.fileOffset.CompareTo(other.virtualLogFile)
                   + this.recordSequence.CompareTo(other.fileOffset)
                   + this.recordSequence.CompareTo(other.recordSequence);
        }
    }
}

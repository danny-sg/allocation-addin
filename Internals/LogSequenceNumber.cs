namespace SqlInternals.AllocationInfo.Internals
{
    using System;
    using System.Text;

    using SqlInternals.AllocationInfo.Internals.Properties;

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
            virtualLogFile = BitConverter.ToInt32(value, 0);
            fileOffset = BitConverter.ToInt32(value, 4);
            recordSequence = BitConverter.ToInt16(value, 8);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSequenceNumber"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public LogSequenceNumber(string value)
        {
            var sb = new StringBuilder(value);
            sb.Replace("(", string.Empty);
            sb.Replace(")", string.Empty);

            var splitAddress = sb.ToString().Split(@":".ToCharArray());

            if (splitAddress.Length != 3)
            {
                throw new ArgumentException(Resources.Exception_InvalidFormat);
            }

            virtualLogFile = int.Parse(splitAddress[0]);
            fileOffset = int.Parse(splitAddress[1]);
            recordSequence = int.Parse(splitAddress[2]);
        }

        /// <summary>
        /// Returns a string representation of the instance
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return $"({virtualLogFile}:{fileOffset}:{recordSequence})";
        }

        /// <summary>
        /// Returns a binary string representation of the instance
        /// </summary>
        public string ToBinaryString()
        {
            return $"{virtualLogFile:X8}:{fileOffset:X8}:{recordSequence:X4}";
        }

        /// <summary>
        /// Returns a Decimal representation of the instance
        /// </summary>
        public decimal ToDecimal()
        {
            return decimal.Parse($"{virtualLogFile}{fileOffset:0000000000}{recordSequence:00000}");
        }

        /// <summary>
        /// Returns a Decimal representation (file offset only) of the instance
        /// </summary>
        public decimal ToDecimalFileOffsetOnly()
        {
            return decimal.Parse($"{virtualLogFile}{fileOffset:0000000000}");
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following 
        /// meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is 
        /// equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>.
        /// </returns>
        int IComparable<LogSequenceNumber>.CompareTo(LogSequenceNumber other)
        {
            return fileOffset.CompareTo(other.virtualLogFile) + recordSequence.CompareTo(other.fileOffset)
                   + recordSequence.CompareTo(other.recordSequence);
        }
    }
}
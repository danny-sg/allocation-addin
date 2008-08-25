using System;

namespace SqlInternals.AllocationInfo.Internals.Compression
{
    /// <summary>
    /// SQL Server 2008 Compression Types
    /// </summary>
    [Flags]
    public enum CompressionType : byte
    {
        None = 0,

        /// <summary>
        /// Row Compression
        /// </summary>
        Row = 1,
        
        /// <summary>
        /// Page Compression
        /// </summary>
        Page = 2
    }
}

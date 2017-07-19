namespace SqlInternals.AllocationInfo.Internals.Compression
{
    using System;

    /// <summary>
    /// SQL Server Compression Types
    /// </summary>
    [Flags]
    public enum CompressionType : byte
    {
        /// <summary>
        /// No Compression
        /// </summary>
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
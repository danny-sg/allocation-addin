using System;
using System.Collections.Generic;
using System.Text;

namespace SqlInternals.AllocationInfo.Internals.Compression
{
    [Flags]
    public enum CompressionType : byte
    {
        None = 0,
        Row = 1,
        Page = 2
    }
}

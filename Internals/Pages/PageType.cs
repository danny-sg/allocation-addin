
namespace SqlInternals.AllocationInfo.Internals.Pages
{
    /// <summary>
    /// Different types of SQL Server database pages
    /// </summary>
    public enum PageType
    {
        None = 0,
        
        /// <summary>
        /// File Header
        /// </summary>
        FileHeader = 15,
        
        /// <summary>
        /// Data Page
        /// </summary>
        Data = 1,
        
        /// <summary>
        /// Index Page
        /// </summary>
        Index = 2,
        
        /// <summary>
        /// Large Object Page
        /// </summary>
        Lob3 = 3,
        
        /// <summary>
        /// Large Object Page
        /// </summary>
        Lob4 = 4,
        
        /// <summary>
        /// Used for query processing
        /// </summary>
        Sort = 7,
        
        /// <summary>
        /// Global Allocation Map
        /// </summary>
        Gam = 8,
        
        /// <summary>
        /// Shared Global Allocation Map
        /// </summary>
        Sgam = 9,
        
        /// <summary>
        /// Index Allocation Map
        /// </summary>
        Iam = 10,
        
        /// <summary>
        /// Page Free Space
        /// </summary>
        Pfs = 11,
        
        /// <summary>
        /// Boot page
        /// </summary>
        Boot = 13,
        
        /// <summary>
        /// Differential Changed Map
        /// </summary>
        Dcm = 16,
        
        /// <summary>
        /// Bulk Changed Map
        /// </summary>
        Bcm = 17
    }
}

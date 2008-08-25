﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlInternals.AllocationInfo.Internals.Properties
{


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SqlInternals.AllocationInfo.Internals.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Bitmap allocMap2 {
            get {
                object obj = ResourceManager.GetObject("allocMap2", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap bufferpool11 {
            get {
                object obj = ResourceManager.GetObject("bufferpool11", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap DataTables {
            get {
                object obj = ResourceManager.GetObject("DataTables", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error converting data.
        /// </summary>
        internal static string Exception_ConvertingDataMessage {
            get {
                return ResourceManager.GetString("Exception_ConvertingDataMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid format.
        /// </summary>
        internal static string Exception_InvalidFormat {
            get {
                return ResourceManager.GetString("Exception_InvalidFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid GUID.
        /// </summary>
        internal static string Exception_InvalidGuid {
            get {
                return ResourceManager.GetString("Exception_InvalidGuid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified login does not have the required SYSADMIN role..
        /// </summary>
        internal static string Exception_NotSysadmin {
            get {
                return ResourceManager.GetString("Exception_NotSysadmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to not yet supported ({0:G}).
        /// </summary>
        internal static string Exception_NotYetSupportedMessage {
            get {
                return ResourceManager.GetString("Exception_NotYetSupportedMessage", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap RefreshDocView {
            get {
                object obj = ResourceManager.GetObject("RefreshDocView", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT s.name+&apos;.&apos;+o.name AS Alloc_Unit
        ///FROM   sys.allocation_units au
        ///       INNER JOIN sys.partitions p ON au.container_id = p.partition_id
        ///       INNER JOIN sys.objects o ON p.object_id = o.object_id
        ///       INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
        ///WHERE  au.allocation_unit_id = @allocation_unit_id.
        /// </summary>
        internal static string SQL_Allocation_Unit {
            get {
                return ResourceManager.GetString("SQL_Allocation_Unit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT o.object_id
        ///	  ,s.name AS schema_name
        ///	  ,o.name AS table_name
        ///	  ,is_ms_shipped AS system 
        ///	  ,iau.first_iam_page
        ///                     ,p.index_id
        ///	  ,CASE iau.type
        ///			WHEN 1 THEN &apos;Row Data&apos;
        ///			WHEN 2 THEN &apos;LOB Data&apos;
        ///			WHEN 3 THEN &apos;Row Overflow Data&apos;
        ///	   END AS type_description
        ///	  ,iau.type
        ///FROM   sys.all_objects o
        ///	   INNER JOIN sys.schemas s ON o.schema_id = s.schema_id 
        ///	   INNER JOIN sys.partitions p ON p.object_id = o.object_id
        ///	   INNER JOIN sys.system_internals_allocation_unit [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_Allocation_Units {
            get {
                return ResourceManager.GetString("SQL_Allocation_Units", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT file_id
        ///      ,page_id
        ///      ,is_modified
        ///      ,row_count
        ///      ,free_space_in_bytes
        ///FROM   sys.dm_os_buffer_descriptors WITH (NOLOCK)
        ///WHERE  database_id = DB_ID(@database).
        /// </summary>
        internal static string SQL_Buffer_Pool {
            get {
                return ResourceManager.GetString("SQL_Buffer_Pool", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT ISNULL(data_compression, 0) FROM sys.partitions WHERE partition_id = @partition_id.
        /// </summary>
        internal static string SQL_Compression {
            get {
                return ResourceManager.GetString("SQL_Compression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT name FROM sys.databases WHERE database_id = @database_id.
        /// </summary>
        internal static string SQL_Database {
            get {
                return ResourceManager.GetString("SQL_Database", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT @@SERVERNAME AS servername, SERVERPROPERTY(&apos;productversion&apos;) AS productversion, SERVERPROPERTY (&apos;productlevel&apos;) AS productlevel, SERVERPROPERTY (&apos;edition&apos;) AS edition, SUSER_NAME() AS username.
        /// </summary>
        internal static string SQL_Database_Properties {
            get {
                return ResourceManager.GetString("SQL_Database_Properties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT size FROM sys.databases d  INNER JOIN sys.master_files mf ON d.database_id = mf.database_id WHERE type = 0 AND d.database_id = @database_id.
        /// </summary>
        internal static string SQL_Database_Size {
            get {
                return ResourceManager.GetString("SQL_Database_Size", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT o.object_id
        ///	  ,s.name AS schema_name
        ///	  ,o.name AS table_name
        ///	  ,is_ms_shipped AS system 
        ///FROM   sys.objects o 
        ///	   INNER JOIN sys.schemas s ON o.schema_id = s.schema_id 
        ///WHERE  type IN (&apos;U&apos;,&apos;S&apos;)
        ///ORDER BY is_ms_shipped desc, s.name asc , o.name asc.
        /// </summary>
        internal static string SQL_Database_Tables {
            get {
                return ResourceManager.GetString("SQL_Database_Tables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT   d.database_id
        ///	    ,d.name
        ///        ,SUM(size) AS Size
        ///        ,d.state 
        ///	    ,COUNT(*) AS DataFiles
        ///       ,d.compatibility_level
        ///FROM     sys.databases d  
        ///	     INNER JOIN sys.master_files mf ON d.database_id = mf.database_id 
        ///WHERE    type = 0
        ///GROUP BY d.database_id
        ///		,d.name
        ///		,d.state 
        ///        ,compatibility_level   
        ///ORDER BY d.name.
        /// </summary>
        internal static string SQL_Databases {
            get {
                return ResourceManager.GetString("SQL_Databases", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT size FROM sys.database_files WHERE file_id = @file_id.
        /// </summary>
        internal static string SQL_File_Size {
            get {
                return ResourceManager.GetString("SQL_File_Size", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE #FileStats
        ///            (file_id int
        ///            ,filegroup int  
        ///            ,total_extents int
        ///            ,used_extents int 
        ///            ,name sysname
        ///            ,file_name nchar(520))
        ///INSERT #FileStats EXEC (&apos;dbcc showfilestats&apos;)
        ///		
        ///SELECT df.file_id
        ///      ,df.type
        ///      ,fg.name as filegroup_name
        ///      ,df.name
        ///      ,physical_name
        ///      ,size
        ///      ,total_extents
        ///      ,used_extents
        ///FROM   sys.database_files df
        ///       INNER JOIN sys.filegroups fg ON df.data_space_id = fg [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_Files {
            get {
                return ResourceManager.GetString("SQL_Files", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- The reason this is necessary is that the key columns are in sys.system_internals_partition_columns
        ///-- but there doesn&apos;&apos;&apos;&apos;t seem to be a link to which column they are. 
        ///-- 
        ///-- Let me know at danny@sqlinternalsviewer.com if you can think of a better way!
        ///WITH IndexColumns AS
        ///(
        ///SELECT ipc.partition_column_id
        ///	  ,i.type
        ///      ,i.index_id
        ///      ,p.object_id
        ///      ,CASE WHEN is_uniqueifier = 1 THEN &apos;Uniqueifier&apos; ELSE ac.name END AS name
        ///      ,ipc.system_type_id
        ///      ,ipc.max_length
        ///      ,ipc.pr [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_Index_Structure {
            get {
                return ResourceManager.GetString("SQL_Index_Structure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT ISNULL(OBJECTPROPERTY(@object_id,&apos;TableHasClustIndex&apos;),0)
        ///.
        /// </summary>
        internal static string SQL_Index_Type {
            get {
                return ResourceManager.GetString("SQL_Index_Type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [Current LSN] AS LSN
        ///	  ,REPLACE(SUBSTRING(Operation, 5, LEN(Operation)),&apos;_&apos;,&apos; &apos;) AS Operation
        ///	  ,CASE WHEN Context = &apos;LCX_NULL&apos; THEN NULL
        ///			ELSE REPLACE(SUBSTRING(Context, 5, LEN(Context)),&apos;_&apos;,&apos; &apos;) 
        ///	   END AS Context
        ///	  ,AllocUnitId
        ///      ,AllocUnitName
        ///	  ,[Page ID] AS PageId
        ///	  ,[Slot ID] AS SlotId
        ///	  ,RowFlags
        ///	  ,[Num Elements] AS NumElements
        ///	  ,[Offset in Row] AS OffsetInRow
        ///	  ,[Rowbits First Bit] AS RowbitsFirstBit
        ///	  ,[Rowbits Bit Count] AS RowbitsBitCount
        ///	  ,[Rowbits Bit  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_Log {
            get {
                return ResourceManager.GetString("SQL_Log", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DBCC PAGE({0},{1},{2},{3}) WITH TABLERESULTS.
        /// </summary>
        internal static string SQL_Page {
            get {
                return ResourceManager.GetString("SQL_Page", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT index_depth
        ///      ,index_level
        ///      ,avg_fragmentation_in_percent
        ///      ,fragment_count
        ///      ,avg_fragment_size_in_pages
        ///      ,page_count
        ///      ,avg_page_space_used_in_percent
        ///      ,record_count
        ///      ,ghost_record_count
        ///      ,version_ghost_record_count
        ///      ,min_record_size_in_bytes
        ///      ,max_record_size_in_bytes
        ///      ,avg_record_size_in_bytes
        ///      ,forwarded_record_count
        ///FROM   sys.dm_db_index_physical_stats(DB_ID(@database_name), @object_id, @index_id, NULL, &apos;DETAILED&apos;).
        /// </summary>
        internal static string SQL_Physical_Stats {
            get {
                return ResourceManager.GetString("SQL_Physical_Stats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WITH Total AS
        ///    (
        ///    SELECT SUM(total_pages) * 8192 / 1048576. AS TotalFileMb
        ///    FROM   sys.allocation_units
        ///    )
        ///SELECT s.name + &apos;.&apos; + o.name              AS ObjectName
        ///      ,o.object_id                        AS ObjectId
        ///      ,MAX(rows)                          AS [Rows]
        ///      ,SUM(total_pages)                   AS TotalPages
        ///      ,SUM(used_pages)                    AS UsedPages
        ///      ,SUM(data_pages)                    AS DataPages
        ///      ,SUM(total_pages) * 8192 / 1048576. AS TotalMb
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_SpaceUsed {
            get {
                return ResourceManager.GetString("SQL_SpaceUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WITH Total AS
        ///    (
        ///    SELECT SUM(total_pages) * 8192 / 1048576. AS TotalFileMb
        ///    FROM   sys.allocation_units
        ///    ),
        ///Fragmentation AS
        ///	(
        ///    SELECT object_id
        ///          ,AVG(avg_fragmentation_in_percent) AS avg_fragmentation_in_percent
        ///    FROM   sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, DEFAULT)
        ///    GROUP BY object_id		
        ///	)
        ///SELECT s.name + &apos;.&apos; + o.name              AS ObjectName
        ///      ,o.object_id                        AS ObjectId
        ///      ,MAX(rows)                          AS [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_SpaceUsedAdvanced {
            get {
                return ResourceManager.GetString("SQL_SpaceUsedAdvanced", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT CONVERT(BIT, IS_SRVROLEMEMBER(&apos;sysadmin&apos;)).
        /// </summary>
        internal static string SQL_Sysadmin_Check {
            get {
                return ResourceManager.GetString("SQL_Sysadmin_Check", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT CASE WHEN is_uniqueifier = 1 THEN &apos;Uniqueifier&apos;
        ///            WHEN is_dropped     = 1 THEN &apos;(Dropped)&apos;
        ///            ELSE c.name END AS name,
        ///       ISNULL(c.column_id,-1) AS column_id,
        ///       TYPE_NAME(pc.system_type_id) as type_name,
        ///       pc.system_type_id,
        ///       pc.max_length,
        ///       pc.precision,
        ///       pc.scale,
        ///       pc.leaf_offset,
        ///       pc.is_uniqueifier,
        ///       pc.is_dropped,
        ///       pc.leaf_null_bit,
        ///       CONVERT(BIT, 0) AS is_sparse
        ///FROM   sys.allocation_units au
        ///       IN [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_Table_Columns_2005 {
            get {
                return ResourceManager.GetString("SQL_Table_Columns_2005", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT CASE WHEN is_uniqueifier = 1 THEN &apos;Uniqueifier&apos;
        ///            WHEN is_dropped     = 1 THEN &apos;(Dropped)&apos;
        ///            ELSE c.name END AS name,
        ///       ISNULL(c.column_id,-1) AS column_id,
        ///       TYPE_NAME(pc.system_type_id) as type_name,
        ///       pc.system_type_id,
        ///       pc.max_length,
        ///       pc.precision,
        ///       pc.scale,
        ///       pc.leaf_offset,
        ///       pc.is_uniqueifier,
        ///       pc.is_dropped,
        ///       ISNULL(c.is_sparse, 0) AS is_sparse,
        ///       pc.leaf_null_bit
        ///FROM   sys.allocation_units au
        ///   [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_Table_Columns_2008 {
            get {
                return ResourceManager.GetString("SQL_Table_Columns_2008", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT o.object_id
        ///      ,s.name AS schema_name
        ///      ,CASE WHEN i.name IS NULL 
        ///            THEN o.name
        ///            ELSE o.name + &apos;.&apos; + i.name 
        ///       END AS object_name
        ///      ,o.type AS object_type
        ///      ,i.type_desc
        ///      ,i.type AS index_type
        ///      ,i.index_id AS index_id
        ///      ,iau.first_iam_page
        ///      ,iau.root_page
        ///      ,iau.first_page
        ///      ,iau.total_pages
        ///      ,iau.used_pages
        ///      ,iau.data_pages
        ///      ,iau.type AS alloc_unit_type
        ///      ,iau.type_desc
        ///      ,partition_number
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQL_Table_Info {
            get {
                return ResourceManager.GetString("SQL_Table_Info", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT SERVERPROPERTY(&apos;productversion&apos;), size FROM sys.database_files WHERE file_id = 1.
        /// </summary>
        internal static string SQL_Version {
            get {
                return ResourceManager.GetString("SQL_Version", resourceCulture);
            }
        }
    }
}

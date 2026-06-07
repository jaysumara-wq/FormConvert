using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchemaEditorCore.Models
{
    /// <summary>
    /// Represents a hierarchical domain in the schema
    /// </summary>
    public class SchemaDomain
    {
        public int DomainID { get; set; }
        public string DomainName { get; set; }
        public string Description { get; set; }
        public List<SchemaTable> Tables { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    /// <summary>
    /// Represents a table node in the schema tree
    /// </summary>
    public class SchemaTable
    {
        public int TableID { get; set; }
        public int DomainID { get; set; }
        public int ParentTableID { get; set; }
        public string TableName { get; set; }
        public string TableCode { get; set; }
        public string TablePlural { get; set; }
        public string TableDesc { get; set; }
        public int TableStructure { get; set; }
        public int TableType { get; set; }
        public int FloatTableYN { get; set; }
        public int TableFunction { get; set; }
        public int SystemServiceYN { get; set; }
        public List<SchemaSlot> Slots { get; set; } = new();
        public List<SchemaTable> Children { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    /// <summary>
    /// Represents a slot (field/column) in a table
    /// </summary>
    public class SchemaSlot
    {
        public int SlotID { get; set; }
        public int TableID { get; set; }
        public string SlotCode { get; set; }
        public string SlotLabel { get; set; }
        public string SlotPlural { get; set; }
        public string SlotDesc { get; set; }
        public int SlotDataType { get; set; }
        public int SlotDataSubType { get; set; }
        public int SlotDataFormat { get; set; }
        public int SlotDocType { get; set; }
        public int NotNullYN { get; set; }
        public int FixedValuesSlotYN { get; set; }
        public int DisplaySlotYN { get; set; }
        public int SystemSlotYN { get; set; }
        public int DoNotDisplayYN { get; set; }
        public int MapKeyID { get; set; }
        public int SlotFunction { get; set; }
        public int ImportBySlotID { get; set; }
        public int ExportBySlotID { get; set; }
        public int ReferenceTableID { get; set; }
        public int InheritSlotID { get; set; }
        public int InheritBaseSlotID { get; set; }
        public int AutoAggrYN { get; set; }
        public int AutoFetchYN { get; set; }
        public int AutoCompYN { get; set; }
        public int AutoFillYN { get; set; }
        public int IDSlotYN { get; set; }
        public int CodeSlotYN { get; set; }
        public List<SchemaChoice> Choices { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    /// <summary>
    /// Represents a choice value for a slot
    /// </summary>
    public class SchemaChoice
    {
        public int ChoiceID { get; set; }
        public int SlotID { get; set; }
        public string Choice { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Represents a reference relationship
    /// </summary>
    public class SchemaReference
    {
        public int SlotID { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
        public string SlotLabel { get; set; }
        public int ReferenceTableID { get; set; }
    }

    /// <summary>
    /// Application schema containing all domains and tables
    /// </summary>
    public class ApplicationSchema
    {
        public string ApplicationDesc { get; set; }
        public string SchemaName { get; set; }
        public int DesignLevel { get; set; }
        public bool PrivateDomainYN { get; set; }
        public string GenerateMode { get; set; }
        public List<SchemaDomain> Domains { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    /// <summary>
    /// Tree node view model
    /// </summary>
    public class TreeNodeItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string NodeType { get; set; }
        public int DomainID { get; set; }
        public int TableID { get; set; }
        public List<TreeNodeItem> Children { get; set; } = new();
        public string ToolTip { get; set; }
        public bool IsExpanded { get; set; }
    }

    /// <summary>
    /// Cell event arguments
    /// </summary>
    public class DataGridCellEventArgs
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public object CellValue { get; set; }
        public object RowData { get; set; }
    }

    /// <summary>
    /// Denormalization request
    /// </summary>
    public class DenormalizeRequest
    {
        public int TableID { get; set; }
        public string Mode { get; set; }
        public string Values { get; set; }
    }
}

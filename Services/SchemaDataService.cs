using SchemaEditorCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchemaEditorCore.Services
{
    public class SchemaDataService
    {
        private ApplicationSchema _applicationSchema;
        private Dictionary<int, SchemaTable> _tableCache = new();
        private Dictionary<int, SchemaSlot> _slotCache = new();

        public SchemaDataService()
        {
        }

        public void Initialize(ApplicationSchema schema)
        {
            _applicationSchema = schema;
            BuildCache();
        }

        private void BuildCache()
        {
            foreach (var domain in _applicationSchema.Domains)
            {
                BuildTableCache(domain.Tables);
            }
        }

        private void BuildTableCache(List<SchemaTable> tables)
        {
            foreach (var table in tables)
            {
                _tableCache[table.TableID] = table;
                foreach (var slot in table.Slots)
                {
                    _slotCache[slot.SlotID] = slot;
                }
                BuildTableCache(table.Children);
            }
        }

        public List<TreeNodeItem> GetTreeNodes()
        {
            var nodes = new List<TreeNodeItem>();
            foreach (var domain in _applicationSchema.Domains)
            {
                var domainNode = new TreeNodeItem
                {
                    Id = $"DomainNode:{domain.DomainID}",
                    Label = domain.DomainName,
                    NodeType = "DomainNode",
                    DomainID = domain.DomainID
                };

                foreach (var table in domain.Tables)
                {
                    domainNode.Children.Add(CreateTableNode(table));
                }

                nodes.Add(domainNode);
            }
            return nodes;
        }

        private TreeNodeItem CreateTableNode(SchemaTable table)
        {
            var node = new TreeNodeItem
            {
                Id = $"TableNode:{table.DomainID}:{table.TableID}",
                Label = table.TableName,
                NodeType = "TableNode",
                DomainID = table.DomainID,
                TableID = table.TableID
            };

            foreach (var child in table.Children)
            {
                node.Children.Add(CreateTableNode(child));
            }

            return node;
        }

        public SchemaTable GetTable(int tableId)
        {
            return _tableCache.TryGetValue(tableId, out var table) ? table : null;
        }

        public SchemaSlot GetSlot(int slotId)
        {
            return _slotCache.TryGetValue(slotId, out var slot) ? slot : null;
        }

        public List<SchemaSlot> GetTableSlots(int tableId)
        {
            var table = GetTable(tableId);
            return table?.Slots ?? new List<SchemaSlot>();
        }

        public List<SchemaChoice> GetSlotChoices(int slotId)
        {
            var slot = GetSlot(slotId);
            return slot?.Choices ?? new List<SchemaChoice>();
        }

        public List<SchemaReference> GetTableReferences(int tableId)
        {
            var references = new List<SchemaReference>();
            var table = GetTable(tableId);
            if (table == null) return references;

            foreach (var slotKvp in _slotCache)
            {
                if (slotKvp.Value.ReferenceTableID == tableId)
                {
                    var referencingTable = GetTable(slotKvp.Value.TableID);
                    references.Add(new SchemaReference
                    {
                        SlotID = slotKvp.Value.SlotID,
                        TableID = slotKvp.Value.TableID,
                        TableName = referencingTable?.TableName,
                        SlotLabel = slotKvp.Value.SlotLabel,
                        ReferenceTableID = tableId
                    });
                }
            }

            return references;
        }

        public void AddSlot(int tableId, SchemaSlot slot)
        {
            var table = GetTable(tableId);
            if (table != null)
            {
                slot.TableID = tableId;
                slot.SlotID = table.Slots.Count > 0 ? table.Slots.Max(s => s.SlotID) + 1 : 1;
                table.Slots.Add(slot);
                _slotCache[slot.SlotID] = slot;
            }
        }

        public void DeleteSlot(int slotId)
        {
            if (_slotCache.TryGetValue(slotId, out var slot))
            {
                var table = GetTable(slot.TableID);
                if (table != null)
                {
                    table.Slots.RemoveAll(s => s.SlotID == slotId);
                    _slotCache.Remove(slotId);
                }
            }
        }

        public void AddChoice(int slotId, string choiceValue)
        {
            var slot = GetSlot(slotId);
            if (slot != null)
            {
                var choice = new SchemaChoice
                {
                    ChoiceID = slot.Choices.Count > 0 ? slot.Choices.Max(c => c.ChoiceID) + 1 : 1,
                    SlotID = slotId,
                    Choice = choiceValue,
                    SortOrder = slot.Choices.Count
                };
                slot.Choices.Add(choice);
            }
        }

        public void DeleteChoice(int choiceId)
        {
            foreach (var slot in _slotCache.Values)
            {
                slot.Choices.RemoveAll(c => c.ChoiceID == choiceId);
            }
        }

        public void DenormalizeTable(int tableId, string mode, string values)
        {
            var table = GetTable(tableId);
            if (table == null) return;

            var references = GetTableReferences(tableId);

            foreach (var reference in references)
            {
                var referencingSlot = GetSlot(reference.SlotID);
                if (referencingSlot == null) continue;

                var referencingTable = GetTable(reference.TableID);
                if (referencingTable == null) continue;

                // Create new denormalized slot
                var newSlot = new SchemaSlot
                {
                    SlotID = _slotCache.Count + 1,
                    TableID = reference.TableID,
                    SlotCode = referencingSlot.SlotCode,
                    SlotLabel = referencingSlot.SlotLabel,
                    SlotPlural = referencingSlot.SlotPlural,
                    SlotDesc = referencingSlot.SlotDesc,
                    FixedValuesSlotYN = mode == "Choice" ? 1 : 0,
                    SlotDataType = mode == "Choice" ? 3 : (mode == "Numeric" ? 3 : 1),
                    SlotDataSubType = mode == "Numeric" ? 2 : 1
                };

                referencingTable.Slots.Add(newSlot);
                _slotCache[newSlot.SlotID] = newSlot;

                // Add choices if needed
                if (mode == "Choice" && !string.IsNullOrEmpty(values))
                {
                    foreach (var value in values.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            AddChoice(newSlot.SlotID, value.Trim());
                        }
                    }
                }
            }

            // Remove the original reference slot
            DeleteSlot(references.FirstOrDefault()?.SlotID ?? -1);
        }

        public string GetNodeProperties(int tableId)
        {
            var table = GetTable(tableId);
            if (table == null) return "";

            var properties = new System.Text.StringBuilder();
            properties.AppendLine($"TableID:\t{table.TableID}");
            properties.AppendLine($"TableName:\t{table.TableName}");
            properties.AppendLine($"TableCode:\t{table.TableCode}");
            properties.AppendLine($"TableType:\t{table.TableType}");
            properties.AppendLine($"Structure:\t{table.TableStructure}");
            properties.AppendLine($"Float:\t{table.FloatTableYN}");
            properties.AppendLine($"Function:\t{table.TableFunction}");
            properties.AppendLine($"System Service:\t{table.SystemServiceYN}");
            properties.AppendLine($"Created:\t{table.CreatedAt}");
            properties.AppendLine($"Modified:\t{table.ModifiedAt}");

            return properties.ToString();
        }
    }
}

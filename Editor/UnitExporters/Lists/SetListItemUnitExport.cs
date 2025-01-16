using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Export;

namespace Editor.UnitExporters.Lists
{
    public class SetListItemUnitExport : IUnitExporter
    {
        public Type unitType { get => typeof(SetListItem); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new SetListItemUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as SetListItem;

            var list = ListHelpers.FindListByConnections(unitExporter.exportContext, unit);
            if (list == null)
            {
                Debug.LogError("Could not find list for SetListItem");
                return;
            }
            
            ListHelpers.SetItem(unitExporter, list, unit.index, unit.item, unit.enter, unit.exit);
        }
    }
}
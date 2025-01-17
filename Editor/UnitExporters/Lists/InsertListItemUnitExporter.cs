using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Export;

namespace Editor.UnitExporters.Lists
{
    public class InsertListItemUnitExporter : IUnitExporter
    {
        public Type unitType { get => typeof(InsertListItem); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new InsertListItemUnitExporter());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as InsertListItem;
            
            var list = ListHelpers.FindListByConnections(unitExporter.exportContext, unit);
            if (list == null)
            {
                Debug.LogError("Could not find list for InsertItem unit");
                return;
            }
            
            ListHelpers.InsertItem(unitExporter, list, unit.index, unit.item, unit.enter, unit.exit);
            unitExporter.ByPassValue(unit.listInput, unit.listOutput);
        }
    }
}
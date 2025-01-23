using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity;
using UnityGLTF.Interactivity.Export;

namespace Editor.UnitExporters.Lists
{
    public class AddListItemUnitExport : IUnitExporter, IUnitExporterFeedback
    {
        public Type unitType { get => typeof(AddListItem); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new AddListItemUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as AddListItem;

            var list = ListHelpers.FindListByConnections(unitExporter.exportContext, unit);
            if (list == null)
            {
                Debug.LogError("Could not find list for SetListItem");
                return;
            }
            
            ListHelpers.AddItem(unitExporter, list, unit.item, unit.enter, unit.exit);
            unitExporter.ByPassValue(unit.listInput, unit.listOutput);
        }

        public UnitLogs GetFeedback(IUnit unit)
        {
            var logs = new UnitLogs();
            logs.infos.Add("Be aware that exported lists will be limited in size to the capacity on export.");
            
            return logs;
        }
    }
}
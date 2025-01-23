using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity;
using UnityGLTF.Interactivity.Export;

namespace Editor.UnitExporters.Lists
{
    public class CreateListUnitExport : IUnitExporter, IUnitExporterFeedback
    {
        public Type unitType { get => typeof(CreateList); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new CreateListUnitExport());
        }
        
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as CreateList;

            var listCapacity = unit.inputCount;

            var firstInput = unit.validInputs.FirstOrDefault();
            if (firstInput == null)
            {
                Debug.LogError("Can't resolve list detection by connections.");
                return;
            }

            if (!unitExporter.IsInputLiteralOrDefaultValue(firstInput as ValueInput, out var value))
            {
                Debug.LogError("Currently only literals are supported for list creation.");
                return;
            }

            unitExporter.ConvertValue(value, out _, out var valueTypeIndex);
            
            var objectList = unitExporter.exportContext.CreateNewVariableBasedList(listCapacity, valueTypeIndex);
            objectList.listCreatorUnit = unit;
            objectList.listCreatorGraph = unitExporter.exportContext.currentGraphProcessing;
            
            ListHelpers.CreateListNodes(unitExporter, objectList);
            
            foreach (var input in unit.validInputs)
            {
                if (unitExporter.IsInputLiteralOrDefaultValue(input as ValueInput, out var inputValue))
                {
                    unitExporter.ConvertValue(inputValue, out var convertedValue, out var convertedTypeIndex);
                    objectList.AddItem(convertedValue);
                }
            }
            
        }

        public UnitLogs GetFeedback(IUnit unit)
        {
            var logs = new UnitLogs();
            logs.infos.Add("Exported list capacity will be limited to the size: "+(unit as CreateList).inputCount);
            return logs;
        }
    }
}
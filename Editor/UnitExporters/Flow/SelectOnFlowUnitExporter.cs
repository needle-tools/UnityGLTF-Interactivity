using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class SelectOnFlowUnitExporter : IUnitExporter
    {
        public Type unitType { get => typeof(SelectOnFlow); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new SelectOnFlowUnitExporter());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as SelectOnFlow;

            if (unit.branchCount == 0)
            {
                UnitExportLogging.AddWarningLog(unit, "No branches. Will be skipped.");
                return;
            }
            
            var setVarNodes = new List<GltfInteractivityUnitExporterNode>();
            
            // Temporary set a Type Index, will be updated later in OnNodesCreated callback,
            // when it's possible to identify the type from then connected inputs
            int typeIndex = 0;
            
            // using VariableKind.Scene, because we already generated a unique name for the variable
            var varIndex = unitExporter.exportContext.AddVariableWithIdIfNeeded($"SelectOnFlowValue_{GUID.Generate().ToString()}", null, VariableKind.Scene, typeIndex);

            var getVar = VariablesHelpers.GetVariable(unitExporter, varIndex, out var getVarValue);
            getVarValue.MapToPort(unit.selection);
            
            for (int i = 0; i < unit.branchCount; i++)
            {
                var setVar = VariablesHelpers.SetVariable(unitExporter, varIndex, unit.valueInputs[i], unit.controlInputs[i], unit.exit);
                setVarNodes.Add(setVar);
            }

            // Update Type Index
            unitExporter.exportContext.OnNodesCreated += (nodes =>
            {
                var typeIndex = unitExporter.exportContext.GetValueTypeForInput(setVarNodes[0], Variable_SetNode.IdInputValue);
                unitExporter.exportContext.variables[varIndex].Type = typeIndex;
                getVar.ValueOut(Variable_GetNode.IdOutputValue).ExpectedType(ExpectedType.GtlfType(typeIndex));
                foreach (var n in setVarNodes)
                {
                    n.ValueIn(Variable_SetNode.IdInputValue).SetType(TypeRestriction.LimitToType(typeIndex));
                }
            });

        }
    }
}
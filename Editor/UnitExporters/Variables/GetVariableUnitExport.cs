using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class GetVariableUnitExport : IUnitExporter
    {
        public Type unitType { get => typeof(GetVariable); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new GetVariableUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as GetVariable;
            var variableIndex = unitExporter.exportContext.AddVariableIfNeeded(unit);
            VariablesHelpers.GetVariable(unitExporter, variableIndex, out var valueSocket);
            valueSocket.MapToPort(unit.value);
        }
    }
}
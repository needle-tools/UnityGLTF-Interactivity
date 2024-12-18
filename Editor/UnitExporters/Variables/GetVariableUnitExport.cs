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
            
            var node = unitExporter.CreateNode(new Variable_GetNode());
            
            var variableIndex = unitExporter.exportContext.AddVariableIfNeeded(unit);
            var varType = unitExporter.exportContext.variables[variableIndex].Type;
            node.OutValueSocketConnectionData[Variable_GetNode.IdOutputValue].expectedType = ExpectedType.GtlfType(varType);
            
            node.ConfigurationData["variable"].Value = variableIndex;
            
            unitExporter.MapValueOutportToSocketName(unit.value, Variable_GetNode.IdOutputValue, node); 
        }
    }
}
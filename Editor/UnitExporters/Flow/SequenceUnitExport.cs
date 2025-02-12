using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class SequenceUnitExport : IUnitExporter
    {
        public Type unitType { get => typeof(Sequence); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new SequenceUnitExport());
        }
        
        public bool InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as Sequence;
            var node = unitExporter.CreateNode(new Flow_SequenceNode());
            
            unitExporter.MapInputPortToSocketName(unit.enter, Flow_SequenceNode.IdFlowIn, node);
            
            if (unit.multiOutputs.Count > 0)
            {
                int index = 0;
                foreach (var output in unit.multiOutputs)
                {
                    if (output.connection == null || output.connection.destination == null)
                        continue;
                    
                    var n = new GltfInteractivityUnitExporterNode.FlowSocketData();
                    var desc = new GltfInteractivityNodeSchema.FlowSocketDescriptor();
                    var id = "sequence" + index.ToString("D3");
                    
                    node.FlowSocketConnectionData.Add(id, n);
                    unitExporter.MapOutFlowConnectionWhenValid(output, id, node);
                    
                    index++;
                }
            }
            return true;
        }
    }
}
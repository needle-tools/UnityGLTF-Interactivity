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
            //NodeConvertRegistry.RegisterImport(new OnSelectNode());
            UnitExporterRegistry.RegisterExporter(new SequenceUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
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
                    desc.Id = "sequence" + index;
                    
                    node.FlowSocketConnectionData.Add(desc.Id, n);
                    unitExporter.MapOutFlowConnectionWhenValid(output, desc.Id, node);
                    
                    index++;
                }
            }
            
        }
    }
}
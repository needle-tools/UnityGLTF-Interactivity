using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class CustomEventUnitExport: IUnitExporter
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new CustomEventUnitExport());
        }
        
        public System.Type unitType { get => typeof(CustomEvent); }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            GltfInteractivityUnitExporterNode node = unitExporter.CreateNode(new Event_ReceiveNode());
            var customEvent = unitExporter.unit as CustomEvent;

            unitExporter.MapInputPortToSocketName(customEvent.name, "event", node);
            
            var args = new List<GltfInteractivityUnitExporterNode.EventValues>();
            foreach (var arg in customEvent.argumentPorts)
            {
                var argId = arg.key;
                var argTypeIndex = GltfInteractivityTypeMapping.TypeIndex(arg.type);
                args.Add(new  GltfInteractivityUnitExporterNode.EventValues{Id = argId, Type = argTypeIndex});

                unitExporter.MapValueOutportToSocketName(arg, argId, node);

            }
            
            var index = unitExporter.exportContext.AddEventIfNeeded(customEvent, args.ToArray());
            node.ConfigurationData["event"].Value = index;
            
            unitExporter.MapOutFlowConnectionWhenValid(customEvent.trigger, "out", node);
        }
    }
}
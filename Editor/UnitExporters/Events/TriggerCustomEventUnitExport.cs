using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class TriggerCustomEventUnitExport: IUnitExporter
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new TriggerCustomEventUnitExport());
        }
        
        public System.Type unitType { get => typeof(TriggerCustomEvent); }
        

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var customEvent = unitExporter.unit as TriggerCustomEvent;
            if (!customEvent.target.hasDefaultValue && !customEvent.target.hasValidConnection)
            {
                Debug.LogWarning("Ignoring TriggerCustomEvent node because it has no target");
                return;
            }
            
            GltfInteractivityUnitExporterNode node = unitExporter.CreateNode(new Event_SendNode());
            
            unitExporter.MapInputPortToSocketName(customEvent.name, Event_SendNode.IdEvent, node);
            unitExporter.MapInputPortToSocketName(customEvent.enter, Event_SendNode.IdFlowIn, node);
            
            node.ValueIn("targetNodeIndex").MapToInputPort(customEvent.target).SetType(TypeRestriction.LimitToInt);
            
            var args = new Dictionary<string, GltfInteractivityUnitExporterNode.EventValues>();
            args.Add("targetNodeIndex", new GltfInteractivityUnitExporterNode.EventValues { Type = GltfTypes.TypeIndex("int") });
            
            foreach (var arg in customEvent.arguments)
            {
                var argId = arg.key;
                var argTypeIndex = GltfTypes.TypeIndex(arg.type);
                var eventValue = new GltfInteractivityUnitExporterNode.EventValues { Type = argTypeIndex };
                args.Add(argId, eventValue);
                
                unitExporter.MapInputPortToSocketName(arg, argId, node);
                var valueSocketData = new GltfInteractivityUnitExporterNode.ValueSocketData()
                {
                    Type = argTypeIndex,
                };
                node.ValueSocketConnectionData.Add(argId, valueSocketData);


            }
            var index = unitExporter.exportContext.AddEventIfNeeded(customEvent, args);
            if (index == -1)
            {
                unitExporter.IsTranslatable = false;
                return;
            }
            node.ConfigurationData["event"].Value = index;
            
            unitExporter.exportContext.OnBeforeSerialization += (List<GltfInteractivityNode> nodes) =>
            {
                var customEvent = unitExporter.exportContext.customEvents[index];

                foreach (var argValue in args)
                {
                    var eventValue = customEvent.Values.FirstOrDefault(x => x.Key == argValue.Key);
                    if (eventValue.Value == null || eventValue.Value.Type != -1)
                        continue;
                    
                    var argTypeIndex = unitExporter.exportContext.GetValueTypeForInput(node, argValue.Key);
                    eventValue.Value.Type = argTypeIndex;
                }
            };
            
            unitExporter.MapOutFlowConnectionWhenValid(customEvent.exit, Event_SendNode.IdFlowOut, node);
        }
    }
}
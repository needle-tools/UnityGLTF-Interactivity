using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class TimeUnitExports : IUnitExporter
    {
        public Type unitType { get; }
        private string _eventOnTickSocketName = "eventOnTick";

        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            GetMemberUnitExport.RegisterMemberExporter(typeof(Time), nameof(Time.deltaTime), new TimeUnitExports(Event_OnTickNode.IdOutTimeSinceLastTick));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Time), nameof(Time.realtimeSinceStartup), new TimeUnitExports(Event_OnTickNode.IdOutTimeSinceStart));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Time), nameof(Time.timeSinceLevelLoad), new TimeUnitExports(Event_OnTickNode.IdOutTimeSinceStart));
        }

        public TimeUnitExports(string eventOnTickSocketName)
        {
            _eventOnTickSocketName = eventOnTickSocketName;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as GetMember;
            
            var deltaTimeNode = unitExporter.CreateNode(new Event_OnTickNode());
            
            if (GenericUnitExport.TryGetAutoGeneratedSchema("math/isnan", out var isNaNSchema)) 
            {
                var isNaNNode = unitExporter.CreateNode(isNaNSchema);
         
                unitExporter.MapInputPortToSocketName(_eventOnTickSocketName, deltaTimeNode,
                    "a", isNaNNode);
                
                var selectNode = unitExporter.CreateNode(new Math_SelectNode());
                unitExporter.MapInputPortToSocketName("value", isNaNNode, Math_SelectNode.IdCondition, selectNode);
                unitExporter.MapValueOutportToSocketName(unit.value, Math_SelectNode.IdOutValue, selectNode);

                unitExporter.MapInputPortToSocketName(_eventOnTickSocketName, deltaTimeNode, 
                    Math_SelectNode.IdValueB, selectNode);
                selectNode.ValueSocketConnectionData[Math_SelectNode.IdValueA] = new GltfInteractivityUnitExporterNode.ValueSocketData()
                {
                    Id = Math_SelectNode.IdValueA,
                    Value = 0f,
                    Type = GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float"),
                };
            }
            else
            {
                unitExporter.MapValueOutportToSocketName(unit.value, _eventOnTickSocketName, deltaTimeNode);
            }
            
        }
    }
}
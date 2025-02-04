using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Material_SetColorUnitExport : IUnitExporter
    {
        public Type unitType { get => typeof(SetMember); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            SetMemberUnitExport.RegisterMemberExporter(typeof(Material), nameof(Material.color), new Material_SetColorUnitExport());
            InvokeUnitExport.RegisterInvokeExporter(typeof(Material), nameof(Material.SetColor), new Material_SetColorUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as MemberUnit;
            
            if (unit.target == null)
                return;
            
            // Regular pointer/set
            var node = unitExporter.CreateNode(new Pointer_SetNode());

            // For testing pointer/interpolate
            /*
            GltfInteractivityNode node = new GltfInteractivityNode(new GltfInt_PointerInterpolate());
            node.ValueSocketConnectionData["duration"] = new GltfInteractivityNode.ValueSocketData()
            {
                Id = "duration",
                Value = 0.7f,
            };
            node.ValueSocketConnectionData["p1"] = new GltfInteractivityNode.ValueSocketData()
            {
                Id = "p1",
                Value = new Vector2(0.0f, 0.0f),
            };
            node.ValueSocketConnectionData["p2"] = new GltfInteractivityNode.ValueSocketData()
            {
                Id = "p2",
                Value = new Vector2(1.0f, 1.0f),
            };
            */
            
            var template = "/materials/{" + GltfInteractivityNodeHelper.IdPointerMaterialIndex +
                           "}/pbrMetallicRoughness/baseColorFactor";
            
            if (unit is SetMember setMember)
            {
                unitExporter.MapInputPortToSocketName(setMember.assign, Pointer_SetNode.IdFlowIn, node);
                unitExporter.MapInputPortToSocketName(setMember.input, Pointer_SetNode.IdValue, node);
                unitExporter.MapOutFlowConnectionWhenValid(setMember.assigned, Pointer_SetNode.IdFlowOut, node);

                node.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMaterialIndex,
                    setMember.target, template, GltfTypes.Float4);
            }
            else if (unit is InvokeMember invokeMember)
            {
                unitExporter.MapInputPortToSocketName(invokeMember.enter, Pointer_SetNode.IdFlowIn, node);
                // first parameter is the color property name – so based on that we can determine what pointer to set
                // var colorPropertyName = invokeMember.inputParameters[0];
                unitExporter.MapInputPortToSocketName(invokeMember.inputParameters[1], Pointer_SetNode.IdValue, node);
                unitExporter.MapOutFlowConnectionWhenValid(invokeMember.exit, Pointer_SetNode.IdFlowOut, node);

                node.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMaterialIndex,
                    invokeMember.target, template, GltfTypes.Float4);
            }
        }
    }
}
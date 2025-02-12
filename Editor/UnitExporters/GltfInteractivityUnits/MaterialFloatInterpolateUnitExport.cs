using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity;
using UnityGLTF.Interactivity.Export;
using UnityGLTF.Interactivity.Schema;

namespace Editor.UnitExporters.GltfInteractivityUnits
{
    public class MaterialFloatInterpolateUnitExport : IUnitExporter
    {
        public Type unitType { get => typeof(MaterialFloatInterpolate); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new MaterialFloatInterpolateUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as MaterialFloatInterpolate;
            
            var materialTemplate = "/materials/{" + GltfInteractivityNodeHelper.IdPointerMaterialIndex + "}/";
            var template = materialTemplate;
            
            var valueType = GltfTypes.Float;
            
            if (unitExporter.IsInputLiteralOrDefaultValue(unit.valueName, out var floatPropertyName))
            {
                var gltfProperty = MaterialPointerHelper.GetPointer(unitExporter, (string)floatPropertyName, out var map);
                if (gltfProperty == null)
                {
                    UnitExportLogging.AddErrorLog(unit, "color property name is not supported.");
                    return;
                }

                // TODO: Flip
                template = materialTemplate + gltfProperty;
            }
            else
            {
                UnitExportLogging.AddErrorLog(unit, "color property name is not a literal or default value, which is not supported.");
                return;
            } 
            
            var node = unitExporter.CreateNode(new Pointer_InterpolateNode());

            node.FlowIn(Pointer_InterpolateNode.IdFlowIn).MapToControlInput(unit.assign);
            node.FlowOut(Pointer_InterpolateNode.IdFlowOut).MapToControlOutput(unit.assigned);
            
            node.ValueIn(Pointer_InterpolateNode.IdValue).MapToInputPort(unit.targetValue);
            node.ValueIn(Pointer_InterpolateNode.IdDuration).MapToInputPort(unit.duration);
            node.ValueIn(Pointer_InterpolateNode.IdPoint1).MapToInputPort(unit.pointA);
            node.ValueIn(Pointer_InterpolateNode.IdPoint2).MapToInputPort(unit.pointB);
            node.FlowOut(Pointer_InterpolateNode.IdFlowOutDone).MapToControlOutput(unit.done);
            
            node.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMaterialIndex, unit.target, template, valueType);
        }
    }
}
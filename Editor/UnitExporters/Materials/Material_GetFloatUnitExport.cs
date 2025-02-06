using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Material_GetFloatUnitExport : IUnitExporter
    {
        public Type unitType { get => typeof(InvokeMember); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Material), nameof(Material.GetFloat), new Material_GetFloatUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as InvokeMember;
            
            if (unit.target == null)
                return;
            
            var materialTemplate = "/materials/{" + GltfInteractivityNodeHelper.IdPointerMaterialIndex + "}/";
            string template = "";
            
            if (unitExporter.IsInputLiteralOrDefaultValue(unit.inputParameters[0], out var floatPropertyName))
            {
                var gltfProperty = MaterialPointerHelper.GetPointer(unitExporter, (string)floatPropertyName, out var map);
                if (gltfProperty == null)
                {
                    UnitExportLogging.AddErrorLog(unit, "float property name is not supported.");
                    return;
                }
                template = materialTemplate + gltfProperty;
            }
            else
            {
                UnitExportLogging.AddErrorLog(unit, "float property name is not a literal or default value, which is not supported.");
                return;
            }

            var node = unitExporter.CreateNode(new Pointer_GetNode());
            unitExporter.ByPassFlow(unit.enter, unit.exit);
            node.FirstValueOut().MapToPort(unit.result).ExpectedType(ExpectedType.Float);
 
            node.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMaterialIndex,
                unit.target, template, GltfTypes.Float);
        }
    }
}
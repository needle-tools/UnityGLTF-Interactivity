using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;
using UnityGLTF.Plugins;

namespace UnityGLTF.Interactivity.Export
{
    public static class MaterialPointerHelper 
    {
        public static string GetPointer(UnitExporter unitExporter, string unityMaterialPropertyName, out MaterialPointerPropertyMap map)
        {
            // TODO: check if its already a PbrGraph property
            
            var plugins = unitExporter.exportContext.exporter.Plugins;
            
            var animationPointerExportContext =
                plugins.FirstOrDefault(x => x is AnimationPointerExportContext) as AnimationPointerExportContext;
            
            if (animationPointerExportContext.materialPropertiesRemapper.GetMapByUnityProperty(unityMaterialPropertyName,
                    out map))
            {
                return map.GltfPropertyName;
            }
            		
            return null;
        }

        public static void ConvertUvOffsetToGltf(UnitExporter unitExporter, ValueInput targetMaterial, string pointerToTextureTransformScale, out GltfInteractivityUnitExporterNode.ValueInputSocketData uvOffset, out GltfInteractivityUnitExporterNode.ValueOutputSocketData convertedUvOffset)
        {
            var getScale = unitExporter.CreateNode(new Pointer_GetNode());
            getScale.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMaterialIndex, targetMaterial, pointerToTextureTransformScale, GltfTypes.Float2);
                   
            var multiplyScale = unitExporter.CreateNode(new Math_MulNode());
            multiplyScale.ValueIn("a").SetValue(new Vector2(0f, 1f));
            multiplyScale.ValueIn("b").ConnectToSource(getScale.FirstValueOut()).SetType(TypeRestriction.LimitToFloat2);
                    
            var sub1 = unitExporter.CreateNode(new Math_SubNode());
            sub1.ValueIn(Math_SubNode.IdValueA).SetValue(new Vector2(0f, 1f));
            uvOffset = sub1.ValueIn(Math_SubNode.IdValueB).SetType(TypeRestriction.LimitToFloat2);

            var sub2 = unitExporter.CreateNode(new Math_SubNode());
            sub2.ValueIn(Math_SubNode.IdValueA).ConnectToSource(sub1.FirstValueOut());
            sub2.ValueIn(Math_SubNode.IdValueB).ConnectToSource(multiplyScale.FirstValueOut()).SetType(TypeRestriction.LimitToFloat2);
                    
            convertedUvOffset = sub2.FirstValueOut();
        }

      
    }
}
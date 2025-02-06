using System;
using System.Linq;
using UnityEngine;
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

      
    }
}
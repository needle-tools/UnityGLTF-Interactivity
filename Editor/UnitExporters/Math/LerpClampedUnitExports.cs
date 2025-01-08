using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace UnityGLTF.Interactivity.Export
{

    public static class LerpClampedHelper
    {
        public static void AddClampedLerp(UnitExporter unitExporter, int gltfType)
        {
            var unit = unitExporter.unit;
            var typeRestr = TypeRestriction.LimitToType(gltfType);
            var expType = ExpectedType.GtlfType(gltfType);
            
            GenericUnitExport.TryGetAutoGeneratedSchema("math/saturate", out var saturateSchema);
            var saturateNode = unitExporter.CreateNode(saturateSchema);
            saturateNode.ValueIn("a").MapToInputPort(unit.valueInputs[2]).SetType(typeRestr);
            saturateNode.FirstValueOut().ExpectedType(expType);

            GenericUnitExport.TryGetAutoGeneratedSchema("math/mix", out var mixSchema);
            var mixNode = unitExporter.CreateNode(mixSchema);
            mixNode.ValueIn("a").MapToInputPort(unit.valueInputs[0]).SetType(typeRestr);
            mixNode.ValueIn("b").MapToInputPort(unit.valueInputs[1]).SetType(typeRestr);
            mixNode.ValueIn("c").ConnectToSource(saturateNode.FirstValueOut()).SetType(typeRestr);
            mixNode.FirstValueOut().MapToPort(unit.valueOutputs[0]).ExpectedType(expType);
        }
    }
        
    public class LerpClampedInvokeUnitExports : IUnitExporter
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Lerp), new LerpClampedInvokeUnitExports(GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float")));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector2), nameof(Vector2.Lerp), new LerpClampedInvokeUnitExports(GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float2")));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector3), nameof(Vector3.Lerp), new LerpClampedInvokeUnitExports(GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float3")));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector4), nameof(Vector4.Lerp), new LerpClampedInvokeUnitExports(GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float4")));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Quaternion), nameof(Quaternion.Lerp), new LerpClampedInvokeUnitExports(GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float4")));
            
            // TODO: correct Slerp, currently we use Mix  
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector3), nameof(Vector3.Slerp), new LerpClampedInvokeUnitExports(GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float3")));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Quaternion), nameof(Quaternion.Slerp), new LerpClampedInvokeUnitExports(GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float4")));
        }

        public Type unitType { get => typeof(InvokeMember); }
        private int gltfType;
        
        public LerpClampedInvokeUnitExports(int gltfType)
        {
            this.gltfType = gltfType;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as InvokeMember;
            LerpClampedHelper.AddClampedLerp(unitExporter, gltfType);

            unitExporter.ByPassFlow(unit.enter, unit.exit);
         }
    }

    public class LerpClampedUnitExports : IUnitExporter
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new LerpClampedUnitExports(typeof(ScalarLerp), GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float")));
            UnitExporterRegistry.RegisterExporter(new LerpClampedUnitExports(typeof(Vector2Lerp), GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float2")));
            UnitExporterRegistry.RegisterExporter(new LerpClampedUnitExports(typeof(Vector3Lerp), GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float3")));
            UnitExporterRegistry.RegisterExporter(new LerpClampedUnitExports(typeof(Vector4Lerp), GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float4")));
        }

        public Type unitType { get; private set; }
        private int gltfType;

        public LerpClampedUnitExports(Type unitType, int gltfType)
        {
            this.unitType = unitType;
            this.gltfType = gltfType;
        }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            LerpClampedHelper.AddClampedLerp(unitExporter, gltfType);
        }
    }
}
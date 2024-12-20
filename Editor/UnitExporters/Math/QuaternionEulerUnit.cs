using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class QuaternionEulerUnit : IUnitExporter
    {
        public Type unitType
        {
            get => typeof(InvokeMember);
        }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Quaternion), nameof(Quaternion.Euler), new QuaternionEulerUnit());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            /*
              Based on:
                float3 s, c;
                sincos(0.5f * xyz, out s, out c);
                return quaternion( float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * float4(c.xyz, s.x) * float4(1.0f, 1.0f, -1.0f, -1.0f)
             */
            var invokeMember = unitExporter.unit as InvokeMember;

            GltfInteractivityUnitExporterNode combineInput = null;
            var cosSchema = GenericUnitExport.GetAutoGeneratedSchema("math/cos");
            var sinSchema = GenericUnitExport.GetAutoGeneratedSchema("math/sin");
            var mulSchema = GenericUnitExport.GetAutoGeneratedSchema("math/mul");
            var halfNode = unitExporter.CreateNode(mulSchema);
            halfNode.FirstValueOut().ExpectedType(ExpectedType.Float3);
            halfNode.ValueIn("b").SetValue(new Vector3(0.5f, 0.5f, 0.5f)).SetType(TypeRestriction.LimitToFloat3);
            
            if (invokeMember.valueInputs.Count == 1)
            {
                // Vector3 Input
                halfNode.ValueIn("a").MapToInputPort(invokeMember.valueInputs[0]).SetType(TypeRestriction.LimitToFloat3);
            }
            else
            {
                // XYZ Input
                var combineXYZ = unitExporter.CreateNode(GenericUnitExport.GetAutoGeneratedSchema("math/combine3"));
                combineXYZ.ValueIn("a").MapToInputPort(invokeMember.valueInputs[0]).SetType(TypeRestriction.LimitToFloat); 
                combineXYZ.ValueIn("b").MapToInputPort(invokeMember.valueInputs[1]).SetType(TypeRestriction.LimitToFloat); 
                combineXYZ.ValueIn("c").MapToInputPort(invokeMember.valueInputs[2]).SetType(TypeRestriction.LimitToFloat); 
                halfNode.ValueIn("a").ConnectToSource(combineXYZ.FirstValueOut()).SetType(TypeRestriction.LimitToFloat3);
            }
            
            var cosNode = unitExporter.CreateNode(cosSchema);
            cosNode.ValueIn("a").ConnectToSource(halfNode.FirstValueOut()).SetType(TypeRestriction.LimitToFloat3);
            cosNode.FirstValueOut().ExpectedType(ExpectedType.Float3);
            
            var sinNode = unitExporter.CreateNode(sinSchema);
            sinNode.ValueIn("a").ConnectToSource(halfNode.FirstValueOut()).SetType(TypeRestriction.LimitToFloat3);
            sinNode.FirstValueOut().ExpectedType(ExpectedType.Float3);

            
            var extractXYZSinNode = unitExporter.CreateNode(new Math_Extract3Node());
            extractXYZSinNode.ValueIn("a").ConnectToSource(sinNode.FirstValueOut());

            var extractXYZCosNode = unitExporter.CreateNode(new Math_Extract3Node());
            extractXYZCosNode.ValueIn("a").ConnectToSource(cosNode.FirstValueOut());

            #region Helpers
            GltfInteractivityUnitExporterNode.ValueInputSocketData SinX(GltfInteractivityUnitExporterNode.ValueInputSocketData inSocket)
            {
                inSocket.ConnectToSource(extractXYZSinNode.ValueOut(Math_Extract3Node.IdValueOutX));
                return inSocket;
            }
            GltfInteractivityUnitExporterNode.ValueInputSocketData SinY(GltfInteractivityUnitExporterNode.ValueInputSocketData inSocket)
            {
                inSocket.ConnectToSource(extractXYZSinNode.ValueOut(Math_Extract3Node.IdValueOutY));
                return inSocket;
            }
            GltfInteractivityUnitExporterNode.ValueInputSocketData SinZ(GltfInteractivityUnitExporterNode.ValueInputSocketData inSocket)
            {
                inSocket.ConnectToSource(extractXYZSinNode.ValueOut(Math_Extract3Node.IdValueOutZ));
                return inSocket;
            } 
            GltfInteractivityUnitExporterNode.ValueInputSocketData CosX(GltfInteractivityUnitExporterNode.ValueInputSocketData inSocket)
            {
                inSocket.ConnectToSource(extractXYZCosNode.ValueOut(Math_Extract3Node.IdValueOutX));
                return inSocket;
            }
            GltfInteractivityUnitExporterNode.ValueInputSocketData CosY(GltfInteractivityUnitExporterNode.ValueInputSocketData inSocket)
            {
                inSocket.ConnectToSource(extractXYZCosNode.ValueOut(Math_Extract3Node.IdValueOutY));
                return inSocket;
            }
            GltfInteractivityUnitExporterNode.ValueInputSocketData CosZ(GltfInteractivityUnitExporterNode.ValueInputSocketData inSocket)
            {
                inSocket.ConnectToSource(extractXYZCosNode.ValueOut(Math_Extract3Node.IdValueOutZ));
                return inSocket;
            } 
            
            #endregion
            
            var combineSchema = GenericUnitExport.GetAutoGeneratedSchema("math/combine4");
            
            var four1 = unitExporter.CreateNode(combineSchema);
            SinX(four1.ValueIn("a"));
            SinY(four1.ValueIn("b"));
            SinZ(four1.ValueIn("c"));
            CosX(four1.ValueIn("d"));
            
            var four2 = unitExporter.CreateNode(combineSchema);
            CosY(four2.ValueIn("a"));
            CosX(four2.ValueIn("b"));
            CosX(four2.ValueIn("c"));
            CosY(four2.ValueIn("d"));

            var four3 = unitExporter.CreateNode(combineSchema);
            CosZ(four3.ValueIn("a"));
            CosZ(four3.ValueIn("b"));
            CosY(four3.ValueIn("c"));
            CosZ(four3.ValueIn("d"));

            var four4 = unitExporter.CreateNode(combineSchema);
            SinY(four4.ValueIn("a"));
            SinX(four4.ValueIn("b"));
            SinX(four4.ValueIn("c"));
            SinY(four4.ValueIn("d"));

            var four5 = unitExporter.CreateNode(combineSchema);
            SinZ(four5.ValueIn("a"));
            SinZ(four5.ValueIn("b"));
            SinY(four5.ValueIn("c"));
            SinZ(four5.ValueIn("d"));

            var four6 = unitExporter.CreateNode(combineSchema);
            CosX(four6.ValueIn("a"));
            CosY(four6.ValueIn("b"));
            CosZ(four6.ValueIn("c"));
            SinX(four6.ValueIn("d"));

            var four7 = unitExporter.CreateNode(combineSchema);
            four7.ValueIn("a").SetValue(1f);
            four7.ValueIn("b").SetValue(1f);
            four7.ValueIn("c").SetValue(-1f);
            four7.ValueIn("d").SetValue(-1f);

            // Sum Part 1
            var mul1 = unitExporter.CreateNode(mulSchema);
            mul1.ValueIn("a").ConnectToSource(four1.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul1.ValueIn("b").ConnectToSource(four2.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul1.FirstValueOut().ExpectedType(ExpectedType.Float4);
            
            var mul2 = unitExporter.CreateNode(mulSchema);
            mul2.ValueIn("a").ConnectToSource(mul1.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul2.ValueIn("b").ConnectToSource(four3.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul2.FirstValueOut().ExpectedType(ExpectedType.Float4);

            // Sum Part 2
            var mul3 = unitExporter.CreateNode(mulSchema);
            mul3.ValueIn("a").ConnectToSource(four4.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul3.ValueIn("b").ConnectToSource(four5.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul3.FirstValueOut().ExpectedType(ExpectedType.Float4);
            
            var mul4 = unitExporter.CreateNode(mulSchema);
            mul4.ValueIn("a").ConnectToSource(mul3.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul4.ValueIn("b").ConnectToSource(four6.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul4.FirstValueOut().ExpectedType(ExpectedType.Float4);
            
            var mul5 = unitExporter.CreateNode(mulSchema);
            mul5.ValueIn("a").ConnectToSource(mul4.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul5.ValueIn("b").ConnectToSource(four7.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            mul5.FirstValueOut().ExpectedType(ExpectedType.Float4);
            
            // Sum
            
            var sum = unitExporter.CreateNode(new Math_AddNode());
            sum.ValueIn("a").ConnectToSource(mul2.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            sum.ValueIn("b").ConnectToSource(mul5.FirstValueOut()).SetType(TypeRestriction.LimitToFloat4);
            sum.FirstValueOut().ExpectedType(ExpectedType.Float4);
            
            sum.FirstValueOut().MapToPort(invokeMember.result);
            
            
        }
    }
}
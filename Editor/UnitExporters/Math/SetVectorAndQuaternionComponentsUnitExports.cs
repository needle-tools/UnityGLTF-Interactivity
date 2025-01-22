using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class SetVectorAndQuaternionComponentsUnitExports : IUnitExporter
    {
        public Type unitType { get => typeof(SetMember); }
        
        private static readonly string[] VectorMemberIndex = new string[] { "x", "y", "z", "w" };
        private static readonly string[] InputNames = new string[] { "a", "b", "c", "d" };
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector2), nameof(Vector2.x), new SetVectorAndQuaternionComponentsUnitExports(2, 0) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector2), nameof(Vector2.y), new SetVectorAndQuaternionComponentsUnitExports(2, 1) );

            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector3), nameof(Vector3.x), new SetVectorAndQuaternionComponentsUnitExports(3, 0) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector3), nameof(Vector3.y), new SetVectorAndQuaternionComponentsUnitExports(3, 1) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector3), nameof(Vector3.z), new SetVectorAndQuaternionComponentsUnitExports(3, 2) );
            
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector4), nameof(Vector4.x), new SetVectorAndQuaternionComponentsUnitExports(4, 0) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector4), nameof(Vector4.y), new SetVectorAndQuaternionComponentsUnitExports(4, 1) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector4), nameof(Vector4.z), new SetVectorAndQuaternionComponentsUnitExports(4, 2) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Vector4), nameof(Vector4.w), new SetVectorAndQuaternionComponentsUnitExports(4, 3) );
            
            SetMemberUnitExport.RegisterMemberConvert(typeof(Quaternion), nameof(Quaternion.x), new SetVectorAndQuaternionComponentsUnitExports(4, 0) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Quaternion), nameof(Quaternion.y), new SetVectorAndQuaternionComponentsUnitExports(4, 1) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Quaternion), nameof(Quaternion.z), new SetVectorAndQuaternionComponentsUnitExports(4, 2) );
            SetMemberUnitExport.RegisterMemberConvert(typeof(Quaternion), nameof(Quaternion.w), new SetVectorAndQuaternionComponentsUnitExports(4, 3) );

        }

        private int componentCount;
        private int componentIndex;
        
        public SetVectorAndQuaternionComponentsUnitExports(int componentCount, int componentIndex)
        {
            this.componentIndex = componentIndex;
            this.componentCount = componentCount;
        }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var setMember = unitExporter.unit as SetMember;
            
            GltfInteractivityNodeSchema extractSchema = null;
            GltfInteractivityNodeSchema combineSchema = null;
            ExpectedType expectedType = ExpectedType.Float;
            switch (componentCount)
            {
                case 2:
                    extractSchema = new Math_Extract2Node();
                    expectedType = ExpectedType.Float2;
                    combineSchema = new Math_Combine2Node();
                    break;
                case 3:
                    extractSchema = new Math_Extract3Node();
                    expectedType = ExpectedType.Float3;
                    combineSchema = new Math_Combine3Node();
                    break;
                case 4:
                    extractSchema = new Math_Extract4Node();
                    expectedType = ExpectedType.Float4;
                    combineSchema = new Math_Combine4Node();
                    break;
                default:
                    Debug.LogError("Unsupported component count: " + componentCount);
                    return;
            }
            
            var extractNode = unitExporter.CreateNode(extractSchema);
            extractNode.ValueIn("a").MapToInputPort(setMember.target);
            
            var combineNode = unitExporter.CreateNode(combineSchema);
            for (int i = 0; i < componentCount; i++)
            {
                if (i == componentIndex)
                    combineNode.ValueIn(InputNames[i]).MapToInputPort(setMember.input);
                else
                    combineNode.ValueIn(InputNames[i]).ConnectToSource(extractNode.ValueOut(i.ToString()));
            }

            combineNode.FirstValueOut().ExpectedType(expectedType);
            
            unitExporter.ByPassValue(setMember.input, setMember.output);
            combineNode.FirstValueOut().MapToPort(setMember.targetOutput);
            unitExporter.ByPassFlow(setMember.assign, setMember.assigned);
        }
    }
}
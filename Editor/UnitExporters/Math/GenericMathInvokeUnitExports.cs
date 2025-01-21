using UnityEditor;
using UnityEngine;

namespace UnityGLTF.Interactivity.Export
{
    [InputSocketDefine("a", 0)]
    [InputSocketDefine("b", 1)]
    [InputSocketDefine("c", 2)]
    [InputSocketDefine("d", 3)]
    [OutputSocketDefine("value", 0)]
    public class GenericInvokeMathInvokeUnit4In1Out : GenericInvokeUnitExport
    {

        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules_FloatIn_Float4Out = new GenericSocketRules();
            socketRules_FloatIn_Float4Out.AddExpectedType("value", ExpectedType.Float4);
            socketRules_FloatIn_Float4Out.AddTypeRestriction("a", TypeRestriction.LimitToFloat);
            socketRules_FloatIn_Float4Out.AddTypeRestriction("b", TypeRestriction.LimitToFloat);
            socketRules_FloatIn_Float4Out.AddTypeRestriction("c", TypeRestriction.LimitToFloat);
            socketRules_FloatIn_Float4Out.AddTypeRestriction("d", TypeRestriction.LimitToFloat);
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector4), ".ctor", new GenericInvokeMathInvokeUnit4In1Out("math/combine4", socketRules_FloatIn_Float4Out));
        }
        
        public GenericInvokeMathInvokeUnit4In1Out(string gltfNodeOp, GenericSocketRules socketRules = null) : base(gltfNodeOp, socketRules)
        {
        }
    }
    
    [InputSocketDefine("a", 0)]
    [InputSocketDefine("b", 1)]
    [InputSocketDefine("c", 2)]
    [OutputSocketDefine("value", 0)]
    public class GenericInvokeMathInvokeUnit3In1Out : GenericInvokeUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules_mix_float = new GenericSocketRules();
            socketRules_mix_float.AddExpectedType("value", ExpectedType.Float);
            socketRules_mix_float.AddTypeRestriction("a", TypeRestriction.LimitToFloat);
            socketRules_mix_float.AddTypeRestriction("b", TypeRestriction.LimitToFloat);
            socketRules_mix_float.AddTypeRestriction("c", TypeRestriction.LimitToFloat);

            var socketRules_mix_float2 = new GenericSocketRules();
            socketRules_mix_float2.AddExpectedType("value", ExpectedType.Float2);
            socketRules_mix_float2.AddTypeRestriction("a", TypeRestriction.LimitToFloat2);
            socketRules_mix_float2.AddTypeRestriction("b", TypeRestriction.LimitToFloat2);
            socketRules_mix_float2.AddTypeRestriction("c", TypeRestriction.LimitToFloat2);

            var socketRules_mix_float3 = new GenericSocketRules();
            socketRules_mix_float3.AddExpectedType("value", ExpectedType.Float3);
            socketRules_mix_float3.AddTypeRestriction("a", TypeRestriction.LimitToFloat3);
            socketRules_mix_float3.AddTypeRestriction("b", TypeRestriction.LimitToFloat3);
            socketRules_mix_float3.AddTypeRestriction("c", TypeRestriction.LimitToFloat3);

            var socketRules_mix_float4 = new GenericSocketRules();
            socketRules_mix_float4.AddExpectedType("value", ExpectedType.Float4);
            socketRules_mix_float4.AddTypeRestriction("a", TypeRestriction.LimitToFloat4);
            socketRules_mix_float4.AddTypeRestriction("b", TypeRestriction.LimitToFloat4);
            socketRules_mix_float4.AddTypeRestriction("c", TypeRestriction.LimitToFloat4);


            
            var socketRules_Clamp = new GenericSocketRules();
            socketRules_Clamp.AddExpectedType("value", ExpectedType.FromInputSocket("a"));
            socketRules_Clamp.AddTypeRestriction("a", TypeRestriction.SameAsInputPort("b"));
            socketRules_Clamp.AddTypeRestriction("b", TypeRestriction.SameAsInputPort("a"));
            socketRules_Clamp.AddTypeRestriction("c", TypeRestriction.SameAsInputPort("a"));
            
            var socketRules_FloatIn_Float3Out = new GenericSocketRules();
            socketRules_FloatIn_Float3Out.AddExpectedType("value", ExpectedType.Float3);
            socketRules_FloatIn_Float3Out.AddTypeRestriction("a", TypeRestriction.LimitToFloat);
            socketRules_FloatIn_Float3Out.AddTypeRestriction("b", TypeRestriction.LimitToFloat);
            socketRules_FloatIn_Float3Out.AddTypeRestriction("c", TypeRestriction.LimitToFloat);
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector3), ".ctor", new GenericInvokeMathInvokeUnit3In1Out("math/combine3", socketRules_FloatIn_Float3Out));
           
            // TODO: Slerp is not the same as Lerp, but for now we use the same node
            InvokeUnitExport.RegisterInvokeExporter(typeof(Quaternion), nameof(Quaternion.SlerpUnclamped), new GenericInvokeMathInvokeUnit3In1Out("math/mix", socketRules_mix_float4));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector3), nameof(Vector3.SlerpUnclamped), new GenericInvokeMathInvokeUnit3In1Out("math/mix", socketRules_mix_float3));
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.LerpUnclamped), new GenericInvokeMathInvokeUnit3In1Out("math/mix", socketRules_mix_float));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Quaternion), nameof(Quaternion.LerpUnclamped), new GenericInvokeMathInvokeUnit3In1Out("math/mix", socketRules_mix_float4));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector2), nameof(Vector2.LerpUnclamped), new GenericInvokeMathInvokeUnit3In1Out("math/mix", socketRules_mix_float2));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector3), nameof(Vector3.LerpUnclamped), new GenericInvokeMathInvokeUnit3In1Out("math/mix", socketRules_mix_float3));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector4), nameof(Vector4.LerpUnclamped), new GenericInvokeMathInvokeUnit3In1Out("math/mix", socketRules_mix_float4));
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Clamp), new GenericInvokeMathInvokeUnit3In1Out("math/clamp", socketRules_Clamp));
        }
        
        public GenericInvokeMathInvokeUnit3In1Out(string gltfNodeOp, GenericSocketRules socketRules = null) : base(gltfNodeOp, socketRules)
        {
        }
    }
    
    [InputSocketDefine("a", 0)]
    [InputSocketDefine("b", 1)]
    [OutputSocketDefine("value", 0)]
    public class GenericInvokeMathInvokeUnit2In1Out : GenericInvokeUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules_SameIn_OutBool = new GenericSocketRules();
            socketRules_SameIn_OutBool.AddExpectedType("value", ExpectedType.Bool);
            socketRules_SameIn_OutBool.AddTypeRestriction("a", TypeRestriction.SameAsInputPort("b"));
            socketRules_SameIn_OutBool.AddTypeRestriction("b", TypeRestriction.SameAsInputPort("a"));
            
            var socketRules_SameOutTypeAsInputA = new GenericSocketRules();
            socketRules_SameOutTypeAsInputA.AddExpectedType("value", ExpectedType.FromInputSocket("a"));
            
            var socketRules_FloatIn_Float2Out = new GenericSocketRules();
            socketRules_FloatIn_Float2Out.AddExpectedType("value", ExpectedType.Float2);
            socketRules_FloatIn_Float2Out.AddTypeRestriction("a", TypeRestriction.LimitToFloat);
            socketRules_FloatIn_Float2Out.AddTypeRestriction("b", TypeRestriction.LimitToFloat);
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Atan2), new GenericInvokeMathInvokeUnit2In1Out("math/atan2", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector2), ".ctor", new GenericInvokeMathInvokeUnit2In1Out("math/combine2", socketRules_FloatIn_Float2Out));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Min), new GenericInvokeMathInvokeUnit2In1Out("math/min", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Max), new GenericInvokeMathInvokeUnit2In1Out("math/max", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Equals), new GenericInvokeMathInvokeUnit2In1Out("math/eq", socketRules_SameIn_OutBool));
        }
        
        public GenericInvokeMathInvokeUnit2In1Out(string gltfNodeOp, GenericSocketRules socketRules = null) : base(gltfNodeOp, socketRules)
        {
        }
    }
    
    [InputSocketDefine("a", 0)]
    [OutputSocketDefine("value", 0)]
    public class GenericInvokeMathInvokeUnit1In1Out : GenericInvokeUnitExport
    {
        /*
             * math/log2
             * math/cbrt (CubeRoot)
             * math/neg     
         */

        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules_SameOutTypeAsInputA = new GenericSocketRules();
            socketRules_SameOutTypeAsInputA.AddExpectedType("value", ExpectedType.FromInputSocket("a"));
            
            var socketRules_OutFloat = new GenericSocketRules();
            socketRules_OutFloat.AddExpectedType("value", ExpectedType.Float);
            
            var socketRules_OutBool = new GenericSocketRules();
            socketRules_OutBool.AddExpectedType("value", ExpectedType.Bool);
            
            var socketRules_InFloat_OutFloat = new GenericSocketRules();
            socketRules_InFloat_OutFloat.AddTypeRestriction("a", TypeRestriction.LimitToFloat);
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Sin), new GenericInvokeMathInvokeUnit1In1Out("math/sin", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Cos), new GenericInvokeMathInvokeUnit1In1Out("math/cos", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Tan), new GenericInvokeMathInvokeUnit1In1Out("math/tan", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Asin), new GenericInvokeMathInvokeUnit1In1Out("math/asin", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Acos), new GenericInvokeMathInvokeUnit1In1Out("math/acos", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Atan), new GenericInvokeMathInvokeUnit1In1Out("math/atan", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Exp), new GenericInvokeMathInvokeUnit1In1Out("math/exp", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Log), new GenericInvokeMathInvokeUnit1In1Out("math/log", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Log10), new GenericInvokeMathInvokeUnit1In1Out("math/log10", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Sqrt), new GenericInvokeMathInvokeUnit1In1Out("math/sqrt", socketRules_SameOutTypeAsInputA));
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Pow), new GenericInvokeMathInvokeUnit2In1Out("math/pow", socketRules_SameOutTypeAsInputA));

            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector3), nameof(Vector3.Magnitude), new GenericInvokeMathInvokeUnit1In1Out("math/length", socketRules_OutFloat));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Vector4), nameof(Vector4.Magnitude), new GenericInvokeMathInvokeUnit1In1Out("math/length", socketRules_OutFloat));
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Sign), new GenericInvokeMathInvokeUnit1In1Out("math/sign", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Floor), new GenericInvokeMathInvokeUnit1In1Out("math/floor", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Ceil), new GenericInvokeMathInvokeUnit1In1Out("math/ceil", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Round), new GenericInvokeMathInvokeUnit1In1Out("math/round", socketRules_SameOutTypeAsInputA));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.RoundToInt), new GenericInvokeMathInvokeUnit1In1Out("type/floatToInt", socketRules_InFloat_OutFloat));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Clamp01), new GenericInvokeMathInvokeUnit1In1Out("math/saturate", socketRules_SameOutTypeAsInputA));
            
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.Abs), new GenericInvokeMathInvokeUnit1In1Out("math/abs", socketRules_SameOutTypeAsInputA));

            InvokeUnitExport.RegisterInvokeExporter(typeof(float), nameof(float.IsNaN), new GenericInvokeMathInvokeUnit1In1Out("math/isnan", socketRules_OutBool));
            InvokeUnitExport.RegisterInvokeExporter(typeof(float), nameof(float.IsInfinity), new GenericInvokeMathInvokeUnit1In1Out("math/isinf", socketRules_OutBool));
            
            // These don't exist as Units but we still want generic export nodes for them
            new GenericInvokeMathInvokeUnit1In1Out("math/trunc", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/fract", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/neg", socketRules_SameOutTypeAsInputA);
            
            new GenericInvokeMathInvokeUnit1In1Out("math/sinh", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/cosh", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/tanh", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/asinh", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/acosh", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/atanh", socketRules_SameOutTypeAsInputA);
            
            new GenericInvokeMathInvokeUnit1In1Out("math/log2", socketRules_SameOutTypeAsInputA);
            new GenericInvokeMathInvokeUnit1In1Out("math/cbrt", socketRules_SameOutTypeAsInputA);
        }
        
        public GenericInvokeMathInvokeUnit1In1Out(string gltfNodeOp, GenericSocketRules socketRules = null) : base(gltfNodeOp, socketRules)
        {
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace UnityGLTF.Interactivity.Export
{
    [OutputSocketDefine("value", 0)]
    public class GenericGetMemberMathUnit1Out : GenericGetMemberUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules = new GenericSocketRules();
            socketRules.AddExpectedType("value", ExpectedType.Float);
            
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(Mathf.PI), new GenericGetMemberMathUnit1Out("math/pi", socketRules));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(Mathf.Epsilon), new GenericGetMemberMathUnit1Out("math/e", socketRules));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(Mathf.Infinity), new GenericGetMemberMathUnit1Out("math/inf", socketRules));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(float.NaN), new GenericGetMemberMathUnit1Out("math/nan", socketRules));
       }
        
        public GenericGetMemberMathUnit1Out(string gltfNodeType, GenericSocketRules socketRules = null) : base(gltfNodeType, socketRules)
        {
        }
    }
    
    [OutputSocketDefine("a", 0)]
    [OutputSocketDefine("value", 0)]
    public class GenericGetMemberMathUnit_1In_1Out : GenericGetMemberUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules = new GenericSocketRules();
            socketRules.AddExpectedType("value", ExpectedType.Float);
            
            GetMemberUnitExport.RegisterMemberExporter(typeof(Vector2), nameof(Vector2.magnitude), new GenericGetMemberMathUnit_1In_1Out("math/length", socketRules));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Vector3), nameof(Vector3.magnitude), new GenericGetMemberMathUnit_1In_1Out("math/length", socketRules));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Vector4), nameof(Vector3.magnitude), new GenericGetMemberMathUnit_1In_1Out("math/length", socketRules));
        }
        
        public GenericGetMemberMathUnit_1In_1Out(string gltfNodeType, GenericSocketRules socketRules = null) : base(gltfNodeType, socketRules)
        {
        }
    }

}
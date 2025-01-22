using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
 
    public class GenericGetMemberMathExporters : GenericGetMemberUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(Mathf.PI), new GenericGetMemberMathExporters(new Math_PiNode()));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(Mathf.Epsilon), new GenericGetMemberMathExporters(new Math_ENode()));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(Mathf.Infinity), new GenericGetMemberMathExporters(new Math_InfNode()));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Mathf), nameof(float.NaN), new GenericGetMemberMathExporters(new Math_NaNNode()));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Vector2), nameof(Vector2.magnitude), new GenericGetMemberMathExporters(new Math_LenghNode()));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Vector3), nameof(Vector3.magnitude), new GenericGetMemberMathExporters(new Math_LenghNode()));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Vector4), nameof(Vector3.magnitude), new GenericGetMemberMathExporters(new Math_LenghNode()));
       }
        
        public GenericGetMemberMathExporters(GltfInteractivityNodeSchema schema) : base(schema)
        {
        }
    }
}
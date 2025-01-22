namespace UnityGLTF.Interactivity.Schema
{
    public class Math_EqNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/eq";

        [OutputSocketDescription(GltfTypes.Bool)]
        public const string IdOut = "value";
        
        [InputSocketDescriptionWithTypeDependencyFromOtherPort(IdValueB, GltfTypes.Int, GltfTypes.Bool,
            GltfTypes.Float, GltfTypes.Float2,
            GltfTypes.Float3, GltfTypes.Float4, 
            GltfTypes.Float2x2, GltfTypes.Float3x3, GltfTypes.Float4x4)]
        public const string IdValueA = "a";
        [InputSocketDescriptionWithTypeDependencyFromOtherPort(IdValueA, GltfTypes.Int, GltfTypes.Bool,
            GltfTypes.Float, GltfTypes.Float2,
            GltfTypes.Float3, GltfTypes.Float4, 
            GltfTypes.Float2x2, GltfTypes.Float3x3, GltfTypes.Float4x4)]
        public const string IdValueB = "b";

    
        
    }
}
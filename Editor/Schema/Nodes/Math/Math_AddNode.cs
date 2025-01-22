namespace UnityGLTF.Interactivity.Schema
{
    public class Math_AddNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/add";

        [OutputSocketDescriptionWithTypeDependencyFromInput(IdValueA)]
        public const string IdOut = "value";
        
        [InputSocketDescriptionWithTypeDependencyFromOtherPort(IdValueB, GltfTypes.Int, GltfTypes.Float, GltfTypes.Float2, GltfTypes.Float3, GltfTypes.Float4)]
        public const string IdValueA = "a";
        [InputSocketDescriptionWithTypeDependencyFromOtherPort(IdValueA, GltfTypes.Int, GltfTypes.Float, GltfTypes.Float2, GltfTypes.Float3, GltfTypes.Float4)]
        public const string IdValueB = "b";
    }
}

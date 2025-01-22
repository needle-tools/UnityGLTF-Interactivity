namespace UnityGLTF.Interactivity.Schema
{
    public class Math_CeilNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/ceil";
        
        [InputSocketDescription(GltfTypes.Float, GltfTypes.Float2, GltfTypes.Float3)]
        public const string IdInputA = "a";
        
        [OutputSocketDescriptionWithTypeDependencyFromInput(IdInputA)]
        public const string IdValueResult = "value";
    }
}
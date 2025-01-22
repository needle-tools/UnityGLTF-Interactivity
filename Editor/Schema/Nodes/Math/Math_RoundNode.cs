namespace UnityGLTF.Interactivity.Schema
{
    public class Math_RoundNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/round";

        [InputSocketDescription(GltfTypes.Float, GltfTypes.Float2, GltfTypes.Float3, GltfTypes.Float4)]
        public const string IdInputA = "a";
        [OutputSocketDescriptionWithTypeDependencyFromInput(IdInputA)]
        public const string IdValueResult = "value";
    }
}
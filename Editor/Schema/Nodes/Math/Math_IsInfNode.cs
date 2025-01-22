namespace UnityGLTF.Interactivity.Schema
{
    public class Math_IsInfNode: GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/isinf";

        [InputSocketDescription(GltfTypes.Float)]
        public const string IdInputA = "a";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueResult = "value";
    }
}
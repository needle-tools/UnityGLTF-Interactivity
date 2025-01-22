namespace UnityGLTF.Interactivity.Schema
{
    public class Math_RandomNode: GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/random";

        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueResult = "value";
    }
}
namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Extract2Node : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/extract2";

        [InputSocketDescription(GltfTypes.Float2)]
        public const string IdValueIn = "a";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutX = "0";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutY = "1";
    }
}
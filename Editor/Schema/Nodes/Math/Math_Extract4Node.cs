namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Extract4Node : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/extract4";

        [InputSocketDescription(GltfTypes.Float4)]
        public const string IdValueIn = "a";
        
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutX = "0";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutY = "1";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutZ = "2";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutW = "3";
    }
}
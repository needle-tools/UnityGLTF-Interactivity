namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Extract3Node : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/extract3";

        [InputSocketDescription(GltfTypes.Float3)]
        public const string IdValueIn = "a";
        
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutX = "0";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutY = "1";
        [OutputSocketDescription(GltfTypes.Float)]
        public const string IdValueOutZ = "2";
    }
}
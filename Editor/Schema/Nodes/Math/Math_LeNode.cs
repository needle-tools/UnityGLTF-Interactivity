namespace UnityGLTF.Interactivity.Schema
{
    public class Math_LeNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/le";

        [OutputSocketDescription(GltfTypes.Bool)]
        public const string IdOut = "value";
        
        [InputSocketDescription(GltfTypes.Float)]
        public const string IdValueA = "a";
        [InputSocketDescription(GltfTypes.Float)]
        public const string IdValueB = "b";
    }
}
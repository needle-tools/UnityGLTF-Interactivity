namespace UnityGLTF.Interactivity.Schema
{
    public class Math_GtNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/gt";

        [OutputSocketDescription(GltfTypes.Bool)]
        public const string IdOut = "value";
        
        [InputSocketDescription(GltfTypes.Float)]
        public const string IdValueA = "a";
        [InputSocketDescription(GltfTypes.Float)]
        public const string IdValueB = "b";
        
        
    }
}
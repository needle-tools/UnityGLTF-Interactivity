namespace UnityGLTF.Interactivity.Schema
{
    public class Pointer_GetNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "pointer/get";

        [OutputSocketDescription()]
        public const string IdValue = "value";
        
        [ConfigDescription(GltfTypes.String)]
        public const string IdPointer = "pointer";
        [ConfigDescription(GltfTypes.Int)]
        public const string IdPointerValueType = "type";
    }
}
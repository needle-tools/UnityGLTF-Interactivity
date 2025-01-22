namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Rotate3dNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/rotate3d";

        [InputSocketDescription(GltfTypes.Float3)]
        public const string IdInputVector = "a";

        [InputSocketDescription(GltfTypes.Float3)]
        public const string IdInputAxis = "b";

        [InputSocketDescription(GltfTypes.Float)]
        public const string IdInputAngleRadians = "b";

        [OutputSocketDescription(GltfTypes.Float3)]
        public const string IdOutputResult = "value";
    }
}
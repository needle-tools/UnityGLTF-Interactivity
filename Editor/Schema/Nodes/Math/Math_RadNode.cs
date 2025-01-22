namespace UnityGLTF.Interactivity.Schema
{
    public class GltfInt_Rad : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/rad";

        [InputSocketDescription(GltfTypes.Float, GltfTypes.Float2, GltfTypes.Float3, GltfTypes.Float4)]
        public const string IdInputA = "a";
        [OutputSocketDescriptionWithTypeDependencyFromInput(IdInputA)]
        public const string IdValueResult = "value";
    }
}
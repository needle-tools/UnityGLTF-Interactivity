namespace UnityGLTF.Interactivity.Schema
{
    public class Math_SelectNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/select";

        [InputSocketDescription(GltfTypes.Bool)]
        public const string IdCondition = "condition";
        [InputSocketDescriptionWithTypeDependencyFromOtherPort(IdValueB)]
        public const string IdValueA = "a";
        [InputSocketDescriptionWithTypeDependencyFromOtherPort(IdValueA)]
        public const string IdValueB = "b";
        
        [OutputSocketDescriptionWithTypeDependencyFromInput(IdValueA)]
        public const string IdOutValue = "value";
    }
}
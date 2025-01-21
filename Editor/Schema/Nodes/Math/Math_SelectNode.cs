namespace UnityGLTF.Interactivity.Schema
{
    public class Math_SelectNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/select";
        public static readonly string IdCondition = "condition";
        public static readonly string IdValueA = "a";
        public static readonly string IdValueB = "b";
        public static readonly string IdOutValue = "value";
        
        public Math_SelectNode()
        {
            Op = TypeName;
            
            InputValueSockets = new GltfInteractivityNodeSchema.InputValueSocketDescriptor[]
            {
                new GltfInteractivityNodeSchema.InputValueSocketDescriptor()
                {
                    Id = IdCondition,
                    SupportedTypes = new string[]{"bool"}
                },
                new GltfInteractivityNodeSchema.InputValueSocketDescriptor()
                {
                    Id = IdValueA,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes,
                    typeRestriction = TypeRestriction.SameAsInputPort(IdValueB)
                },
                new GltfInteractivityNodeSchema.InputValueSocketDescriptor()
                {
                    Id = IdValueB,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes,
                    typeRestriction = TypeRestriction.SameAsInputPort(IdValueA)
                }
            };
            
            OutputValueSockets = new GltfInteractivityNodeSchema.OutValueSocketDescriptor[]
            {
                new GltfInteractivityNodeSchema.OutValueSocketDescriptor()
                {
                    Id = IdOutValue,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes,
                    expectedType = ExpectedType.FromInputSocket(IdValueA)
                }
            };
        }
    }
}
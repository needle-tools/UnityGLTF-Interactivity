namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_SelectNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/select";
        public static readonly string IdInCondition = "condition";
        public static readonly string IdInA = "a";
        public static readonly string IdInB = "b";
        public static readonly string IdOutValue = "value";

        public Flow_SelectNode()
        {
            Type = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInCondition,
                    SupportedTypes = new string[]{"bool"}
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdInA,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes,
                    typeRestriction = TypeRestriction.SameAsInputPort(IdInB)
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdInB,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes,
                    typeRestriction = TypeRestriction.SameAsInputPort(IdInA)
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOutValue,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes,
                    expectedType = ExpectedType.FromInputSocket(IdInA)
                }
            };
            
        }
    }
}
namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_ForLoopNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/for";
        public static readonly string IdFlowIn = "in";
        
        public static readonly string IdLoopBody = "loopBody";
        public static readonly string IdCompleted = "completed";

        public static readonly string IdStartIndex = "startIndex";
        public static readonly string IdEndIndex = "endIndex";
        public static readonly string IdIndex = "index";
        public static readonly string IdConfigInitialIndex = "initialIndex";

        public Flow_ForLoopNode()
        {
            Op = TypeName;
            Description = "";
            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor
                {
                    Id = IdConfigInitialIndex,
                    Type = "int",
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdLoopBody,
                },
                new FlowSocketDescriptor()
                {
                    Id = IdCompleted,
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdStartIndex,
                    Description = "",
                    SupportedTypes = new string[] { "int" }
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdEndIndex,
                    Description = "",
                    SupportedTypes = new string[] { "int" }
                }
            };

            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdIndex,
                    Description = "",
                    SupportedTypes = new string[] { "int" },
                    expectedType = ExpectedType.Int
                }
            };
        }
    }
}
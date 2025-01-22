namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_DoNNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/doN";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowReset = "reset";
        public static readonly string IdN = "n";
        public static readonly string IdOut = "out";
        public static readonly string IdCurrentExecutionCount = "currentCount";

        public Flow_DoNNode()
        {
            Op = TypeName;
          
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                },
                new FlowSocketDescriptor()
                {
                    Id = IdFlowReset,
                }
            };
            
            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdOut,
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdN,
                    SupportedTypes = new string[] { "int" }
                },
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdCurrentExecutionCount,
                    SupportedTypes = new string[]{"int"},
                    expectedType = ExpectedType.Int
                }
            };
        }
    }
}
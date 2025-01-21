namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_WaitAllNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/waitAll";
        public static readonly string IdFlowOutCompleted = "completed";
        public static readonly string IdFlowOutNotCompleted = "out";
        public static readonly string IdFlowInReset = "reset";
        public static readonly string IdOutRemainingInputs = "remainingInputs";
        public static readonly string IdConfigInputFlows = "inputFlows";
        
        public Flow_WaitAllNode()
        {
            Op = TypeName;
            Description = "";
            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor
                {
                    Id = IdConfigInputFlows,
                    Type = "int",
                }
            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowInReset,
                }
            };
            
            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOutCompleted,
                }
            };

            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOutRemainingInputs,
                    Description = "",
                    SupportedTypes = new string[] { "int" },
                    expectedType = ExpectedType.Int
                }
            };
        }
    }
}
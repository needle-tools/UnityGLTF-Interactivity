namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_SetDelayNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/setDelay";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdDuration = "duration";
        public static readonly string IdFlowOutError = "err";
        public static readonly string IdFlowDone = "done";
        public static readonly string IdFlowInCancel = "cancel";
        public static readonly string IdOutLastDelayIndex = "lastDelayIndex";

        public Flow_SetDelayNode()
        {
            Op = TypeName;
            
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdDuration,
                    SupportedTypes = new string[] {"float"}
                }
            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                },
                new FlowSocketDescriptor()
                {
                    Id = IdFlowInCancel,
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                },
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOutError,
                },
                new FlowSocketDescriptor()
                {
                    Id = IdFlowDone,
                }
            };

            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOutLastDelayIndex,
                    SupportedTypes = new string[] {"int"},
                    expectedType = ExpectedType.Int
                }
            };
        }
    }
}
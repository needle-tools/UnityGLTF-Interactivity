namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_BranchNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/branch";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdCondition = "condition";
        public static readonly string IdFlowOutTrue = "true";
        public static readonly string IdFlowOutFalse = "false";

        public Flow_BranchNode()
        {
            Type = TypeName;
            Configuration = new ConfigDescriptor[]
            {
            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn
                }
            };
            
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdCondition,
                    SupportedTypes = new string[]{"bool"}
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOutTrue
                },
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOutFalse
                }
            };
        }
        
    }
}
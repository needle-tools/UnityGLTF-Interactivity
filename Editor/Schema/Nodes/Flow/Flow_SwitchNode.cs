namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_SwitchNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/switch";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdSelection = "selection";
        public static readonly string IdFDefaultFlowOut = "default";

        public Flow_SwitchNode()
        {
            Op = TypeName;
            Description = "Switch between different flows based on a condition.";
            Configuration = new ConfigDescriptor[]
            {
            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                    Description = "The flow to be followed when the custom event is fired."
                },
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFDefaultFlowOut
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdSelection,
                    Description = "The index of the selected flow.",
                    SupportedTypes = new string[]{"int"}
                }
            };
        }
        
    }
}
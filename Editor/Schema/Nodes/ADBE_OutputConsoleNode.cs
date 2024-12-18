namespace UnityGLTF.Interactivity.Schema
{
    public class ADBE_OutputConsoleNode: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "ADBE/output_console_node";
        public static readonly string EXTENSION_ID = "ADBE_output_console_node";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdMessage = "message";
        
        public ADBE_OutputConsoleNode()
        {
            Type = TypeName;
            
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdMessage,
                    SupportedTypes = new string[]{"string"}
                }
            };
            
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn
                }
            };
            
            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut
                }
            };
        }
    }
}
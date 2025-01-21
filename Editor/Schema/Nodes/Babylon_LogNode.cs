namespace UnityGLTF.Interactivity.Schema
{
    public class Babylon_LogNode: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "babylon/log";
        public static readonly string EXTENSION_ID = "babylon/log";

        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdMessage = "message";
        
        public Babylon_LogNode()
        {
            Op = TypeName;
            Extension = EXTENSION_ID;
            
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
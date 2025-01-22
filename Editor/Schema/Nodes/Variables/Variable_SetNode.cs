namespace UnityGLTF.Interactivity.Schema
{
    internal class Variable_SetNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "variable/set";
        public static readonly string IdConfigVarIndex = "variable";

        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdInputValue = "value";
        
        public Variable_SetNode()
        {
            Op = TypeName;

            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdConfigVarIndex,
                    Type = "int",
                },

            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputValue,
                    SupportedTypes = new string[] { "int", "float", "float2", "float3", "float4", "float4x4" },
                },

            };
        }
    }
}
namespace UnityGLTF.Interactivity.Schema
{
    internal class Variable_GetNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "variable/get";
        public static readonly string IdConfigVarIndex = "variable";
        public static readonly string IdOutputValue = "value";
        
        public Variable_GetNode()
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
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOutputValue,
                    SupportedTypes = new string[] { "int", "float", "float2", "float3", "float4", "float4x4" },
                },

            };
        }
    }
}
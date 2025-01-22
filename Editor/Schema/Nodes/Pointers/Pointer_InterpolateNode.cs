namespace UnityGLTF.Interactivity.Schema
{
    public class Pointer_InterpolateNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "pointer/interpolate";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdValue = "value";
        public static readonly string IdPointer = "pointer";
        public static readonly string IdDuration = "duration";
        public static readonly string IdPoint1 = "p1";
        public static readonly string IdPoint2 = "p2";

        public Pointer_InterpolateNode()
        {
            Op = TypeName;
            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdPointer,
                    Type = "pointer",
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                }
            };
            
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValue,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdDuration,
                    SupportedTypes = new[] { "float" },
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdPoint1,
                    SupportedTypes = new[] { "float2" },
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdPoint2,
                    SupportedTypes = new[] { "float2" },
                },
            };
        }
        
    }
}
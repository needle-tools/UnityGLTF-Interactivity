namespace UnityGLTF.Interactivity.Schema
{
    public class Pointer_InterpolateNode : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "pointer/interpolate";

        [FlowInSocketDescription]
        public const string IdFlowIn = "in";
        [FlowOutSocketDescription]
        public const string IdFlowOut = "out";
        [InputSocketDescription()]
        public const string IdValue = "value";
        [ConfigDescription]
        public const string IdPointer = "pointer";
        [ConfigDescription]
        public const string IdPointerValueType = "type";
        
        [InputSocketDescription(GltfTypes.Float)]
        public const string IdDuration = "duration";
        [InputSocketDescription(GltfTypes.Float)]
        public const string IdPoint1 = "p1";
        [InputSocketDescription(GltfTypes.Float)]
        public const string IdPoint2 = "p2";
    }
}
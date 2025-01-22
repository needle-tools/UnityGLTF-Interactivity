using System.Runtime.InteropServices;

namespace UnityGLTF.Interactivity.Schema.Bitwise
{
    public class Math_LeftShift : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/lsl";
        
        [OutputSocketDescription(GltfTypes.Int)]
        public const string IdOut = "value";
        
        [InputSocketDescription(GltfTypes.Int)]
        public const string IdValueA = "a";
        [InputSocketDescription(GltfTypes.Int)]
        public const string IdValueB = "b";
    }
}
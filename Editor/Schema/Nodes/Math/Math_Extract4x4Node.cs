using System.Collections.Generic;

namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Extract4x4Node : GltfInteractivityNodeSchema
    {
        public override string Op { get; set; } = "math/extract4x4";

        [InputSocketDescription(GltfTypes.Float4x4)]
        public const string IdValueIn = "a";

        public Math_Extract4x4Node() : base()
        {
            for (int i = 0; i < 16; i++)
                OutputValueSockets.Add(i.ToString(), new OutValueSocketDescriptor()
                {
                    SupportedTypes = new string[] { "float" },
                    expectedType =  ExpectedType.Float
                });
        }
    }
}
using System.Collections.Generic;

namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Extract4x4 : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/extract4x4";
        public static readonly string IdValueIn = "a";

        public Math_Extract4x4()
        {
            Type = TypeName;
            
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueIn,
                    SupportedTypes = new string[] { "float4x4" }
                }
            };
   
            var output = new List<OutValueSocketDescriptor>();
            for (int i = 0; i < 16; i++)
            {
                output.Add(
                    new OutValueSocketDescriptor()
                    {
                        Id = i.ToString(),
                        SupportedTypes = new string[] { "float" }
                    }
                    );
            }
            
            OutputValueSockets = output.ToArray();
        }
        
    }
}
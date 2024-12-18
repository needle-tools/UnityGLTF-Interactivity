using UnityGLTF.Interactivity.Export;

namespace UnityGLTF.Interactivity
{
    public class GltfInteractivityUnitExporterNode : GltfInteractivityNode
    {
        public UnitExporter Exporter { get; private set; }
        /// <summary>
        /// The constructor takes in a Schema to which the data should be validated against.
        /// </summary>
        /// <param name="schema"> The Schema to which the data is expected to conform. </param>
        public GltfInteractivityUnitExporterNode(UnitExporter exporter, GltfInteractivityNodeSchema schema) : base(schema)
        {
            Exporter = exporter;
        }
    }
    
}

using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class SelectOnIntegerNode : IUnitExporter
    {
        public System.Type unitType => typeof(Unity.VisualScripting.SelectOnInteger);

        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new SelectOnIntegerNode());
        }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as SelectOnInteger;
            
            GltfInteractivityUnitExporterNode prevSelectNode = null;
            for (int i = 0; i < unit.branches.Count; i++)
            { 
                // Equal Node: Value A = Selector, Value B = Branch Key
                
                var equalNode = unitExporter.CreateNode(new Math_EqNode());
                
                unitExporter.MapInputPortToSocketName(unit.selector, Math_EqNode.IdValueA, equalNode);

                equalNode.ValueIn("a").SetType(TypeRestriction.LimitToInt);
                equalNode.ValueIn("b").SetType(TypeRestriction.LimitToInt);
                
                equalNode.ValueSocketConnectionData[Math_EqNode.IdValueB].Value = (int)unit.branches[i].Key;
                equalNode.ValueSocketConnectionData[Math_EqNode.IdValueB].Type = GltfTypes.TypeIndexByGltfSignature("int");
                
                // Select Node: Value A = Branch Value, Value B =
                var selectNode = unitExporter.CreateNode(new Math_SelectNode());
                
                unitExporter.MapInputPortToSocketName(unit.branches[i].Value, Math_SelectNode.IdValueA, selectNode);

                unitExporter.MapInputPortToSocketName(Math_EqNode.IdOut, equalNode, Math_SelectNode.IdCondition, selectNode);
                if (i == 0)
                {
                    if (!unit.@default.hasDefaultValue && !unit.@default.hasValidConnection)
                    {
                        selectNode.ValueSocketConnectionData[Math_SelectNode.IdValueB].Value = null;
                        selectNode.ValueSocketConnectionData[Math_SelectNode.IdValueB].Type = -1;
                    }
                    else
                        unitExporter.MapInputPortToSocketName(unit.@default, Math_SelectNode.IdValueB, selectNode);
                    
                }
                else
                    unitExporter.MapInputPortToSocketName(Math_SelectNode.IdOutValue, prevSelectNode, Math_SelectNode.IdValueB, selectNode);

                prevSelectNode = selectNode;
            }
            
            unitExporter.MapValueOutportToSocketName(unit.selection, Math_SelectNode.IdOutValue, prevSelectNode);
        }
    }
}

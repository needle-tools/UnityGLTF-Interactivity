using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;
using Random = UnityEngine.Random;

namespace UnityGLTF.Interactivity.Export
{
    public class RandomRangeUnitExport : IUnitExporter
    {
        public Type unitType
        {
            get => typeof(InvokeMember);
        }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Random), nameof(Random.Range), new RandomRangeUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as InvokeMember;
            var varIndex =unitExporter.exportContext.AddVariableWithIdIfNeeded("lastRandom", 312.3f, VariableKind.Graph,
                typeof(float));
            
            var tickNode = unitExporter.CreateNode(new Event_OnTickNode());
      
            var getVar = unitExporter.CreateNode(new Variable_GetNode());
            getVar.ConfigurationData["variable"].Value = varIndex;
            getVar.OutValueSocket["value"].expectedType = ExpectedType.Float;
      
            
            var mulNode = unitExporter.CreateNode(GenericUnitExport.GetAutoGeneratedSchema("math/mul"));
            unitExporter.MapInputPortToSocketName(Event_OnTickNode.IdOutTimeSinceStart, tickNode, "a", mulNode);
            unitExporter.MapInputPortToSocketName("value", getVar, "b", mulNode);

            var mul2Node = unitExporter.CreateNode(GenericUnitExport.GetAutoGeneratedSchema("math/mul"));
            unitExporter.MapInputPortToSocketName("value", mulNode, "a", mul2Node);
            var bSocket = mul2Node.ValueSocketConnectionData["b"];
            bSocket.Type = GltfInteractivityTypeMapping.TypeIndex(typeof(float));
            bSocket.Value = 0.1367f;
            
            var ceilNode = unitExporter.CreateNode(GenericUnitExport.GetAutoGeneratedSchema("math/ceil"));
            unitExporter.MapInputPortToSocketName("value", mul2Node, "a", ceilNode);
            
            var subNode = unitExporter.CreateNode(GenericUnitExport.GetAutoGeneratedSchema("math/sub"));
            unitExporter.MapInputPortToSocketName("value", ceilNode, "a", subNode);
            unitExporter.MapInputPortToSocketName("value", mul2Node, "b", subNode);
            
            subNode.ValueIn("a").ConnectToSource(ceilNode.FirstValueOut());
            subNode.ValueIn("b").ConnectToSource(mul2Node.FirstValueOut());
            
            
            var mixNode = unitExporter.CreateNode(GenericUnitExport.GetAutoGeneratedSchema("math/mix"));
            mixNode.ValueIn("a").SetType(TypeRestriction.LimitToFloat).MapToInputPort(unit.valueInputs[0]);
            mixNode.ValueIn("b").SetType(TypeRestriction.LimitToFloat).MapToInputPort(unit.valueInputs[1]);
            mixNode.ValueIn("c").ConnectToSource(subNode.FirstValueOut());
            mixNode.FirstValueOut().ExpectedType(ExpectedType.Float).MapToPort(unit.result);

            
            
            // TODO: when Int Random, convert to Integer the result > Add FloorToInt
            
            var addNode = unitExporter.CreateNode(new Math_AddNode());
            unitExporter.MapInputPortToSocketName("value", mul2Node, "a", addNode);
            unitExporter.MapInputPortToSocketName("value", mixNode, "b", addNode);
            
            var setVar = unitExporter.CreateNode(new Variable_SetNode());
            setVar.ConfigurationData["variable"].Value = varIndex;
            unitExporter.MapInputPortToSocketName("value", addNode, "value", setVar);
   
            unitExporter.ByPassFlow(unit.enter, unit.exit);
        }
    }
}
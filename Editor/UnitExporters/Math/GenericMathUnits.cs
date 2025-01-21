using System;
using Unity.VisualScripting;
using UnityEditor;

namespace UnityGLTF.Interactivity.Export
{
    /* TODO: MISSING:
     
     !!Max with multiple inputs!! like Add
     
             *
             * math/extract2
             * math/extract3
             * math/extract4

             * math/rotate2d
             * math/rotate3d
             * math/transform
             
             * math/rad  (gibts in unity nur als const)
             * math/deg  (gibts in unity nur als const)
             *
          
             *
             * MATRIX:
             * math/transpose
             * math/determinant
             * math/inverse
             * math/matmul
             * math/combine4x4
             * math/extract4x4
             *

     */
    [InputSocketDefine("a", 0)]
    [InputSocketDefine("b", 1)]
    [InputSocketDefine("c", 2)]
    [OutputSocketDefine("value", 0)]
    public class GenericMathUnit_3In_1Out : GenericUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules_InputABisSame_SameOutTypeAsInputA = new GenericSocketRules();
            socketRules_InputABisSame_SameOutTypeAsInputA.AddExpectedType("value", ExpectedType.FromInputSocket("a"));
            socketRules_InputABisSame_SameOutTypeAsInputA.AddTypeRestriction("a", TypeRestriction.SameAsInputPort("b"));
            socketRules_InputABisSame_SameOutTypeAsInputA.AddTypeRestriction("b", TypeRestriction.SameAsInputPort("a"));
            
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_3In_1Out("math/select", typeof(SelectUnitExport), socketRules_InputABisSame_SameOutTypeAsInputA));
        }
        
        public GenericMathUnit_3In_1Out(string gltfNodeOp, Type unitType, GenericSocketRules socketRules = null) : base(gltfNodeOp, unitType, socketRules)
        {
        }
    }
    
    
    [InputSocketDefine("a", 0)]
    [InputSocketDefine("b", 1)]
    [OutputSocketDefine("value", 0)]
    public class GenericMathUnit_2In_1Out : GenericUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules_InputABisSame_SameOutTypeAsInputA = new GenericSocketRules();
            socketRules_InputABisSame_SameOutTypeAsInputA.AddExpectedType("value", ExpectedType.FromInputSocket("a"));
            socketRules_InputABisSame_SameOutTypeAsInputA.AddTypeRestriction("a", TypeRestriction.SameAsInputPort("b"));
            socketRules_InputABisSame_SameOutTypeAsInputA.AddTypeRestriction("b", TypeRestriction.SameAsInputPort("a"));
            
            var socketRules_InputABisSame_OutFloat = new GenericSocketRules();
            socketRules_InputABisSame_OutFloat.AddExpectedType("value", ExpectedType.Float);
            socketRules_InputABisSame_OutFloat.AddTypeRestriction("a", TypeRestriction.SameAsInputPort("b"));
            socketRules_InputABisSame_OutFloat.AddTypeRestriction("b", TypeRestriction.SameAsInputPort("a"));

            var socketRules_InputABisSame_OutBool = new GenericSocketRules();
            socketRules_InputABisSame_OutBool.AddExpectedType("value", ExpectedType.Bool);
            socketRules_InputABisSame_OutBool.AddTypeRestriction("a", TypeRestriction.SameAsInputPort("b"));
            socketRules_InputABisSame_OutBool.AddTypeRestriction("b", TypeRestriction.SameAsInputPort("a"));
            
            var socketRules_AllBool = new GenericSocketRules();
            socketRules_AllBool.AddExpectedType("value", ExpectedType.Bool);
            socketRules_AllBool.AddTypeRestriction("a", TypeRestriction.LimitToBool);
            socketRules_AllBool.AddTypeRestriction("b", TypeRestriction.LimitToBool);

            var socketRules_InputVector3_OutFloat = new GenericSocketRules();
            socketRules_InputVector3_OutFloat.AddExpectedType("value", ExpectedType.Float);
            socketRules_InputVector3_OutFloat.AddTypeRestriction("a", TypeRestriction.LimitToFloat3);
            socketRules_InputVector3_OutFloat.AddTypeRestriction("b", TypeRestriction.LimitToFloat3);

            
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/eq", typeof(Equal),socketRules_InputABisSame_OutBool));

            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/sub", typeof(GenericSubtract), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/sub", typeof(ScalarSubtract), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/sub", typeof(Vector2Subtract), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/sub", typeof(Vector3Subtract), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/sub", typeof(Vector4Subtract), socketRules_InputABisSame_SameOutTypeAsInputA));
            
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/mul", typeof(GenericMultiply), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/mul", typeof(Vector2Multiply), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/mul", typeof(Vector3Multiply), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/mul", typeof(Vector4Multiply), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/mul", typeof(ScalarMultiply), socketRules_InputABisSame_SameOutTypeAsInputA));
            
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/div", typeof(GenericDivide), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/div", typeof(ScalarDivide), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/div", typeof(Vector2Divide), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/div", typeof(Vector3Divide), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/div", typeof(Vector4Divide), socketRules_InputABisSame_SameOutTypeAsInputA));
            
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/rem", typeof(GenericModulo), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/rem", typeof(ScalarModulo), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/rem", typeof(Vector2Modulo), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/rem", typeof(Vector3Modulo), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/rem", typeof(Vector4Modulo), socketRules_InputABisSame_SameOutTypeAsInputA));
            
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/min", typeof(ScalarMinimum), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/min", typeof(Vector2Minimum), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/min", typeof(Vector3Minimum), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/min", typeof(Vector4Minimum), socketRules_InputABisSame_SameOutTypeAsInputA));
            
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/max", typeof(ScalarMaximum), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/max", typeof(Vector2Maximum), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/max", typeof(Vector3Maximum), socketRules_InputABisSame_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/max", typeof(Vector4Maximum), socketRules_InputABisSame_SameOutTypeAsInputA));

            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/lt", typeof(Less), socketRules_InputABisSame_OutBool));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/le", typeof(LessOrEqual), socketRules_InputABisSame_OutBool));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/gt", typeof(Greater), socketRules_InputABisSame_OutBool));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/ge", typeof(GreaterOrEqual), socketRules_InputABisSame_OutBool));
       
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/dot", typeof(Vector2DotProduct), socketRules_InputABisSame_OutFloat));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/dot", typeof(Vector3DotProduct), socketRules_InputABisSame_OutFloat));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/dot", typeof(Vector4DotProduct), socketRules_InputABisSame_OutFloat));

            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/cross", typeof(Vector3CrossProduct), socketRules_InputABisSame_SameOutTypeAsInputA));

            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/and", typeof(And), socketRules_AllBool));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/or", typeof(Or), socketRules_AllBool));
            UnitExporterRegistry.RegisterExporter(new GenericMathUnit_2In_1Out("math/xor", typeof(ExclusiveOr), socketRules_AllBool));
        }
        
        public GenericMathUnit_2In_1Out(string gltfNodeOp, Type unitType, GenericSocketRules socketRules = null) : base(gltfNodeOp, unitType, socketRules)
        {
        }
    }
    
    [InputSocketDefine("a", 0)]
    [OutputSocketDefine("value", 0)]
    public class GenericMathNodesOneInOut : GenericUnitExport
    {
        [InitializeOnLoadMethod]
        public static void Register()
        {
            var socketRules_SameOutTypeAsInputA = new GenericSocketRules();
            socketRules_SameOutTypeAsInputA.AddExpectedType("value", ExpectedType.FromInputSocket("a"));

            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/abs", typeof(ScalarAbsolute), socketRules_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/abs", typeof(Vector2Absolute), socketRules_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/abs", typeof(Vector3Absolute), socketRules_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/abs", typeof(Vector4Absolute), socketRules_SameOutTypeAsInputA));

            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/normalize", typeof(Vector2Normalize), socketRules_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/normalize", typeof(Vector3Normalize), socketRules_SameOutTypeAsInputA));
            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/normalize", typeof(Vector4Normalize), socketRules_SameOutTypeAsInputA));

            UnitExporterRegistry.RegisterExporter(new GenericMathNodesOneInOut("math/not", typeof(Negate), socketRules_SameOutTypeAsInputA));
        }
        
        public GenericMathNodesOneInOut(string gltfNodeOp, Type unitType, GenericSocketRules socketRules = null) : base(gltfNodeOp, unitType, socketRules)
        {
        }
    }
}
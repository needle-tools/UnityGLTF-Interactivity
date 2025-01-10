using Unity.VisualScripting;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public static class TransformHelpers
    {
        public static void GetLocalPosition(UnitExporter unitExporter, ValueInput target,
            out GltfInteractivityUnitExporterNode.ValueOutputSocketData positionOutput)
        {
            var getPosition = unitExporter.CreateNode(new Pointer_GetNode());
            getPosition.FirstValueOut().ExpectedType(ExpectedType.Float3);

            SpaceConversionHelpers.AddSpaceConversionNodes(unitExporter, getPosition.FirstValueOut(),
                out var convertedOutput);
            positionOutput = convertedOutput;
            positionOutput.ExpectedType(ExpectedType.Float3);

            if (GltfInteractivityNodeHelper.IsMainCameraInInput(target))
            {
                GltfInteractivityNodeHelper.AddPointerConfig(getPosition, "/activeCamera/position");
                return;
            }

            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
                target, getPosition,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/translation");
        }

        public static void GetLocalPosition(UnitExporter unitExporter, ValueInput target, ValueOutput positionOutput)
        {
            GetLocalPosition(unitExporter, target, out var positionOutputData);
            positionOutputData.MapToPort(positionOutput);
        }

        public static void SetLocalPosition(UnitExporter unitExporter, ValueInput target, ValueInput position,
            ControlInput flowIn, ControlOutput flowOut)
        {
            var setPosition = unitExporter.CreateNode(new Pointer_SetNode());

            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
                target, setPosition,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/translation");

            SpaceConversionHelpers.AddSpaceConversionNodes(unitExporter, position, out var convertedOutput);
            setPosition.ValueIn(Pointer_SetNode.IdValue).ConnectToSource(convertedOutput);

            setPosition.FlowIn(Pointer_SetNode.IdFlowIn).MapToControlInput(flowIn);
            setPosition.FlowOut(Pointer_SetNode.IdFlowOut).MapToControlOutput(flowOut);
        }

        public static void SetLocalPosition(UnitExporter unitExporter, ValueInput target,
            GltfInteractivityUnitExporterNode.ValueOutputSocketData position, ControlInput flowIn,
            ControlOutput flowOut)
        {
            var setPosition = unitExporter.CreateNode(new Pointer_SetNode());

            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
                target, setPosition,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/translation");

            SpaceConversionHelpers.AddSpaceConversionNodes(unitExporter, position, out var convertedOutput);
            setPosition.ValueIn(Pointer_SetNode.IdValue).ConnectToSource(convertedOutput);

            setPosition.FlowIn(Pointer_SetNode.IdFlowIn).MapToControlInput(flowIn);
            setPosition.FlowOut(Pointer_SetNode.IdFlowOut).MapToControlOutput(flowOut);
        }

        public static void GetLocalRotation(UnitExporter unitExporter, ValueInput target,
            out GltfInteractivityUnitExporterNode.ValueOutputSocketData value)
        {
            var getRotation = unitExporter.CreateNode(new Pointer_GetNode());
            getRotation.OutValueSocket[Pointer_GetNode.IdValue].expectedType = ExpectedType.GtlfType("float4");

            SpaceConversionHelpers.AddRotationSpaceConversionNodes(unitExporter, getRotation.FirstValueOut(),
                out var convertedRotation);
            value = convertedRotation;

            //unitExporter.MapValueOutportToSocketName(unit.value, Pointer_GetNode.IdValue, getRotation);

            if (GltfInteractivityNodeHelper.IsMainCameraInInput(target))
            {
                GltfInteractivityNodeHelper.AddPointerConfig(getRotation, "/activeCamera/rotation");
                QuaternionHelpers.Invert(unitExporter, convertedRotation, out var invertedRotation);
                value = invertedRotation;
                return;
            }

            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
                target, getRotation,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/rotation");
        }

        public static void GetLocalRotation(UnitExporter unitExporter, ValueInput target, ValueOutput value)
        {
            GetLocalRotation(unitExporter, target, out var valueSocket);
            valueSocket.MapToPort(value);
        }

        public static void SetLocalRotation(UnitExporter unitExporter, ValueInput target,
            GltfInteractivityUnitExporterNode.ValueOutputSocketData rotationInput,
            ControlInput flowIn, ControlOutput flowOut)
        {
            var setRotation = unitExporter.CreateNode(new Pointer_SetNode());

            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
                target, setRotation,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/rotation");

            SpaceConversionHelpers.AddRotationSpaceConversionNodes(unitExporter, rotationInput,
                out var convertedRotation);
            setRotation.ValueIn(Pointer_SetNode.IdValue).ConnectToSource(convertedRotation);
            setRotation.FlowOut(Pointer_SetNode.IdFlowOut).MapToControlOutput(flowOut);
            setRotation.FlowIn(Pointer_SetNode.IdFlowIn).MapToControlInput(flowIn);
        }

        public static void SetLocalRotation(UnitExporter unitExporter, ValueInput target, ValueInput rotationInput,
            ControlInput flowIn, ControlOutput flowOut)
        {
            var setRotation = unitExporter.CreateNode(new Pointer_SetNode());

            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
                target, setRotation,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/rotation");

            SpaceConversionHelpers.AddRotationSpaceConversionNodes(unitExporter, rotationInput,
                out var convertedRotation);
            setRotation.ValueIn(Pointer_SetNode.IdValue).ConnectToSource(convertedRotation);
            setRotation.FlowOut(Pointer_SetNode.IdFlowOut).MapToControlOutput(flowOut);
            setRotation.FlowIn(Pointer_SetNode.IdFlowIn).MapToControlInput(flowIn);
        }
    }
}
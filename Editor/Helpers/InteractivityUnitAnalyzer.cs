using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Export;

namespace UnityGLTF.Interactivity
{
    [Analyser(typeof(Unit))] 
    [Analyser(typeof(InvokeMember))] 
    [Analyser(typeof(GetMember))] 
    [Analyser(typeof(SetMember))] 
    [UsedImplicitly]
    public class InteractivityUnitAnalyzer: UnitAnalyser<IUnit>
    {
        public InteractivityUnitAnalyzer(GraphReference reference, IUnit target) : base(reference, target)
        {
            TypeCache.GetTypesDerivedFrom<IUnitExporter>();
        }

        protected override IEnumerable<Warning> Warnings()
        {
            foreach (var baseWarning in base.Warnings())
            {
                yield return baseWarning;
            }
            
            // These are exported implicitly, so let's not warn.
            // TODO for some types we might want to warn if we don't support them altogether
            if (target is Literal || target is This)
                yield break;
            
            if (!UnitExporterRegistry.HasUnitExporter(target))
                yield return Warning.Error("Node will not be exported with KHR_interactivity");
            else
            {
                string[] supportedMembers = null;
                
                if (target is Expose expose)
                    supportedMembers = ExposeUnitExport.GetSupportedMembers(expose.type);
                
                if (supportedMembers != null)
                    yield return Warning.Info("Node will be exported with KHR_interactivity." 
                                              +  System.Environment.NewLine 
                                              + "Supported members:"
                                              + System.Environment.NewLine + "•"
                                              + string.Join(System.Environment.NewLine+ "•", supportedMembers));
                else
                    yield return Warning.Info("Node will be exported with KHR_interactivity");
            }
        }
    }
}
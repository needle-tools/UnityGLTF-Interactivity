using Unity.VisualScripting;
using UnityGLTF.Interactivity;

namespace Editor.UnitExporters
{
    public interface IUnitExporterFeedback
    {
        UnitLogs GetFeedback(IUnit unit);
    }
}
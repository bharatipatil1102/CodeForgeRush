using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeForgeRush.UI
{
    public sealed class ProgramDropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] private ProgramBuilderUI programBuilder;
        [SerializeField] private HUDController hud;

        public void OnDrop(PointerEventData eventData)
        {
            if (DragContext.Current == null || programBuilder == null)
                return;

            bool added = programBuilder.TryAddInstruction(DragContext.ToInstruction(DragContext.Current));
            if (!added && hud != null)
                hud.SetStatus("Instruction limit reached.");
        }
    }
}

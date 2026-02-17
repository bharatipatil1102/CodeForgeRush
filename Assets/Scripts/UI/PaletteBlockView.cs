using CodeForgeRush.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class PaletteBlockView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private OpCode opCode;
        [SerializeField] private int argument;
        [SerializeField] private Image dragIcon;

        private Canvas _rootCanvas;
        private RectTransform _iconRect;
        private Vector3 _iconStartPosition;
        private bool _isDragging;

        public OpCode OpCode => opCode;
        public int Argument => argument;

        private void Awake()
        {
            _rootCanvas = GetComponentInParent<Canvas>();
            if (dragIcon != null)
            {
                _iconRect = dragIcon.rectTransform;
                _iconStartPosition = _iconRect.position;
                dragIcon.enabled = false;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragContext.Current = this;
            _isDragging = true;

            if (dragIcon != null)
            {
                dragIcon.enabled = true;
                _iconRect.position = eventData.position;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || dragIcon == null)
                return;

            if (_rootCanvas != null && _rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    _rootCanvas.transform as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out var worldPoint);
                _iconRect.position = worldPoint;
            }
            else
            {
                _iconRect.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            DragContext.Current = null;

            if (dragIcon != null)
            {
                dragIcon.enabled = false;
                _iconRect.position = _iconStartPosition;
            }
        }
    }
}

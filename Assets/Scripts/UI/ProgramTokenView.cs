using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class ProgramTokenView : MonoBehaviour
    {
        [SerializeField] private Text label;
        [SerializeField] private Button removeButton;

        private int _index;
        private Action<int> _onRemove;

        public void Bind(string text, int index, Action<int> onRemove)
        {
            _index = index;
            _onRemove = onRemove;
            if (label != null)
                label.text = text;

            if (removeButton != null)
            {
                removeButton.onClick.RemoveAllListeners();
                removeButton.onClick.AddListener(OnRemoveClicked);
            }
        }

        private void OnRemoveClicked()
        {
            _onRemove?.Invoke(_index);
        }
    }
}

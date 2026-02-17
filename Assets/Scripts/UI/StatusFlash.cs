using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class StatusFlash : MonoBehaviour
    {
        [SerializeField] private Graphic target;
        [SerializeField] private Color successColor = new Color(0.28f, 0.95f, 0.42f);
        [SerializeField] private Color failColor = new Color(0.95f, 0.24f, 0.24f);
        [SerializeField] private float duration = 0.22f;

        private Color _base;
        private Coroutine _routine;

        private void Awake()
        {
            if (target == null)
                target = GetComponent<Graphic>();

            if (target != null)
                _base = target.color;
        }

        public void FlashSuccess()
        {
            Flash(successColor);
        }

        public void FlashFail()
        {
            Flash(failColor);
        }

        private void Flash(Color color)
        {
            if (target == null)
                return;

            if (_routine != null)
                StopCoroutine(_routine);
            _routine = StartCoroutine(FlashRoutine(color));
        }

        private IEnumerator FlashRoutine(Color color)
        {
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float p = Mathf.Clamp01(t / duration);
                target.color = Color.Lerp(_base, color, p);
                yield return null;
            }

            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float p = Mathf.Clamp01(t / duration);
                target.color = Color.Lerp(color, _base, p);
                yield return null;
            }

            target.color = _base;
            _routine = null;
        }
    }
}

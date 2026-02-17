using System.Collections;
using UnityEngine;

namespace CodeForgeRush.UI
{
    public sealed class ButtonPulse : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private float scaleUp = 1.06f;
        [SerializeField] private float duration = 0.18f;

        private Vector3 _baseScale;
        private Coroutine _routine;

        private void Awake()
        {
            if (target == null)
                target = transform as RectTransform;

            if (target != null)
                _baseScale = target.localScale;
        }

        public void Pulse()
        {
            if (target == null)
                return;

            if (_routine != null)
                StopCoroutine(_routine);
            _routine = StartCoroutine(PulseRoutine());
        }

        private IEnumerator PulseRoutine()
        {
            float t = 0f;
            Vector3 peak = _baseScale * scaleUp;

            while (t < duration)
            {
                t += Time.deltaTime;
                float p = Mathf.Clamp01(t / duration);
                target.localScale = Vector3.Lerp(_baseScale, peak, p);
                yield return null;
            }

            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float p = Mathf.Clamp01(t / duration);
                target.localScale = Vector3.Lerp(peak, _baseScale, p);
                yield return null;
            }

            target.localScale = _baseScale;
            _routine = null;
        }
    }
}

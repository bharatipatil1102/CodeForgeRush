using System;
using System.Collections;
using UnityEngine;

namespace CodeForgeRush.Ads
{
    public sealed class MockRewardedAdService : MonoBehaviour, IRewardedAdService
    {
        [SerializeField] private float fakeAdDurationSeconds = 1.0f;
        public bool IsReady { get; private set; }

        public void Initialize()
        {
            IsReady = true;
        }

        public void ShowRewarded(Action<bool, string> onComplete)
        {
            if (!IsReady)
            {
                onComplete?.Invoke(false, "Ad not ready.");
                return;
            }

            StartCoroutine(FakeAdRoutine(onComplete));
        }

        private IEnumerator FakeAdRoutine(Action<bool, string> onComplete)
        {
            yield return new WaitForSeconds(fakeAdDurationSeconds);
            onComplete?.Invoke(true, "Ad complete.");
        }
    }
}

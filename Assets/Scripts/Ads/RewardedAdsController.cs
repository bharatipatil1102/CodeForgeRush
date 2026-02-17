using System;
using UnityEngine;

namespace CodeForgeRush.Ads
{
    public sealed class RewardedAdsController : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour serviceBehaviour;

        private IRewardedAdService _service;

        public bool IsReady => _service != null && _service.IsReady;

        private void Awake()
        {
            _service = serviceBehaviour as IRewardedAdService;
            if (_service == null)
            {
                Debug.LogWarning("RewardedAdsController: serviceBehaviour does not implement IRewardedAdService.");
                return;
            }

            _service.Initialize();
        }

        public void ShowRewarded(Action<bool, string> onComplete)
        {
            if (_service == null)
            {
                onComplete?.Invoke(false, "Ad service unavailable.");
                return;
            }

            _service.ShowRewarded(onComplete);
        }
    }
}

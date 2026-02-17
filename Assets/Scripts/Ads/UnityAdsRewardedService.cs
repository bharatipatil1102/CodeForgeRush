#if UNITY_ADS
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeForgeRush.Ads
{
    public sealed class UnityAdsRewardedService : MonoBehaviour, IRewardedAdService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string androidGameId = "REPLACE_ANDROID_GAME_ID";
        [SerializeField] private string iosGameId = "REPLACE_IOS_GAME_ID";
        [SerializeField] private string rewardedPlacementId = "Rewarded_Android";
        [SerializeField] private bool testMode = true;

        private Action<bool, string> _pending;

        public bool IsReady { get; private set; }

        public void Initialize()
        {
            string gameId = Application.platform == RuntimePlatform.IPhonePlayer ? iosGameId : androidGameId;
            Advertisement.Initialize(gameId, testMode, this);
        }

        public void ShowRewarded(Action<bool, string> onComplete)
        {
            _pending = onComplete;
            if (!IsReady)
            {
                onComplete?.Invoke(false, "Ad not ready.");
                return;
            }

            Advertisement.Show(rewardedPlacementId, this);
        }

        public void OnInitializationComplete()
        {
            Advertisement.Load(rewardedPlacementId, this);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogWarning($"Unity Ads init failed: {error} {message}");
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId == rewardedPlacementId)
                IsReady = true;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            IsReady = false;
            Debug.LogWarning($"Unity Ads load failed: {placementId} {error} {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            _pending?.Invoke(false, "Ad failed.");
            _pending = null;
            IsReady = false;
            Advertisement.Load(rewardedPlacementId, this);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            bool rewarded = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
            _pending?.Invoke(rewarded, rewarded ? "Ad complete." : "Ad skipped.");
            _pending = null;
            IsReady = false;
            Advertisement.Load(rewardedPlacementId, this);
        }
    }
}
#endif

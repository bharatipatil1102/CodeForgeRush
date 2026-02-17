using CodeForgeRush.Core;
using CodeForgeRush.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class MonetizationPanelController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private HUDController hud;
        [SerializeField] private RewardedAdsController rewardedAdsController;

        [Header("Buttons")]
        [SerializeField] private Button claimMissionButton;
        [SerializeField] private Button rewardedHintButton;
        [SerializeField] private Button spendHintButton;
        [SerializeField] private Button buyNeonSkinButton;
        [SerializeField] private Button equipNeonSkinButton;

        private const string NeonSkinId = "neon_blade";

        private void Start()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();

            if (claimMissionButton != null)
            {
                claimMissionButton.onClick.RemoveAllListeners();
                claimMissionButton.onClick.AddListener(OnClaimMission);
            }

            if (rewardedHintButton != null)
            {
                rewardedHintButton.onClick.RemoveAllListeners();
                rewardedHintButton.onClick.AddListener(OnRewardedHint);
            }

            if (spendHintButton != null)
            {
                spendHintButton.onClick.RemoveAllListeners();
                spendHintButton.onClick.AddListener(OnSpendHint);
            }

            if (buyNeonSkinButton != null)
            {
                buyNeonSkinButton.onClick.RemoveAllListeners();
                buyNeonSkinButton.onClick.AddListener(OnBuyNeonSkin);
            }

            if (equipNeonSkinButton != null)
            {
                equipNeonSkinButton.onClick.RemoveAllListeners();
                equipNeonSkinButton.onClick.AddListener(OnEquipNeonSkin);
            }
        }

        private void OnClaimMission()
        {
            int reward = gameManager.ClaimMissionRewards();
            if (reward > 0)
                hud?.SetStatus($"Mission reward claimed: +{reward} coins.");
            else
                hud?.SetStatus("No mission reward available yet.");
        }

        private void OnRewardedHint()
        {
            if (rewardedAdsController == null)
            {
                hud?.SetStatus("Rewarded ad service not configured.");
                return;
            }

            rewardedAdsController.ShowRewarded((ok, message) =>
            {
                if (!ok)
                {
                    hud?.SetStatus(message);
                    return;
                }

                gameManager.ClaimRewardedHint();
                hud?.SetStatus("Rewarded hint claimed (+1 token).");
            });
        }

        private void OnSpendHint()
        {
            bool ok = gameManager.TrySpendHint(out string message);
            if (ok)
                hud?.SetStatus("Hint unlocked: prioritize shortest path + loop.");
            else
                hud?.SetStatus(message);
        }

        private void OnBuyNeonSkin()
        {
            gameManager.TryBuySkin(NeonSkinId, out string message);
            hud?.SetStatus(message);
        }

        private void OnEquipNeonSkin()
        {
            gameManager.TryEquipSkin(NeonSkinId, out string message);
            hud?.SetStatus(message);
        }
    }
}

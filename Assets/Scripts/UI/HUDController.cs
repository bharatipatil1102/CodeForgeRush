using CodeForgeRush.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class HUDController : MonoBehaviour
    {
        [SerializeField] private Text levelText;
        [SerializeField] private Text rankText;
        [SerializeField] private Text coinsText;
        [SerializeField] private Text missionText;
        [SerializeField] private Text seasonText;
        [SerializeField] private Text skinText;
        [SerializeField] private Text statusText;

        public void RefreshProfile(PlayerProfile profile, int missionATarget, int missionBTarget)
        {
            if (levelText != null)
                levelText.text = $"Level: {profile.CurrentLevel}/15000";
            if (rankText != null)
                rankText.text = $"Rank: {profile.Rank}";
            if (coinsText != null)
                coinsText.text = $"Coins: {profile.Coins} | Hints: {profile.HintTokens}";
            if (missionText != null)
                missionText.text = $"Daily: {profile.MissionAProgress}/{missionATarget} | Perfect: {profile.MissionBProgress}/{missionBTarget}";
            if (seasonText != null)
                seasonText.text = $"Season Tier: {profile.SeasonTier} | XP: {profile.SeasonXp}";
            if (skinText != null)
                skinText.text = $"Skin: {profile.EquippedSkinId}";
        }

        public void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }
    }
}

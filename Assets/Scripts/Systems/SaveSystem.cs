using CodeForgeRush.Models;
using UnityEngine;

namespace CodeForgeRush.Systems
{
    public sealed class SaveSystem
    {
        private const string Key = "cfr_player_profile_v1";

        public PlayerProfile Load()
        {
            if (!PlayerPrefs.HasKey(Key))
                return new PlayerProfile();

            string json = PlayerPrefs.GetString(Key);
            if (string.IsNullOrWhiteSpace(json))
                return new PlayerProfile();

            return JsonUtility.FromJson<PlayerProfile>(json) ?? new PlayerProfile();
        }

        public void Save(PlayerProfile profile)
        {
            string json = JsonUtility.ToJson(profile);
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
        }
    }
}

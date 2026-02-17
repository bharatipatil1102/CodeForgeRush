using System;
using System.IO;
using CodeForgeRush.Config;
using UnityEngine;

namespace CodeForgeRush.Systems
{
    public sealed class LiveOpsConfigService
    {
        private const string FileName = "liveops_config.json";

        public LiveOpsConfig Load()
        {
            string path = Path.Combine(Application.streamingAssetsPath, FileName);
            try
            {
                if (!File.Exists(path))
                    return LiveOpsConfig.Default();

                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    return LiveOpsConfig.Default();

                var config = JsonUtility.FromJson<LiveOpsConfig>(json) ?? LiveOpsConfig.Default();
                config.Sanitize();
                Debug.Log($"LiveOps config loaded v{config.version} from {path}");
                return config;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to load liveops config from {path}: {ex.Message}");
                return LiveOpsConfig.Default();
            }
        }
    }
}

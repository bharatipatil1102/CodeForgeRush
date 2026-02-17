using CodeForgeRush.Core;
using CodeForgeRush.Gameplay;
using CodeForgeRush.Models;
using UnityEngine;

namespace CodeForgeRush.Audio
{
    public sealed class AudioManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip runSuccess;
        [SerializeField] private AudioClip runFail;
        [SerializeField] private AudioClip levelLoaded;

        private void Start()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();

            if (gameManager != null)
            {
                gameManager.LevelLoaded += OnLevelLoaded;
                gameManager.RunResolved += OnRunResolved;
            }

            if (musicSource != null && backgroundMusic != null)
            {
                musicSource.loop = true;
                musicSource.clip = backgroundMusic;
                musicSource.Play();
            }
        }

        private void OnDestroy()
        {
            if (gameManager == null)
                return;

            gameManager.LevelLoaded -= OnLevelLoaded;
            gameManager.RunResolved -= OnRunResolved;
        }

        private void OnLevelLoaded(LevelDefinition obj)
        {
            PlaySfx(levelLoaded);
        }

        private void OnRunResolved(SimulationResult result)
        {
            PlaySfx(result.Success ? runSuccess : runFail);
        }

        private void PlaySfx(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
                sfxSource.PlayOneShot(clip);
        }
    }
}

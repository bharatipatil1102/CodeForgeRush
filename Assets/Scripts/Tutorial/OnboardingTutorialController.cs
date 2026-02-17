using CodeForgeRush.Core;
using CodeForgeRush.Gameplay;
using CodeForgeRush.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.Tutorial
{
    public sealed class OnboardingTutorialController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private Text tutorialText;
        [SerializeField] private Button nextButton;

        private readonly string[] _steps =
        {
            "Welcome to CodeForge Rush. Drag code blocks to build a program.",
            "Use MOVE and TURN blocks to navigate to GOAL.",
            "Collect coins for bonus rewards and season progress.",
            "Use ATTACK_AHEAD to clear enemies and damage boss cores.",
            "Tip: loops reduce script size and help perfect runs."
        };

        private bool _active;

        private void Start()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();

            if (gameManager == null)
                return;

            gameManager.LevelLoaded += OnLevelLoaded;
            gameManager.RunResolved += OnRunResolved;

            if (nextButton != null)
            {
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(NextStep);
            }

            RefreshVisibility();
        }

        private void OnDestroy()
        {
            if (gameManager == null)
                return;

            gameManager.LevelLoaded -= OnLevelLoaded;
            gameManager.RunResolved -= OnRunResolved;
        }

        private void OnLevelLoaded(LevelDefinition level)
        {
            if (gameManager.Profile.TutorialCompleted)
            {
                Hide();
                return;
            }

            if (level.LevelNumber <= gameManager.TutorialLevelsCount)
            {
                _active = true;
                ShowStep(gameManager.Profile.TutorialStepIndex);
            }
            else
            {
                gameManager.MarkTutorialCompleted();
                Hide();
            }
        }

        private void OnRunResolved(SimulationResult result)
        {
            if (!_active || gameManager.Profile.TutorialCompleted)
                return;

            if (result.Success && gameManager.Profile.TutorialStepIndex < _steps.Length - 1)
            {
                gameManager.AdvanceTutorialStep();
                ShowStep(gameManager.Profile.TutorialStepIndex);
            }
        }

        private void NextStep()
        {
            if (!_active || gameManager.Profile.TutorialCompleted)
                return;

            if (gameManager.Profile.TutorialStepIndex >= _steps.Length - 1)
            {
                gameManager.MarkTutorialCompleted();
                Hide();
                return;
            }

            gameManager.AdvanceTutorialStep();
            ShowStep(gameManager.Profile.TutorialStepIndex);
        }

        private void RefreshVisibility()
        {
            if (gameManager.Profile.TutorialCompleted || gameManager.Profile.CurrentLevel > gameManager.TutorialLevelsCount)
            {
                Hide();
                return;
            }

            _active = true;
            ShowStep(gameManager.Profile.TutorialStepIndex);
        }

        private void ShowStep(int index)
        {
            index = Mathf.Clamp(index, 0, _steps.Length - 1);
            if (tutorialPanel != null)
                tutorialPanel.SetActive(true);
            if (tutorialText != null)
                tutorialText.text = _steps[index];
        }

        private void Hide()
        {
            _active = false;
            if (tutorialPanel != null)
                tutorialPanel.SetActive(false);
        }
    }
}

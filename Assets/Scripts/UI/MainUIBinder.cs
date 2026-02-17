using CodeForgeRush.Core;
using CodeForgeRush.Gameplay;
using CodeForgeRush.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class MainUIBinder : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ProgramBuilderUI programBuilder;
        [SerializeField] private HUDController hud;
        [SerializeField] private LevelGridView levelGrid;
        [SerializeField] private Button runButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private ButtonPulse runButtonPulse;
        [SerializeField] private StatusFlash statusFlash;

        private void Start()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();

            if (gameManager == null)
            {
                Debug.LogError("MainUIBinder: GameManager not found in scene.");
                return;
            }

            gameManager.LevelLoaded += OnLevelLoaded;
            gameManager.ProfileChanged += OnProfileChanged;
            gameManager.RunResolved += OnRunResolved;

            if (runButton != null)
            {
                runButton.onClick.RemoveAllListeners();
                runButton.onClick.AddListener(OnRunClicked);
            }

            if (clearButton != null)
            {
                clearButton.onClick.RemoveAllListeners();
                clearButton.onClick.AddListener(OnClearClicked);
            }

            BindInitialState();
        }

        private void OnDestroy()
        {
            if (gameManager == null)
                return;

            gameManager.LevelLoaded -= OnLevelLoaded;
            gameManager.ProfileChanged -= OnProfileChanged;
            gameManager.RunResolved -= OnRunResolved;
        }

        private void BindInitialState()
        {
            if (gameManager.CurrentLevel != null)
                OnLevelLoaded(gameManager.CurrentLevel);

            if (gameManager.Profile != null)
                OnProfileChanged(gameManager.Profile);
        }

        private void OnLevelLoaded(LevelDefinition level)
        {
            if (programBuilder != null)
            {
                programBuilder.ClearProgram();
                programBuilder.SetMaxInstructions(level.MaxInstructions);
            }

            levelGrid?.Render(level);
            if (level.IsBossLevel)
                hud?.SetStatus($"BOSS LEVEL: defeat core (HP {level.BossHealth}) then reach GOAL. Par: {level.ParMoves}");
            else
                hud?.SetStatus($"Build a script to reach GOAL. Par: {level.ParMoves} steps");
        }

        private void OnProfileChanged(PlayerProfile profile)
        {
            hud?.RefreshProfile(profile, gameManager.MissionATarget, gameManager.MissionBTarget);
        }

        private void OnRunResolved(SimulationResult result)
        {
            if (result.Success)
            {
                hud?.SetStatus($"Success! Steps: {result.StepsUsed}, Coins: +{result.CoinsCollected}, BossHit: {result.BossDamageDealt}, HP: {result.HealthRemaining}");
                statusFlash?.FlashSuccess();
            }
            else
            {
                hud?.SetStatus($"Fail: {result.Error} (HP {result.HealthRemaining})");
                statusFlash?.FlashFail();
            }
        }

        private void OnRunClicked()
        {
            if (programBuilder == null || programBuilder.Instructions.Count == 0)
            {
                hud?.SetStatus("Drag blocks into the program first.");
                return;
            }

            runButtonPulse?.Pulse();
            gameManager.SubmitProgram(programBuilder.Instructions);
        }

        private void OnClearClicked()
        {
            programBuilder?.ClearProgram();
            hud?.SetStatus("Program cleared.");
        }
    }
}

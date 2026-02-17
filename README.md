# CodeForge Rush (iOS + Android)

CodeForge Rush is a mobile-first puzzle game for ages 12-22 where players learn coding logic through short, addictive sessions.

For exact Unity hierarchy and Inspector wiring, use:
- `LAUNCH_CHECKLIST.md`

## Core Idea
Players build tiny programs using drag-and-drop code blocks to solve levels. Each solved level teaches one coding concept while preserving a fast game loop.

## Why this is addictive (healthy loop)
- 30-90 second levels (fast win cycle)
- Daily streak + missions
- Unlockable skins and trails
- Win streak multipliers
- Soft fail recovery (hints + retry economy)
- Endless progression up to level 15,000 via procedural generation

## Learning Outcomes
- Sequence and execution order
- Loops
- Conditions
- Functions
- Variables and counters
- Basic debugging mindset

## Project Structure
- `Assets/Scripts/Core`: app orchestration and runtime state
- `Assets/Scripts/Gameplay`: puzzle simulation + validation
- `Assets/Scripts/Models`: strongly typed gameplay models
- `Assets/Scripts/Systems`: progression, level gen, reward, save, missions
- `Assets/Scripts/Data`: balancing constants

## How 15,000 levels are built
Levels are generated from deterministic seeds (`levelNumber`) plus stage bands. Every level can be recreated without storing 15,000 files.

## Unity Setup (quick)
1. Open project in Unity 2022.3+ LTS.
2. Create a scene `Assets/Scenes/Main.unity`.
3. Add an empty GameObject called `Bootstrap`.
4. Attach `GameManager` script.
5. Press Play.

## Export Targets
- Android: Player Settings -> Android -> IL2CPP/ARM64
- iOS: Player Settings -> iOS -> IL2CPP, then build Xcode project

## Monetization (optional and safe for teens)
- Cosmetic-only pass
- Rewarded ads for optional hints
- No pay-to-win progression gates


## UI Setup (Option 1 completed)

Build these objects in `Main.unity`:

1. `Canvas` (Screen Space - Overlay)
2. `EventSystem`
3. `Bootstrap` with `GameManager`
4. `UIRoot` with `MainUIBinder`

### Required references for `MainUIBinder`
- `gameManager`: drag `Bootstrap`
- `programBuilder`: object with `ProgramBuilderUI`
- `hud`: object with `HUDController`
- `levelGrid`: object with `LevelGridView`
- `runButton`: Run button
- `clearButton`: Clear button

### Program Builder hierarchy
- `ProgramPanel` (add `ProgramBuilderUI`)
- `ProgramPanel/TokensRoot` (VerticalLayoutGroup)
- `ProgramTokenPrefab` (add `ProgramTokenView`, has `Text` + remove `Button`)
- `InstructionCounterText`

### Drag and Drop hierarchy
- `DropZone` (add `ProgramDropZone`)
- Palette buttons for each opcode (add `PaletteBlockView`)
  - MoveForward
  - TurnLeft
  - TurnRight
  - Loop (argument 2 or 3)
  - IfCoinAhead
  - EndIf

### Grid View hierarchy
- `GridPanel` with `GridLayoutGroup`
- Add `LevelGridView`
- Assign `gridRoot` -> grid transform
- Assign `cellPrefab` -> `Image` prefab (simple square)

### HUD hierarchy
- Text fields: Level, Rank, Coins, Mission, Status
- Add `HUDController` and wire text references

### Mobile UX tips
- Make palette buttons at least 90px touch size
- Put Run/Clear fixed at bottom for thumb access
- Use short status text (1 line)


## Combat + Boss System (Option 2 completed)

New mechanics:
- Hazard tiles damage player HP (3 base HP).
- Patrol enemies move each action tick and damage on collision.
- New instruction: `AttackAhead` (kills adjacent enemy or damages boss core).
- Boss level every 100 levels (`100, 200, ... 15000`).
- Boss objective: defeat core HP, then reach GOAL tile.

UI updates:
- Grid colors now include hazards, enemies, and boss core.
- Status text shows boss objective and HP after runs.

Palette update required in Unity:
- Add a new palette button for `AttackAhead` and set its `PaletteBlockView.opCode` to `AttackAhead`.


## Economy + Season + Shop (Option 3 completed)

Added systems:
- `ShopSystem` + `CosmeticCatalog` for cosmetic-only purchases.
- `SeasonPassSystem` progression on wins (extra gain on perfect/boss clears).
- `RewardSystem` now supports hint tokens plus coin fallback.
- `GameManager` APIs for mission claim, rewarded hint claim, buy/equip skin.

New scripts:
- `Assets/Scripts/Systems/ShopSystem.cs`
- `Assets/Scripts/Systems/CosmeticCatalog.cs`
- `Assets/Scripts/Systems/SeasonPassSystem.cs`
- `Assets/Scripts/UI/MonetizationPanelController.cs`
- `Assets/Scripts/Models/CosmeticItem.cs`
- `Assets/Scripts/Models/EnemyDefinition.cs`

### Scene wiring for monetization panel
Create `MonetizationPanel` and attach `MonetizationPanelController`.
Assign:
- `gameManager` -> `Bootstrap`
- `hud` -> object with `HUDController`
- Buttons:
  - `claimMissionButton`
  - `rewardedHintButton`
  - `buyNeonSkinButton`
  - `equipNeonSkinButton`

### Teen-safe monetization policy in this starter
- Cosmetics are optional and do not block progression.
- Rewarded hints are optional support.
- No pay-to-win gates.

## Final Build Checklist (Complete Game)
1. Build `Main.unity` scene using all references in this README.
2. Add palette blocks for `MoveForward`, `TurnLeft`, `TurnRight`, `AttackAhead`, `Loop`, `IfCoinAhead`, `EndIf`.
3. Tune GridLayout and colors for mobile readability.
4. Add audio, animation, and VFX prefabs.
5. Replace rewarded-hint stub with your ad SDK callback.
6. Configure Player Settings and build Android + iOS.


## Final Production Pass (Polish + Tutorial + Ads)

Added scripts:
- `Assets/Scripts/Tutorial/OnboardingTutorialController.cs`
- `Assets/Scripts/Audio/AudioManager.cs`
- `Assets/Scripts/Ads/IRewardedAdService.cs`
- `Assets/Scripts/Ads/MockRewardedAdService.cs`
- `Assets/Scripts/Ads/RewardedAdsController.cs`
- `Assets/Scripts/UI/ButtonPulse.cs`
- `Assets/Scripts/UI/StatusFlash.cs`

### Tutorial setup
Create `TutorialPanel` with text + Next button and attach `OnboardingTutorialController`.
Assign:
- `gameManager` -> `Bootstrap`
- `tutorialPanel` -> panel object
- `tutorialText` -> text object
- `nextButton` -> next button

Tutorial behavior:
- Active for levels `1-20`
- Progresses by next button and successful runs
- Auto-completes after level 20

### Audio setup
Create `AudioRoot` and attach `AudioManager`.
Assign:
- `gameManager` -> `Bootstrap`
- `musicSource` + `sfxSource`
- clips: `backgroundMusic`, `runSuccess`, `runFail`, `levelLoaded`

### Rewarded ads setup (ready for real SDK wiring)
Create `AdRoot` with:
1. `MockRewardedAdService`
2. `RewardedAdsController`

Set `RewardedAdsController.serviceBehaviour` -> `MockRewardedAdService` component.
Then assign `MonetizationPanelController.rewardedAdsController` -> `AdRoot` controller.

When integrating a real SDK, implement `IRewardedAdService` and replace `MockRewardedAdService`.

### UI polish setup
- Add `ButtonPulse` to Run button and assign in `MainUIBinder.runButtonPulse`.
- Add `StatusFlash` to status text and assign in `MainUIBinder.statusFlash`.

## Handcrafted Tutorial Levels (1-20)

First 20 levels are now handcrafted via:
- `Assets/Scripts/Systems/TutorialLevelFactory.cs`

Behavior:
- Levels 1-20 use fixed layouts and gradual mechanic unlocks.
- Level 21+ uses deterministic procedural generation.


## LiveOps Config (No-Code Balancing)

Game balance is now editable from:
- `Assets/StreamingAssets/liveops_config.json`

Loaded at app startup by:
- `Assets/Scripts/Systems/LiveOpsConfigService.cs`
- `Assets/Scripts/Config/LiveOpsConfig.cs`

Config controls:
- Progression rewards/xp curves
- Daily/mission economy
- Season pass tuning
- Boss frequency and tutorial level count
- Procedural difficulty caps (hazards/enemies/grid size)

After editing JSON, restart Play Mode to apply changes.


## Real Rewarded Ads Adapter

A Unity Ads adapter template is included:
- `Assets/Scripts/Ads/UnityAdsRewardedService.cs`

Usage:
1. Install Unity Ads package and enable `UNITY_ADS` define (if not auto-defined).
2. Add `UnityAdsRewardedService` to `AdRoot`.
3. Set platform game IDs + placement ID.
4. Assign `RewardedAdsController.serviceBehaviour` to `UnityAdsRewardedService`.


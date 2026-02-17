# CodeForge Rush Launch Checklist (Unity)

## 1) Project + Scene
- Open project in Unity 2022.3+ LTS.
- Create scene: `Assets/Scenes/Main.unity`.
- Save scene and add to Build Settings.

## 2) Core Objects
Create GameObject: `Bootstrap`
- Add component: `GameManager`

Create GameObject: `EventSystem`
- Default Unity EventSystem + Standalone/Input System module

Create Canvas: `Canvas` (Screen Space Overlay)
- Add `CanvasScaler` (Scale With Screen Size, 1080x1920)

## 3) UI Root
Create GameObject under Canvas: `UIRoot`
- Add component: `MainUIBinder`

Create panel: `HUDPanel`
- Add component: `HUDController`
- Create Texts and assign:
  - `levelText`
  - `rankText`
  - `coinsText`
  - `missionText`
  - `seasonText`
  - `skinText`
  - `statusText`
- Add `StatusFlash` to `statusText` object

Create panel: `ProgramPanel`
- Add component: `ProgramBuilderUI`
- Child: `TokensRoot` (VerticalLayoutGroup)
- Child: `InstructionCounterText` (Text)
- Create prefab: `ProgramTokenPrefab`
  - Add `ProgramTokenView`
  - Child text -> `label`
  - Child button -> `removeButton`
- Assign prefab/refs in `ProgramBuilderUI`

Create panel: `DropZone`
- Add component: `ProgramDropZone`
- Assign:
  - `programBuilder` -> `ProgramPanel`
  - `hud` -> `HUDPanel`

Create panel: `GridPanel`
- Add `GridLayoutGroup`
- Add `LevelGridView`
- Create simple square image prefab: `CellPrefab`
- Assign in `LevelGridView`:
  - `gridRoot` -> GridPanel RectTransform
  - `cellPrefab` -> CellPrefab
  - `gridLayout` -> GridLayoutGroup

Create buttons:
- `RunButton` (add `ButtonPulse`)
- `ClearButton`

Wire `MainUIBinder`:
- `gameManager` -> `Bootstrap`
- `programBuilder` -> `ProgramPanel`
- `hud` -> `HUDPanel`
- `levelGrid` -> `GridPanel`
- `runButton` -> `RunButton`
- `clearButton` -> `ClearButton`
- `runButtonPulse` -> `RunButton`
- `statusFlash` -> `statusText` object

## 4) Palette Blocks
Create panel: `PalettePanel`
- Add one button/prefab per opcode with `PaletteBlockView`:
  - MoveForward
  - TurnLeft
  - TurnRight
  - AttackAhead
  - Loop (set argument 2 or 3)
  - EndLoop
  - IfCoinAhead
  - EndIf
  - CallFuncA
  - FuncAMarker
  - EndFunc
- Optional drag icon image for each block in `PaletteBlockView.dragIcon`

## 5) Monetization Panel
Create panel: `MonetizationPanel`
- Add component: `MonetizationPanelController`
- Create buttons:
  - `ClaimMissionButton`
  - `RewardedHintButton`
  - `SpendHintButton`
  - `BuyNeonSkinButton`
  - `EquipNeonSkinButton`
- Assign refs:
  - `gameManager` -> `Bootstrap`
  - `hud` -> `HUDPanel`

## 6) Ads Objects
Create GameObject: `AdRoot`
- Add `MockRewardedAdService`
- Add `RewardedAdsController`
- Assign `RewardedAdsController.serviceBehaviour` -> `MockRewardedAdService`

Back to `MonetizationPanelController`:
- Assign `rewardedAdsController` -> `AdRoot`

## 7) Tutorial Objects
Create panel: `TutorialPanel`
- Child text: `TutorialText`
- Child button: `NextButton`
- Add `OnboardingTutorialController`
- Assign:
  - `gameManager` -> `Bootstrap`
  - `tutorialPanel` -> `TutorialPanel`
  - `tutorialText` -> `TutorialText`
  - `nextButton` -> `NextButton`

## 8) Audio Objects
Create GameObject: `AudioRoot`
- Add `AudioManager`
- Add two AudioSources:
  - `MusicSource`
  - `SfxSource`
- Assign in `AudioManager`:
  - `gameManager` -> `Bootstrap`
  - `musicSource` -> MusicSource
  - `sfxSource` -> SfxSource
  - clips: backgroundMusic, runSuccess, runFail, levelLoaded

## 9) Build Settings
- Add `Main.unity` to scenes list.
- Android:
  - IL2CPP
  - ARM64
  - Min API level as needed by store policy
- iOS:
  - IL2CPP
  - Arm64
  - Build to Xcode and set signing

## 10) Smoke Test
- Press Play.
- Verify level loads and grid renders.
- Drag blocks into program and run.
- Verify fail/success status + audio + flash/pulse.
- Verify boss level condition at level 100.
- Verify mission claim and rewarded hint button.
- Verify skin buy/equip updates HUD.
- Edit `Assets/StreamingAssets/liveops_config.json`, restart Play Mode, and confirm updated mission targets/rewards.

## 11) Store-Readiness
- Replace `MockRewardedAdService` with real SDK implementation of `IRewardedAdService`.
- Add privacy policy URL and age-appropriate content labels.
- Add crash analytics and remote config.
- Disable ad `testMode` for production builds.
- Verify Play Store Data safety and App Store Privacy forms against actual SDKs.
- Verify ATT behavior on iOS if tracking/IDFA is used.
- Prepare store listing assets (icon, screenshots, feature graphic, descriptions).
- Validate support email/URL and legal URLs are live and reachable.

## 12) Release Documents
- Complete:
  - `RELEASE/STORE_SUBMISSION_MASTER_CHECKLIST.md`
  - `RELEASE/PLAYSTORE_DATA_SAFETY_TEMPLATE.md`
  - `RELEASE/APPSTORE_PRIVACY_TEMPLATE.md`
  - `RELEASE/STORE_LISTING_TEMPLATE.md`
  - `RELEASE/PRELAUNCH_QA_MATRIX.md`
  - `RELEASE/RELEASE_RUNBOOK.md`

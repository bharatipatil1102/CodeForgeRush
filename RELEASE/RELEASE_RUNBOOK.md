# Release Runbook

Last updated: February 17, 2026

## 1) Prepare Build
1. Set version numbers for Android and iOS.
2. Switch to production ad IDs and disable ad test mode.
3. Validate `Assets/StreamingAssets/liveops_config.json`.
4. Build Android App Bundle (`.aab`) and iOS Xcode project.

## 2) Validate Locally
1. Smoke test all core loops.
2. Verify rewarded hint grant behavior.
3. Verify no critical logs/errors in runtime.
4. Confirm save data survives relaunch.

## 3) Upload to Stores
1. Google Play Console:
   - Upload `.aab`
   - Fill Data safety and content rating
   - Upload listing assets and release notes
2. App Store Connect:
   - Archive/upload build via Xcode
   - Fill privacy labels and age rating
   - Add review notes and test account if needed

## 4) Post-Submission
1. Monitor review feedback and fix rejects quickly.
2. On launch day monitor:
   - Crash-free users
   - Retention and ad reward completion
   - Support inbox
3. Prepare hotfix branch for blocker issues.


# Store Submission Master Checklist

Last updated: February 17, 2026

Use this as the final go/no-go checklist for Google Play and Apple App Store.

## 1) Product Identity
- [ ] Final app name confirmed: `CodeForge Rush`
- [ ] Final bundle/application IDs frozen
- [ ] Versioning strategy set:
  - [ ] Android `versionCode` increments every release
  - [ ] Android `versionName` follows semantic versioning
  - [ ] iOS `CFBundleVersion` increments every build
  - [ ] iOS `CFBundleShortVersionString` matches marketing version

## 2) Build Configuration
- [ ] Unity version locked (2022.3 LTS recommended)
- [ ] IL2CPP enabled for Android and iOS
- [ ] ARM64 enabled for Android and iOS
- [ ] Debug symbols and stripping tuned for release
- [ ] App signing configured:
  - [ ] Android keystore backup secured
  - [ ] iOS signing certificate + provisioning profile valid

## 3) Privacy and Compliance
- [ ] Public privacy policy URL is live and accessible
- [ ] Terms of Use URL is live
- [ ] Google Play Data safety form completed
- [ ] App Store Privacy nutrition labels completed
- [ ] Content rating questionnaire submitted:
  - [ ] Google Play questionnaire
  - [ ] Apple Age Rating
- [ ] Child/teen policy reviewed (ads + data handling)
- [ ] If tracking is used on iOS, ATT flow implemented and tested

## 4) Monetization and Ads
- [ ] Rewarded ad flow works on production ad placement IDs
- [ ] Ad test mode disabled in production
- [ ] No deceptive ad placement (especially near core controls)
- [ ] In-app purchases (if enabled) tested in store sandbox
- [ ] Refund/support policy published

## 5) Store Listing Assets
- [ ] App icon (all required sizes)
- [ ] Feature graphic (Play Store)
- [ ] Screenshots for phone (and tablet if targeting tablets)
- [ ] App preview video (optional)
- [ ] Localized descriptions prepared
- [ ] Support URL and contact email set

## 6) Operational Readiness
- [ ] Crash reporting enabled and verified
- [ ] Support channel operational (email/helpdesk)
- [ ] Privacy request process documented (access/delete requests)
- [ ] Release notes prepared
- [ ] Rollout plan chosen:
  - [ ] Play staged rollout %
  - [ ] App Store phased release enabled/disabled intentionally

## 7) Final QA Gate
- [ ] Cold start, resume, background/foreground tested
- [ ] Ad reward cannot be granted without completed ad
- [ ] Save/load integrity validated across reinstall/update paths
- [ ] No blocker bugs in tutorial, run loop, boss levels, rewards
- [ ] Battery/network usage sanity checked
- [ ] Legal docs and links verified from inside app


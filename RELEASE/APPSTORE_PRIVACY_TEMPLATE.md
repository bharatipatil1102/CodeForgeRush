# App Store Privacy and ATT Template

Last updated: February 17, 2026

Use this template for App Store Connect privacy labels and ATT decisions.

## Privacy Label Draft (verify against final SDK set)
- Contact Info: `Not collected` (unless support/account system added)
- Identifiers: `May collect` (if advertising ID via ads SDK)
- Usage Data: `May collect` (if analytics or gameplay telemetry enabled)
- Diagnostics: `May collect` (if crash reporting enabled)
- Purchases: `Not collected in current code` (set to collected only if IAP backend logs receipts)
- Sensitive info categories: `Not collected`

## Tracking (ATT)
- If ad personalization or cross-app tracking is used:
  - [ ] ATT prompt integrated and tested on iOS 14.5+
  - [ ] `NSUserTrackingUsageDescription` added with clear text
  - [ ] App Privacy tracking section marked accordingly
- If no tracking:
  - [ ] Confirm all SDKs are configured in non-tracking mode
  - [ ] Mark "No tracking" consistently in App Store Connect

## Required Final Verification
- [ ] Check each integrated SDK's App Store privacy guidance
- [ ] Ensure privacy policy text matches App Store privacy answers
- [ ] Validate ATT behavior on real iPhone test device


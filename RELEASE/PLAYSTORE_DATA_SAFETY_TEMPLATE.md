# Google Play Data Safety Template

Last updated: February 17, 2026

Use this template while filling Google Play Console Data safety.
Review with your final SDK list before submission.

## Collected Data (current codebase expectation)
- Personal info: `No` (unless you later add account/login)
- Financial info: `No` (unless real IAP/payment SDK added)
- Location: `No`
- Messages: `No`
- Photos/Files: `No`
- Contacts: `No`
- Device/other IDs: `Potentially Yes` if ads SDK uses advertising ID
- App activity: `Yes` (game progress/events if analytics enabled)
- App info and performance: `Potentially Yes` (crash/perf SDK)

## Data Handling
- Is data collected? `Yes` (if ads/analytics/crash SDK enabled)
- Is data shared with third parties? `Yes` (ads/analytics providers)
- Is data encrypted in transit? `Yes` (SDK transport)
- Can users request deletion? `Yes` (process must be documented)

## Purposes (typical selections)
- App functionality
- Analytics
- Fraud prevention/security
- Advertising or marketing (only if ads SDK active)

## Required Final Verification Before Submit
- [ ] Confirm exact SDKs in project (ads, analytics, crash, attribution)
- [ ] Match each SDK's official data disclosure docs
- [ ] Ensure in-app behavior matches declared usage
- [ ] Ensure Privacy Policy URL exactly describes declared data uses


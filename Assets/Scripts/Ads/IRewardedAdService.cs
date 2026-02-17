using System;

namespace CodeForgeRush.Ads
{
    public interface IRewardedAdService
    {
        bool IsReady { get; }
        void Initialize();
        void ShowRewarded(Action<bool, string> onComplete);
    }
}

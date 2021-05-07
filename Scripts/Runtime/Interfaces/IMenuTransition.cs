using System;
using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public enum MenuTransitionMode : int
    {
        Reverse,
        Forward,
    }

    [Flags]
    public enum MenuTransitionFlags : int
    {
        Everything = -1,
        /// <summary>Resets the transition to the begining at startup.</summary>
        ResetOnInitialize = 1 << 0,
        /// <summary>Resets the transition before playing.</summary>
        ResetOnPlay = 1 << 1,
        /// <summary>Disables the GameObject when transitioned out (good for performance).</summary>
        DisableWhenDone = 1 << 2,
    }

    public interface IMenuTransition 
    {
        void Initialize();

        IPromise Play(in MenuTransitionMode mode = MenuTransitionMode.Forward, in bool instant = false, float? delayOverride = null);
        
        void Complete();
        
        void SetTime(in float time, in MenuTransitionMode mode = MenuTransitionMode.Forward);

        float CurrentTime { get; }

        bool Instant { get; }

        MenuTransitionMode Mode { get; }

        AnimationCurve Curve { get; }

        bool IsPlaying { get; }

        float Duration { get; set; }

        float Delay { get; set; }

        float TotalDuration { get; }

        MenuTransitionFlags Flags { get; set; }
    }
}

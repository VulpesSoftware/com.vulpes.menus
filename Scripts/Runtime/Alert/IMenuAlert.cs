using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public interface IMenuAlert
    {
        void Initialize();

        IPromise Show(string message, Sprite icon, float duration);

        IPromise Show(string message, Sprite icon, IPromise promiseToWaitFor);

        void UpdatePromiseTimer(in float deltaTime);
    }
}
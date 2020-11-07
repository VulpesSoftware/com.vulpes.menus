using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public interface IMenuAlert 
    {
        IPromise Show(string asMessage, Sprite akIcon, float afDuration);

        IPromise Show(string asMessage, Sprite akIcon, IPromise akPromise);

        void UpdateTimer(float afDeltaTime);
    }
}

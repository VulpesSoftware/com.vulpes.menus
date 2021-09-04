using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public interface IMenuAlert 
    {
        IPromise Show(string message, Sprite icon, float duration);

        IPromise Show(string message, Sprite icon, IPromise onResolved);

        void UpdateTimer(in float deltaTime);
    }
}

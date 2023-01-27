using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public interface IMenuTooltip
    {
        IPromise Show(in string title, in string body);

        IPromise Hide();

        void SetPosition(Vector2 position);

        void PivotLeft();

        void PivotRight();
    }
}
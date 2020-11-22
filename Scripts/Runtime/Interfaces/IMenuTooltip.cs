using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public interface IMenuTooltip
    {
        bool IsActive { get; }

        IPromise Show(string title, string body);

        IPromise Hide();

        void SetPosition(Vector2 position);

        void PivotLeft();

        void PivotRight();
    }
}
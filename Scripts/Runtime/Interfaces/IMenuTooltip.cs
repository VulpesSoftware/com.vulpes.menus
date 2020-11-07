using UnityEngine;
using Vulpes.Promises;

namespace Vulpes.Menus
{
    public interface IMenuTooltip
    {
        IPromise Show(string title, string body);

        IPromise Hide();
    }
}
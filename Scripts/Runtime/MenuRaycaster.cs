using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vulpes.Menus
{
    [AddComponentMenu("Vulpes/Menus/Menu Raycaster"), RequireComponent(typeof(MenuHandler)), DisallowMultipleComponent]
    public sealed class MenuRaycaster : UIBehaviour
    {
        /// <summary>
        /// Performs a Raycast against the UI using the current <see cref="EventSystem"/> and returns a list of <see cref="RaycastResult"/>s.
        /// </summary>
        public List<RaycastResult> Raycast(Vector2 position)
        {
            PointerEventData pointerEventData = new(EventSystem.current)
            {
                position = position
            };
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerEventData, results);
            return results;
        }
    }
}
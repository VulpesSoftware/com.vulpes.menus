using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vulpes.Menus.Experimental
{
    [RequireComponent(typeof(Graphic))]
    public class MenuColorProfileListener : MonoBehaviour
    {
        [SerializeField] private int colorIndex = -1;

        private Color defaultColor;
    }
}
using UnityEngine;

namespace Vulpes.Menus.Examples
{
    [AddComponentMenu("Vulpes/Menus/Examples/Game/Tasty Cube")]
    public sealed class TastyCube : MonoBehaviour
    {
        public void Consume()
            => Destroy(gameObject);
    }
}
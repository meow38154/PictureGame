using UnityEngine;

namespace MapSystem
{
    public class MapPart : MonoBehaviour, IMapPart
    {
        [field: SerializeField] public Collider2D Collider { get; private set; }
        public Transform Transform => transform;
    }
}
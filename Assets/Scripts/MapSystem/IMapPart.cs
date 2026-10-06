using UnityEngine;

namespace MapSystem
{
    public interface IMapPart
    {
        Transform Transform { get; }
        Collider2D Collider { get; }
    }
}
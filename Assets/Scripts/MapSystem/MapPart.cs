using UnityEngine;

namespace MapSystem
{
    public class MapPart : MonoBehaviour
    {
        [field: SerializeField] public MapPieceSo MapPiece { get; private set; }
    }
}
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace MapSystem
{
    public class DisposableMapPiece : AbstractMapPiece, IDisposable
    {
        [SerializeField] private UnityEvent onDispose;
        
        public void Dispose() 
            => onDispose?.Invoke();
    }
}
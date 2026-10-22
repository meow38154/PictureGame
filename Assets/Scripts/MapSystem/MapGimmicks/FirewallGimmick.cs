using DevLib.EventChannelSystem;
using UnityEngine;

namespace MapSystem.MapGimmicks
{
    public class FirewallGimmick : AbstractMapPiece, IToggleable
    {
        [SerializeField] private EventChannelSO mapGridEvent; //받는거
        
        public bool Current { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            mapGridEvent.AddListener<>();
        }

        public void Toggle()
        {
            
        }
    }
}
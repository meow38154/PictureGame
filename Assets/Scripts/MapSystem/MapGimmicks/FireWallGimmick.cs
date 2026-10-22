using System;
using UnityEngine;

namespace MapSystem.MapGimmicks
{
    public class FireWallGimmick : MonoBehaviour, IToggleable
    {
        [SerializeField] private GameObject fireWall;
        public bool IsOn { get; private set; }
        
        private FirewallMapPiece _firewallMapPiece;

        private void Awake()
        {
            _firewallMapPiece = fireWall.GetComponent<FirewallMapPiece>();
        }

        public void OnOffToggle()
        {
            
        }
    }
}
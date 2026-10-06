using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace CoreSystem.Manager
{
    [DefaultExecutionOrder(-100)]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private EventChannelSO playerTrmRegistered;
        
        private void Start()
        {
            playerTrmRegistered.RaiseEvent(CreateEvents.PlayerTransformRegistered.InitData(playerTransform));
        }
    }
}
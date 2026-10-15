using Common.Attributes;
using CoreSystem.Events;
using CoreSystem.GrabSystem;
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class GrabController : Module
    {
        [ReadOnly, SerializeField] private GameObject currentGrabbingObject;
        [SerializeField] private EventChannelSO grabEventChannelSo;
        [SerializeField] private Vector2 grabOffset;
        
        private IGrabbable grabbedObject;
        
        private void OnEnable()
        {
            grabEventChannelSo.AddListener<GrabbableObjectEvent>(HandleGrabbableObject);
        }

        private void OnDisable()
        {
            grabEventChannelSo.RemoveListener<GrabbableObjectEvent>(HandleGrabbableObject);
        }

        private void HandleGrabbableObject(GrabbableObjectEvent obj)
        {
            grabbedObject = obj.Value;
            grabbedObject.Transform.position = (Vector2)_owner.transform.position + grabOffset;
            grabbedObject.Transform.SetParent(_owner.transform, true);
            
            currentGrabbingObject = obj.Value.Transform.gameObject;
        }
    }
}
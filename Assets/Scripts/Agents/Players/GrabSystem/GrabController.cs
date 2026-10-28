using Common.Attributes;
using CoreSystem.Events;
using CoreSystem.GrabSystem;
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class GrabController : Module, IGrabController
    {
        [field: SerializeField, ReadOnly] public GameObject CurrentGrabbingObject { get; set; }
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
            
            CurrentGrabbingObject = obj.Value.Transform.gameObject;
        }
    }
}
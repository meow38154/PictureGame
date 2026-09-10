using System;
using CoreSystem.GrabSystem;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class PlayerGrabController : MonoBehaviour
    {
        [SerializeField] private PlayerInputSo playerInput;
        [SerializeField] private Transform handTrm;
        
        private IGrabFinder _targetGrabFinder;
        private IPlayerGrabHandler _playerGrabHandler;
        private IGrabMover _grabMover;
        
        private void Awake()
        {
            _targetGrabFinder = GetComponentInChildren<IGrabFinder>();
            Debug.Assert(_targetGrabFinder != null, "IGrabFinder object not found");            
            _playerGrabHandler = GetComponentInChildren<IPlayerGrabHandler>();
            Debug.Assert(_playerGrabHandler != null, "IGrabHandler object not found");            
            _grabMover = GetComponentInChildren<IGrabMover>();
            Debug.Assert(_grabMover != null, "IGrabMover object not found");

            playerInput.OnMouseLeftButtonPressed += HandleCatchGrabbable;
            playerInput.OnMouseLeftButtonReleased += HandlePutGrabbable;
        }
        
        private void HandleCatchGrabbable()
        {
            if (!_targetGrabFinder.TryFindObject(out IGrabbable grabbable)) return;
            if (grabbable == null || _playerGrabHandler.IsGrabbing) return;
            
            _playerGrabHandler.Grab(grabbable, handTrm);
            _grabMover.MoveGrabbable(grabbable, grabbable.Pivot.position);
            Debug.Log(grabbable.Pivot.position);
        }

        private void HandlePutGrabbable()
        {
            if (_playerGrabHandler == null) return;
            _playerGrabHandler.Release();
        }
        
        private void OnDestroy()
        {
            playerInput.OnMouseLeftButtonPressed -= HandleCatchGrabbable;
            playerInput.OnMouseLeftButtonReleased -= HandlePutGrabbable;
        }
    }
}
using Command;
using CoreSystem.Effect;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UI.Force_Undo
{
    [RequireComponent(typeof(Animator))]
    public class ForceUndoSystem : MonoBehaviour
    {
        [SerializeField] private EventChannelSO forceUndoEvent;
        [SerializeField] private EventChannelSO commandInvokeEvent;
        [SerializeField] private EventChannelSO playerFreezeControllerEvent;
        [SerializeField] private AssetNameSO animationName;

        [Header("ChromaticAberration Show")] 
        [SerializeField] private float showAnimChromaticTargetValue = 1;
        [SerializeField] private float showAnimBloomTargetValue = 1;
        [SerializeField] private float showAnimDuration = 0.2f;
        
        [Header("ChromaticAberration Hide")]
        [SerializeField] private float hideAnimChromaticTargetValue = 0;
        [SerializeField] private float hideAnimBloomTargetValue = 0;
        [SerializeField] private float hideAnimDuration = 0.2f;

        
        private bool _isPlaying;
        
        private Animator _animator;
        private Camera _camera;
        private Volume _volume;
        private ChromaticAberration _chromaticAberration;
        private Bloom _bloom;

        private void Awake()
        {
            _camera = Camera.main;
            _animator = GetComponent<Animator>();
            if (_camera != null) _volume = _camera.GetComponent<Volume>();
            _volume.profile.TryGet(out _chromaticAberration);
            _volume.profile.TryGet(out _bloom);
        }

        private void OnEnable()
        {
            forceUndoEvent.AddListener<BoolEvent>(HandlePlayForceUndoAnim);
        }

        private void OnDisable()
        {
            forceUndoEvent.RemoveListener<BoolEvent>(HandlePlayForceUndoAnim);
        }

        private void HandlePlayForceUndoAnim(BoolEvent obj)
        {
            if (!obj.Value || _isPlaying) return;
            
            _animator.Play(animationName.AssetHash,0,0);
            _isPlaying = true;
        }

        public void ShowPlay()
        {
            playerFreezeControllerEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(false));
            ChromaticAberrationTween(showAnimChromaticTargetValue, showAnimDuration);
            BloomAberrationTween(showAnimBloomTargetValue, showAnimDuration);
        }

        public void HidePlay()
        {
            playerFreezeControllerEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));
            ChromaticAberrationTween(hideAnimChromaticTargetValue, hideAnimDuration);
            BloomAberrationTween(hideAnimBloomTargetValue, hideAnimDuration);
        }

        private void ChromaticAberrationTween(float value, float duration)
        {
            if (_chromaticAberration == null) return;
            
            _chromaticAberration.intensity.overrideState = true;
            DOTween.To(() => _chromaticAberration.intensity.value, x => _chromaticAberration.intensity.value = x, value, duration).SetEase(Ease.OutQuad);
        }        
        
        private void BloomAberrationTween(float value, float duration)
        {
            if (_bloom == null) return;
            
            _bloom.intensity.overrideState = true;
            DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, value, duration).SetEase(Ease.OutQuad);
        }
        
        public void RaiseCommand()
        {
            CommandInvokeEventStruct eventData = new()
            {
                Action = CommandActionType.Undo,
                Command = null
            };

            _isPlaying = false;
            
            commandInvokeEvent.RaiseEvent(CreateEvents.CommandInvokeEvent.InitData(eventData));
        }
    }
}
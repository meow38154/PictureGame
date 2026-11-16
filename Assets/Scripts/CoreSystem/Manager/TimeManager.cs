using System;
using System.Diagnostics.Tracing;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace CoreSystem.Manager
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO timeScaleChangeEvent;
        [SerializeField] private float currentTimeScale = 1f;

        private void Start()
        {
            UpdateTimeScale();
        }

        private void OnEnable()
        {
            timeScaleChangeEvent.AddListener<FloatEvent>(HandleChangeTimeScale);
        }

        private void OnDisable()
        {
            timeScaleChangeEvent.RemoveListener<FloatEvent>(HandleChangeTimeScale);
        }

        private void HandleChangeTimeScale(FloatEvent eventData)
        {
            currentTimeScale = eventData.Value;
            UpdateTimeScale();
        }

        private void UpdateTimeScale()
        {
            Time.timeScale = currentTimeScale;
        }
    }
}
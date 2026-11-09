using System;
using TMPro;
using UnityEngine;

namespace UI.Wall_Paper.Taskbar
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    
    public class TimeRenderer : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private float _timer;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            DateTime currentTime = DateTime.Now;
            _text.text = currentTime.ToString("HH:mm:ss");
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > 1f)
            {
                DateTime currentTime = DateTime.Now;
                _text.text = currentTime.ToString("HH:mm:ss");
                _timer = 0f;
            }
        }
    }
}
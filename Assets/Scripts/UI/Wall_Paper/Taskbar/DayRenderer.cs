using System;
using TMPro;
using UnityEngine;

namespace UI.Wall_Paper.Taskbar
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    
    public class DayRenderer : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private float _timer;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > 1f)
            {
                _text.text = DateTime.Now.ToString("yyyy-MM-dd");
                _timer = 0f;
            }
        }
    }
}
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Windows
{
    public class ScreenCaptureEvent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private EventChannelSO reflectScreenCaptureEvent;

        private AbstractWindows _windows;
        private float _closeTime;
        private bool _isClosing;

        private void Awake()
        {
            _windows = GetComponent<AbstractWindows>();
            reflectScreenCaptureEvent.AddListener<BoolEvent>(HandleImageChangeEvent);
        }

        private void Update()
        {
            if (!_isClosing || Time.time < _closeTime) return;

            _isClosing = false;
            Debug.Log(" closed");
            _windows.WindowClose();
        }

        private async void HandleImageChangeEvent(BoolEvent obj)
        {
            if (!obj.Value) return;

            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            ImageChange(await Utility.GetCurrentSceneScreenAsync(rect));

            Debug.Log("dk");

            _closeTime = Time.time + 0.5f;
            _isClosing = true;
        }

        private void ImageChange(Texture2D texture)
        {
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        private void OnDestroy()
        {
            reflectScreenCaptureEvent.RemoveListener<BoolEvent>(HandleImageChangeEvent);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Windows
{
    public class StageWindows : AbstractWindows
    {
        [SerializeField] private Image screenImage;

        private Texture2D _capturedTexture;
        private Sprite _capturedSprite;

        public async void ScreenImageChanged(string sceneName)
        {
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            
            OnScreenCaptured(await Utility.GetSceneFirstScreenAsync(sceneName, rect));
        }

        private void OnScreenCaptured(Texture2D texture)
        {
            if (texture == null) return;

            if (this == null || screenImage == null)
            {
                Destroy(texture);
                return;
            }

            ReleaseCapturedImage();

            _capturedTexture = texture;

            _capturedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f) );

            screenImage.sprite = _capturedSprite;
        }

        private void ReleaseCapturedImage()
        {
            if (screenImage != null)
            {
                screenImage.sprite = null;
            }

            if (_capturedSprite != null)
            {
                Destroy(_capturedSprite);
                _capturedSprite = null;
            }

            if (_capturedTexture != null)
            {
                Destroy(_capturedTexture);
                _capturedTexture = null;
            }
        }

        private void OnDestroy()
        {
            ReleaseCapturedImage();
        }
    }
}
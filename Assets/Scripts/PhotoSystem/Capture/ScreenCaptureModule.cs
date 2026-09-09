using System;
using System.Collections;
using DevLib.ModuleSystem;
using UnityEngine;

namespace PhotoSystem.Capture
{
    public class ScreenCaptureModule : Module, IScreenCapture
    {
        [SerializeField] private LayerMask captureLayerMask;
        
        public IEnumerator Capture(Rect screenRect, Action<Texture2D> onCaptured)
        {
            yield return new WaitForEndOfFrame();

            int width = Mathf.RoundToInt(screenRect.width);
            int height = Mathf.RoundToInt(screenRect.height);

            
            if (width <= 0 || height <= 0)
                yield break;

            Texture2D texture = new(width, height, TextureFormat.RGBA32, false);

            texture.ReadPixels(screenRect, 0, 0);
            texture.Apply();

            onCaptured?.Invoke(texture);
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace PhotoSystem.Capture
{
    public interface IScreenCapture
    {
        IEnumerator Capture(Rect screenRect, Action<Texture2D> onCaptured);
    }
}
using UnityEngine;

namespace PhotoSystem.Capture
{
    public interface ICaptureSelection
    {
        bool IsSelecting { get; }
        void BeginSelection(Vector2 screenPosition);
        void UpdateSelection(Vector2 screenPosition);
        Rect EndSelection(Vector2 screenPosition);
    }
}
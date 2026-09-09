using DevLib.ModuleSystem;
using UnityEngine;

namespace PhotoSystem.Capture
{
public class CaptureSelectionModule : Module, ICaptureSelection
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform captureAreaRectTrm;

    private bool _isSelecting;

    private Vector2 _firstCanvasPos;
    private Vector2 _secondCanvasPos;

    private Vector2 _firstScreenPos;
    private Vector2 _secondScreenPos;

    public bool IsSelecting => _isSelecting;

    public void BeginSelection(Vector2 screenPosition)
    {
        _isSelecting = true;

        _firstScreenPos = screenPosition;
        _firstCanvasPos = ScreenToCanvasPosition(screenPosition);

        captureAreaRectTrm.gameObject.SetActive(true);
        captureAreaRectTrm.anchoredPosition = _firstCanvasPos;
        captureAreaRectTrm.sizeDelta = Vector2.zero;
    }

    public void UpdateSelection(Vector2 screenPosition)
    {
        if (!_isSelecting)
            return;

        _secondScreenPos = screenPosition;
        _secondCanvasPos = ScreenToCanvasPosition(screenPosition);

        UpdateSelectionRect();
    }

    public Rect EndSelection(Vector2 screenPosition)
    {
        _isSelecting = false;

        _secondScreenPos = screenPosition;
        captureAreaRectTrm.gameObject.SetActive(false);

        Vector2 min = Vector2.Min(_firstScreenPos, _secondScreenPos);
        Vector2 max = Vector2.Max(_firstScreenPos, _secondScreenPos);

        return new Rect(min, max - min);
    }

    private void UpdateSelectionRect()
    {
        Vector2 min = Vector2.Min(_firstCanvasPos, _secondCanvasPos);
        Vector2 max = Vector2.Max(_firstCanvasPos, _secondCanvasPos);

        captureAreaRectTrm.anchoredPosition = (min + max) * 0.5f;
        captureAreaRectTrm.sizeDelta = max - min;
    }

    private Vector2 ScreenToCanvasPosition(Vector2 screenPosition)
    {
        RectTransform canvasRect = (RectTransform)canvas.transform;

        Camera uiCamera =
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            uiCamera,
            out Vector2 localPoint);

        return localPoint;
    }
}
}

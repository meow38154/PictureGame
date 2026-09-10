using UnityEngine;
using UnityEngine.InputSystem;

public static class Utility
{
    private static Camera _cam;

    public static void Init()
    {
        _cam = Camera.main;
    }

    public static Vector2 GetMouseWorldPosition()
    {
        if (_cam == null || Mouse.current == null)
            return Vector2.zero;

        Vector2 mousePos = GetMouseScreenPosition();

        float distanceFromCamera = -_cam.transform.position.z;

        Vector3 worldPos = _cam.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, distanceFromCamera)
        );

        return worldPos;
    }

    public static Vector2 GetMouseScreenPosition()
    {
        return Mouse.current == null ? Vector2.zero : Mouse.current.position.ReadValue();
    }
}
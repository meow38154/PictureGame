using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Debugs
{
    public class UIRaycastDebugger : MonoBehaviour
    {
        private readonly List<RaycastResult> _results = new();

        private void Update()
        {
            if (Mouse.current == null ||
                !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            if (EventSystem.current == null)
            {
                UnityEngine.Debug.LogError("EventSystem이 없음");
                return;
            }

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            _results.Clear();
            EventSystem.current.RaycastAll(eventData, _results);

            if (_results.Count == 0)
            {
                UnityEngine.Debug.LogWarning("감지된 UI 없음");
                return;
            }

            for (int i = 0; i < _results.Count; i++)
            {
                UnityEngine.Debug.Log(
                    $"{i}: {_results[i].gameObject.name} " +
                    $"({_results[i].gameObject.transform.GetHierarchyPath()})"
                );
            }
        }
    }

    public static class TransformExtensions
    {
        public static string GetHierarchyPath(this Transform transform)
        {
            string path = transform.name;

            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }

            return path;
        }
    }
}
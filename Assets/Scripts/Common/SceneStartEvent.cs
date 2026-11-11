using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class SceneStartEvent : MonoBehaviour
    {
        [SerializeField] public UnityEvent onStartEvent;

        private void Start()
        {
            onStartEvent?.Invoke();
        }
    }
}
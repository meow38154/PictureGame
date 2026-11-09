using UnityEngine;
using UnityEngine.Events;

namespace UI.Menu
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnInteract;

        public void Interact()
        {
            OnInteract?.Invoke();
        }
    }
}
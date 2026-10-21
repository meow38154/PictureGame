using System.Linq;
using Condition;
using CoreSystem.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI.Interactions
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] private ConditionEntry[] conditions;
        [SerializeField] public UnityEvent onKeyDownSuccess;
        [SerializeField] public UnityEvent onKeyDownFail;
        
        private ColliderTrigger colliderTrigger;

        private void Awake()
        {
            colliderTrigger = GetComponentInChildren<ColliderTrigger>();
            Debug.Assert(colliderTrigger != null, "ColliderTrigger is missing");
        }

        private void Update()
        {
            if (!colliderTrigger.IsSearch || !Keyboard.current.eKey.wasPressedThisFrame) return;
            if (CheckCondition()) onKeyDownSuccess?.Invoke();
            else onKeyDownFail?.Invoke();
        }

        private bool CheckCondition()
        {
            return conditions.Length == 0 || conditions.All(i => i.condition.Evaluate(i.target));
        }
    }
}
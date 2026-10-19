using UnityEngine;

namespace Condition
{
    public abstract class ConditionSo : ScriptableObject
    {
        public abstract bool Evaluate(GameObject target);
    }
}
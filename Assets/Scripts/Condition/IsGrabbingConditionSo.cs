using CoreSystem.GrabSystem;
using UnityEngine;

namespace Condition
{
    [CreateAssetMenu(fileName = "base grabbing condition", menuName = "Conditions/Is Grabbing", order = 0)]
    public class IsGrabbingConditionSo : ConditionSo
    {
        public override bool Evaluate(GameObject target)
        {
            return target.TryGetComponent(out IGrabbable grabbable) && grabbable.IsGrabbing;
        }
    }
}
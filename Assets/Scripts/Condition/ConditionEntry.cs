using System;
using UnityEngine;

namespace Condition
{
    [Serializable]
    
    public struct ConditionEntry
    {
        public ConditionSo condition;
        public GameObject target;
    }
}
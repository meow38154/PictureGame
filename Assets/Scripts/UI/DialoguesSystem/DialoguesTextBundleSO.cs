using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI.DialoguesSystem
{
    [Serializable]
    public struct DialoguesDataStruct
    {
        [TextArea(10, 100)] public string text;
        
        public float textDuration;
        public float actionInvokeTime;
        public Action EventDuringTextRendering;
    }
    
    [CreateAssetMenu(fileName = "base text bundle", menuName = "Dialogues/Text Bundle", order = 0)]
    public class DialoguesTextBundleSO : ScriptableObject
    {
        [field: SerializeField] public DialoguesDataStruct[] DialoguesDataBundle { get; private set; }
    }
}
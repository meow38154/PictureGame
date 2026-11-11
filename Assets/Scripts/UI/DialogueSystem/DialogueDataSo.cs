using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.DialogueSystem
{
    [Serializable]
    public struct DialogueTextData
    {
        [Range(0.2f, 4f)] public float TextSpeed;
        [Range(0.7f, 2f)] public float TextSize;
        public string Text;
    }

    [Serializable]
    public struct DialogueLineData
    {
        public DialogueTextData[] DialogueTextData;
        public Sprite SpeakerImage;
    }
    
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue", order = 0)]
    public class DialogueDataSo : ScriptableObject
    {
        public List<DialogueLineData> LineDatas = new List<DialogueLineData>();
    }
}
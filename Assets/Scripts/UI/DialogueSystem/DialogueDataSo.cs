using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.DialogueSystem
{
    [Serializable]
    public struct DialogueTextData
    {
        [Range(0.2f, 4f)] public float textSpeed;
        [Range(0.7f, 2f)] public float textSize;
        public string text;
    }

    [Serializable, ]
    public struct DialogueLineData
    {
        public List<DialogueTextData> dialogueTextData;
        public Sprite speakerImage;

        public string convertedString;
    }
    
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue", order = 0)]
    public class DialogueDataSo : ScriptableObject
    {
        public List<DialogueLineData> LineDatas = new List<DialogueLineData>();

        [SerializeField] private bool isOnValidate = false;
        public string convertedString;

        private void OnValidate()
        {
            if (LineDatas.Count == 0 || !isOnValidate)
                return;

            string tmp = "";
            
            for (int i = 0; i < LineDatas.Count; i++)
            {
                if (LineDatas[i].dialogueTextData.Count == 0) 
                    continue;
                
                tmp = "{";
                
                foreach (DialogueTextData text in LineDatas[i].dialogueTextData)
                    tmp += $"({text.textSpeed})({text.textSize*100}){text.text}";
                
                tmp += "}";
                
                var lineData = LineDatas[i];
                lineData.convertedString = tmp;
                LineDatas[i] = lineData;
            }

            tmp = "";

            foreach (DialogueLineData lineData in LineDatas)
            {
                if (lineData.dialogueTextData.Count == 0) 
                    continue;
                
                tmp += lineData.convertedString;
            }

            convertedString = tmp;
        }

        [ContextMenu("MakeToData")]
        public void MakeToData()
        {
            if (convertedString.Length == 0)
                return;
            
            var lines = convertedString.Split(new[] {'{', '}'}, StringSplitOptions.RemoveEmptyEntries);
            
            LineDatas.Clear();

            foreach (var line in lines)
            {
                DialogueLineData lineData = new DialogueLineData();
                lineData.dialogueTextData = new List<DialogueTextData>();
                var texts = line.Split(new[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries);
                
                for (int i = 0; i < texts.Length/3; i++)
                {
                     DialogueTextData textData = new DialogueTextData();
                     Debug.Log(texts[(i*3)]);
                     textData.textSpeed = float.Parse(texts[(i*3)]);
                     textData.textSize = float.Parse(texts[(i*3)+1]);
                     textData.text = texts[(i*3)+2];
                     
                     lineData.dialogueTextData.Add(textData);
                }
                
                LineDatas.Add(lineData);
            }
        }
    }
}
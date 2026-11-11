using UnityEngine;

namespace UI.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [field: SerializeField] static public DialogueManager Instance { get; private set; }

        [SerializeField] private DialogueUI dialogueUI;
        

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void Dialogue(DialogueDataSo dialogueData)
        {
            if (dialogueData != null) 
                return;
            
            dialogueUI.ShowDialogue(dialogueData);
        }

        private void OnDestroy()
        {
            if(Instance == this)
                Instance = null;
        }
    }
}
using TMPro;
using UnityEngine;

namespace Agents.Mining
{
    public class TmpTextChanger : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void ChangeText(string value)
        {
            text.text = value;
        }
    }
}
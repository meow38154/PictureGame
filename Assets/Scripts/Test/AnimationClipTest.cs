using System;
using Agents;
using CoreSystem.Effect;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    [Serializable]
    public struct ClipPlayButton
    {
        public Key key;
        public AssetNameSO assetNameSo;
    }
    
    public class AnimationClipTest : MonoBehaviour
    {
        [SerializeField] private AgentRenderer agentRenderer;

        [SerializeField] private ClipPlayButton[] keys;
        
        private void Update()
        {
            foreach (var key in keys)
            {
                if (Keyboard.current[key.key].wasPressedThisFrame)
                {
                    agentRenderer.PlayClip(key.assetNameSo.AssetHash, 0f, 0.2f);
                }
            }
        }
    }
}
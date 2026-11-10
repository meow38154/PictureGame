using System;
using CoreSystem.Manager;
using Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Test
{
    public class TestSceneChange : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                //SceneChangeManager.Instance.ChangeScene("Stage_02");
            }
        }
    }
}
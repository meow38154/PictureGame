using System;
using UnityEngine;

namespace WindowsVolumeCheckSystem
{
    public class WindowsVolume : MonoBehaviour
    {
        [SerializeField] private float currentVolume;

        private void Update()
        {
            //currentVolume = WindowsAudio.GetMasterVolume();
        }
    }
}
using System;
using UnityEngine;

namespace UI.Wall_Paper
{
    public class WallPaperManager : MonoBehaviour
    {
        public static WallPaperManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    }
}
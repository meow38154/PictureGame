using System;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class PlayerAimController : MonoBehaviour
    {
        private void Awake()
        {
            Utility.Init();
        }

        private void Update()
        {
            Vector2 dir = Utility.GetMouseWorldPosition() - (Vector2)transform.position;
            transform.right  = dir.normalized;
        }
    }
}
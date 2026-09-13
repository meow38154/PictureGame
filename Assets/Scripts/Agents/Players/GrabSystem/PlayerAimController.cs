using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class PlayerAimController : MonoBehaviour
    {
        private Vector3 _defaultScale;

        private void Awake()
        {
            Utility.Init();
            _defaultScale = transform.localScale;
        }

        private void Update()
        {
            Vector2 dir = Utility.GetMouseWorldPosition() - (Vector2)transform.position;

            if (dir.sqrMagnitude <= Mathf.Epsilon) return;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            transform.localScale = new Vector3(_defaultScale.x, dir.x < 0f ? -_defaultScale.y : _defaultScale.y, _defaultScale.z
            );
        }
    }
}
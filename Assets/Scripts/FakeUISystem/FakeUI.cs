using System;
using CoreSystem.Triggers;
using UnityEngine;

//다음주 발표라서 코드 좀 이상하게 써둘게요ㅠㅠ

namespace FakeUISystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FakeUI : MonoBehaviour
    {
        [SerializeField] private UiTrigger trigger;
        [SerializeField] private GameObject[] enableTargetGameObjects;
        [SerializeField] private GameObject[] disableTargetGameObjects;
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();     
            _collider2D = GetComponent<Collider2D>();
        }

        public void EnableFakeUI()
        {
            if (!trigger.IsStay) return;
            
            _collider2D.enabled = true;
            foreach (var go in enableTargetGameObjects)
            {
                go.SetActive(true);
            }            
            
            foreach (var go in disableTargetGameObjects)
            {
                go.SetActive(false);
            }
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            transform.SetParent(null);
        }
    }
}
using System;
using CoreSystem.Triggers;
using UnityEngine;
using UnityEngine.Events;

//다음주 발표라서 코드 좀 이상하게 써둘게요ㅠㅠ

namespace FakeUISystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FakeUI : MonoBehaviour
    {
        [SerializeField] private UiTrigger trigger;
        [SerializeField] private GameObject[] enableTargetGameObjects;
        [SerializeField] private GameObject[] disableTargetGameObjects;
        [SerializeField] public UnityEvent endUnityEvent;
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;

        [SerializeField] private int health = 3;

        private bool _endPlay;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();     
            _collider2D = GetComponent<Collider2D>();
        }

        
        
        public void EnableFakeUI()
        {
            if (health > 1)
            {
                health--;
                return;
            }
            
            if (_endPlay) return;
            
            if (!trigger.IsStay) return;

            _endPlay = true;
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
            endUnityEvent?.Invoke();
        }
    }
}
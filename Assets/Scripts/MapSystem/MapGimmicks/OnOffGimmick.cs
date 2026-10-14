using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapSystem.MapGimmicks
{
    public class OnOffGimmick : MonoBehaviour, IToggleable
    {
        [SerializeField] private List<GameObject> items;

        private List<GameObject> _onItems = new();
        private List<GameObject> _offItems = new();
            
        public bool Current { get; private set; }

        private void Awake()
        {
            foreach (GameObject item in items)
            {
                if(item)
                    _onItems.Add(item);
                else if(item)
                    _offItems.Add(item);

                Current = false;
            }
        }

        public void Toggle()
        {
            //대기
        }
    }
}
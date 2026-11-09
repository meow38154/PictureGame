using System;
using System.Collections.Generic;
using Common.Attributes;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Windows.Manager
{
    public class WindowController : MonoBehaviour
    {
        [ReadOnly, SerializeField] private List<AbstractWindows> windowsList;
        [SerializeField] private RectTransform taskbarTrm;

        [SerializeField] private GameObject pathButton;
        private readonly Dictionary<Type, AbstractWindowManager> abstractWindowDict = new();
        
        [SerializeField] private EventChannelSO windowOpenRegisterEvent;
        [SerializeField] private EventChannelSO windowCloseRegisterEvent;

        private void Awake()
        {
            var managers = GetComponentsInChildren<AbstractWindowManager>();

            foreach (var m in managers)
            {
                abstractWindowDict.Add(m.FileSo.GetType(), m);
            }
        }

        private void OnEnable()
        {
            windowOpenRegisterEvent.AddListener<AbstractFileSOEvent>(HandleWindowOpenRegister);
            windowCloseRegisterEvent.AddListener<AbstractWindowEvent>(HandleWindowCloseRegister);
        }


        private void HandleWindowOpenRegister(AbstractFileSOEvent obj)
        {
            AbstractWindows win = abstractWindowDict[obj.Value.GetType()].OpenWindow(obj.Value);
            Debug.Log(obj.Value.GetType().Name);
            windowsList.Add(win);
            
            GameObject taskbarIcon = new GameObject("TaskbarIcon");
            Image imageCompo = taskbarIcon.AddComponent<Image>();
            imageCompo.sprite = obj.Value.Icon;
            taskbarIcon.transform.SetParent(taskbarTrm);
        }

        private void HandleWindowCloseRegister(AbstractWindowEvent obj)
        {
            windowsList.Remove(obj.Value);
            obj.Value.WindowClose();
        }

        private void OnDisable()
        {
            windowOpenRegisterEvent.RemoveListener<AbstractFileSOEvent>(HandleWindowOpenRegister);
            windowCloseRegisterEvent.RemoveListener<AbstractWindowEvent>(HandleWindowCloseRegister);
        }
    }
}
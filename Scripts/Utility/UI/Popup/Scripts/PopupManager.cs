using Banana.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banana.UI.Popup
{
    public class PopupManager : UnitySingleton<PopupManager>
    {
        [SerializeField] private RectTransform popupRoot;
        [SerializeField] private bool ignoreWhileTransition;
        [SerializeField] private bool isOnlyOnePopup;

        public Dictionary<string, PopupBase> PopupDic { get; private set; }
        public Stack<string> showingPopupStack { get; private set; }
        public Queue<string> waitingPopupQueue { get; private set; }
        public bool IsTransition { get; private set; }

        private ObjectPool<Dimming> _dimmingPool;
        private Stack<Dimming> _dimmingStack;

        private Coroutine _showingRoutine;
        private Coroutine _hidingRoutine;

        // ============ Editor ============
        public string[] GetPopupNames()
        {
            if (popupRoot == null)
                return null;

            var strList = new List<string>();
            for (var i = 0; i < popupRoot.childCount; ++i)
            {
                var child = popupRoot.GetChild(i);
                if (child.GetComponent<PopupBase>() != null)
                    strList.Add(child.name);
            }

            return strList.ToArray();
        }
        // ================================

        public void PreInit()
        {
            PopupDic = new Dictionary<string, PopupBase>();
            showingPopupStack = new Stack<string>();
            waitingPopupQueue = new Queue<string>();

            FindChildPopups();

            foreach (var popup in PopupDic)
                popup.Value.PreInit();

            // Dimming
            _dimmingPool = new ObjectPool<Dimming>(Resources.Load<Dimming>("Prefabs/Dimming"), popupRoot, PopupDic.Count, 4, true);
            _dimmingStack = new Stack<Dimming>();
        }

        public bool IsInitialize()
        {
            return PopupDic != null;
        }

        public void FindChildPopups()
        {
            if (popupRoot == null)
            {
                Debug.LogError("[Banana] PopupRoot is Null!!");
                return;
            }

            for (var i = 0; i < popupRoot.childCount; ++i)
            {
                var child = popupRoot.GetChild(i).GetComponent<PopupBase>();
                if (child != null)
                    PopupDic.Add(child.name, child);
            }
        }

        public void ShowPopup(string newPopupName, bool isQueued = false)
        {
            if (!IsInitialize())
                PreInit();

            if (!PopupDic.ContainsKey(newPopupName))
                return;

            if (isQueued)
            {
                if (showingPopupStack.Count > 0 || waitingPopupQueue.Count > 0)
                {
                    waitingPopupQueue.Enqueue(newPopupName);
                    return;
                }
            }
            else
            {
                // Overlap is blocked..
                if (showingPopupStack.Contains(newPopupName))
                {
                    return;
                }

                if (IsTransition && ignoreWhileTransition)
                    return;
            }

            if (_showingRoutine != null)
            {
                StopCoroutine(_showingRoutine);
                _showingRoutine = null;
            }

            _showingRoutine = StartCoroutine(ShowPopupRoutine(newPopupName));
        }

        public void PopHidePopup(bool isForce = false)
        {
            if (!IsInitialize())
                PreInit();

            if (showingPopupStack.Count == 0)
                return;

            var popup = showingPopupStack.Peek();
            if (IsTransition && ignoreWhileTransition)
            {
                if (isForce == false)
                    return;
            }
            else if (!ignoreWhileTransition)
                StopCoroutine(PopupDic[popup].ShowPopup());

            if (_hidingRoutine != null)
                StopCoroutine(_hidingRoutine);

            _hidingRoutine = StartCoroutine(HidePopupRoutine(popup));
        }

        public void HidePopup(string popupName)
        {
            // 전제조건: ShowingPopupStack내의 popupName이 하나만 있다.
            if (!showingPopupStack.Contains(popupName))
                return;

            var popupNames = new string[showingPopupStack.Count];

            for (var i = 0; i < popupNames.Length; ++i)
                popupNames[i] = showingPopupStack.Pop();

            for (var i = popupNames.Length - 1; i >= 0; --i)
            {
                if (popupNames[i] == popupName)
                    continue;

                showingPopupStack.Push(popupNames[i]);
            }

            showingPopupStack.Push(popupName);
            PopHidePopup();
        }

        private IEnumerator ShowPopupRoutine(string showingPopupName)
        {
            IsTransition = true;

            var popup = PopupDic[showingPopupName];
            popup.ShowWill();

            if (isOnlyOnePopup && showingPopupStack.Count >= 1)
            {
                var hidePoupName = showingPopupStack.Peek();
                StartCoroutine(PopupDic[hidePoupName].HidePopup(true));
            }

            showingPopupStack.Push(showingPopupName);

            if (PopupDic[showingPopupName].isHaveDimming)
                PushDimming();

            if (ignoreWhileTransition)
                yield return popup.ShowPopup();
            else
                StartCoroutine(popup.ShowPopup());

            IsTransition = false;
            _showingRoutine = null;
        }

        private IEnumerator HidePopupRoutine(string hidingPopupName)
        {
            IsTransition = true;

            showingPopupStack.Pop();

            if (isOnlyOnePopup && showingPopupStack.Count >= 1)
            {
                var prevPopupName = showingPopupStack.Peek();
                StartCoroutine(PopupDic[prevPopupName].ShowPopup());
            }

            if (PopupDic[hidingPopupName].isHaveDimming)
                PopDimming();

            if (ignoreWhileTransition || waitingPopupQueue.Count > 0)
                yield return PopupDic[hidingPopupName].HidePopup();
            else
                StartCoroutine(PopupDic[hidingPopupName].HidePopup());

            IsTransition = false;

            if (showingPopupStack.Count == 0 && waitingPopupQueue.Count > 0)
            {
                var queuePoup = waitingPopupQueue.Dequeue();
                ShowPopup(queuePoup);
                _hidingRoutine = null;
                yield break;
            }

            _hidingRoutine = null;
        }

        // --------------------
        // Dimming 
        // 
        private void PushDimming()
        {
            if (isOnlyOnePopup && _dimmingStack.Count >= 1)
                return;
            else
            {
                if (_dimmingStack.Count >= showingPopupStack.Count)
                    return;
            }

            var dimming = _dimmingPool.Pop();
            dimming.ShowDimming();
            _dimmingStack.Push(dimming);
        }

        private void PopDimming()
        {
            if (isOnlyOnePopup && showingPopupStack.Count > 0)
                return;

            var dimming = _dimmingStack.Pop();
            dimming.HideDimming();
            _dimmingPool.Push(dimming, true);

        }
    }
}
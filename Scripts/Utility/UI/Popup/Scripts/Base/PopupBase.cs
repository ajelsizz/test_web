using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Banana.UI.Popup
{
    public enum PopupShowTransitionType
    {
        None,
        ScaleUp,
        Up,
        Down,
        Left,
        Right,
    }
    public enum PopupHideTransitionType
    {
        None,
        ScaleDown,
        Up,
        Down,
        Left,
        Right,
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class PopupBase : CanvasRootObject
    {
        [SerializeField] private bool isChangeCanvasGroupSetting = true;
        [SerializeField] private PopupShowTransitionType popupShowType;
        [SerializeField] private PopupHideTransitionType popupHideType;

        public bool isHaveDimming;

        public bool IsShown { get; private set; }
        private bool _isShowing;

        public override void PreInit()
        {
            base.PreInit();

            IsShown = false;

            gameObject.SetActive(true);
            SetActiveCanvasGroup(false);
        }

        public virtual IEnumerator ShowPopup()
        {
            transform.SetAsLastSibling();
            transform.DOKill();
            transform.localScale = Vector3.one;

            _isShowing = true;

            switch (popupShowType)
            {
                case PopupShowTransitionType.ScaleUp:
                    transform.localScale = Vector3.zero;
                    SetActiveCanvasGroup(true);
                    yield return transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Up:
                    transform.localPosition = new Vector3(0, -1500, 0);
                    SetActiveCanvasGroup(true);
                    yield return transform.DOLocalMoveY(0f, 0.3f).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Down:
                    transform.localPosition = new Vector3(0, 1500, 0);
                    SetActiveCanvasGroup(true);
                    yield return transform.DOLocalMoveY(0f, 0.3f).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Left:
                    transform.localPosition = new Vector3(-1200, 0, 0);
                    SetActiveCanvasGroup(true);
                    yield return transform.DOLocalMoveX(0f, 0.3f).WaitForCompletion();
                    break;
                case PopupShowTransitionType.Right:
                    transform.localPosition = new Vector3(1200, 0, 0);
                    SetActiveCanvasGroup(true);
                    yield return transform.DOLocalMoveX(0f, 0.3f).WaitForCompletion();
                    break;
                default:
                    SetActiveCanvasGroup(true);
                    break;
            }

            ShowDone();
        }

        public virtual void ShowWill()
        {
            IsShown = false;
        }

        public virtual void ShowDone()
        {
            IsShown = true;
        }

        public virtual IEnumerator HidePopup(bool isHideImmediately = false)
        {
            _isShowing = false;
            transform.DOKill();

            IsShown = false;
            if (isHideImmediately == false)
            {
                switch (popupHideType)
                {
                    case PopupHideTransitionType.ScaleDown:
                        yield return transform.DOScale(0, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Up:
                        yield return transform.DOLocalMoveY(1500f, 0.3f).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Down:
                        yield return transform.DOLocalMoveY(-1500f, 0.3f).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Left:
                        yield return transform.DOLocalMoveX(-1200f, 0.3f).WaitForCompletion();
                        break;
                    case PopupHideTransitionType.Right:
                        yield return transform.DOLocalMoveX(1200f, 0.3f).WaitForCompletion();
                        break;
                    default:
                        break;
                }
            }

            if(_isShowing == false)
                SetActiveCanvasGroup(false);

            HideDone();
        }

        public virtual void HideDone()
        {
        }
    }
}


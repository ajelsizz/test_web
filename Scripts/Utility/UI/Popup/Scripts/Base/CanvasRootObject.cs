using UnityEngine;
using DG.Tweening;

namespace Banana.UI.Popup
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasRootObject : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;
        [SerializeField] private bool _isNotChangeGroupSetting;

        public virtual void PreInit()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetActiveCanvasGroup(bool enable, float fadeTime = 0.0f)
        {
            canvasGroup.DOKill();

            if (_isNotChangeGroupSetting == false)
                canvasGroup.interactable = enable;

            if (fadeTime > 0f)
                canvasGroup.DOFade(enable ? 1f : 0f, fadeTime);
            else
                canvasGroup.alpha = enable ? 1f : 0f;

            if (_isNotChangeGroupSetting == false)
                canvasGroup.blocksRaycasts = enable;
        }
    }
}
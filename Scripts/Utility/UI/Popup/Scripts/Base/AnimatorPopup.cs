using System.Collections;
using UnityEngine;

namespace Banana.UI.Popup
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorPopup : PopupBase
    {
        [SerializeField] private bool haveInAnimation = true;

        private readonly string animationIn = "In";
        private readonly string animationReset = "Reset";

        protected Animator popupAnimator;

        public override void PreInit()
        {
            base.PreInit();
            popupAnimator = GetComponent<Animator>();
        }

        public override void ShowWill()
        {
            base.ShowWill();

            if (haveInAnimation)
            {
                popupAnimator.enabled = true;
                popupAnimator.Play(animationReset, -1, 0.0f);
            }
        }

        public override void ShowDone()
        {
            base.ShowDone();

            if(haveInAnimation)
                popupAnimator.Play(animationIn, -1, 0.0f);
        }

        public override IEnumerator HidePopup(bool isHideImmediately = false)
        {
            popupAnimator.enabled = false;
            yield return base.HidePopup(isHideImmediately);
        }
    }
}
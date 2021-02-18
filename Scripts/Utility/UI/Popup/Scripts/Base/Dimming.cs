using Banana.Patterns;

namespace Banana.UI.Popup
{
    public class Dimming : CanvasRootObject, IManagedObject
    {
        public override void PreInit()
        {
            base.PreInit();
            SetActiveCanvasGroup(false);
        }

        public void Release()
        {

        }

        public void ShowDimming()
        {
            transform.SetAsLastSibling();
            SetActiveCanvasGroup(true, 0.1f);
        }

        public void HideDimming()
        {
            transform.SetAsFirstSibling();
            SetActiveCanvasGroup(false, 0.1f);
        }

        public void OnPressedDimming()
        {
            PopupManager.Instance.PopHidePopup();
        }
    }
}
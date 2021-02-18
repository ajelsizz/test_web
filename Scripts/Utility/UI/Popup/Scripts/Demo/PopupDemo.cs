using UnityEngine;
using Banana.UI.Popup;

public class PopupDemo : MonoBehaviour
{
    void Start()
    {
        PopupManager.Instance.PreInit();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            PopupManager.Instance.ShowPopup("TestPopup1", true);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            PopupManager.Instance.ShowPopup("TestPopup2");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PopupManager.Instance.ShowPopup("TestPopup_Ani");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            PopupManager.Instance.PopHidePopup();
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            PopupManager.Instance.HidePopup("TestPopup_Ani");
        }
    }
}

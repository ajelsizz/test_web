using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleBattleController : MonoBehaviour
{
	public UISpineController skillSpine;

	public GameObject skillInforRoot;
	public GameObject spineRoot;
	public GameObject informationRoot;
	public UILabel unitName;
	public UILabel skillName;

    public Unit leftUnit;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if( Input.GetKeyDown(KeyCode.D))
        {
            skillInforRoot.SetActive(true);
            Debug.LogError("D");
            SetSkillInformation("10601");
            StartSkillInformation();
        }
	}
    //public void SetUnit(Unit left)
    //{

    //}

    void SetSkillInformation(string name)
    {
        skillSpine.TartgetParent = skillSpine.gameObject;

        //skillSpine.spinePrefabName = string.Format("{0}_big", leftUnit.spineName);

        string strPath = string.Empty;

        string strBase = "Prefabs/FMH/";

        strPath = strBase + "SP_Spine/" + "SP_" + name;

        skillSpine.spineType = ESpineType.SpSpine;
        skillSpine.spinePrefabName = strPath;

        unitName.text = name;

        string skillString = "-";
        while (skillString == "-")
        {
            int ran = Random.Range(0, 2);
            skillString = "test";
        }

        skillName.text = skillString;
    }


    void SetSkillInformation()
    {
        skillSpine.TartgetParent = skillSpine.gameObject;

        //skillSpine.spinePrefabName = string.Format("{0}_big", leftUnit.spineName);

        string strPath = string.Empty;

        string strBase = "Prefabs/FMH/";

        strPath = strBase + "SP_Spine/" + "SP_" + leftUnit.spineName;

        skillSpine.spineType = ESpineType.SpSpine;
        skillSpine.spinePrefabName = strPath;

        unitName.text = leftUnit.unitName;

        string skillString = "-";
        while (skillString == "-")
        {
            int ran = Random.Range(0, 2);
            skillString = leftUnit.skillName[ran];
        }

        skillName.text = skillString;
    }

    public void StartSkillInformation()
    {
        skillInforRoot.SetActive(true);

//        if (leftUnit.PlayerNumber == 0)
  //          ;
        //          SoundManager.PlaySFX(string.Format("{0}_Skill", leftUnit.spineName));
        iTween.MoveFrom(spineRoot, iTween.Hash("x", -3, "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo));
        iTween.MoveFrom(informationRoot, iTween.Hash("x", -3, "time", 0.7f, "easetype", iTween.EaseType.easeOutExpo, "oncompletetarget", this.gameObject, "oncomplete", "SkillInformationEnd"));
    }

    public void StartFatalCut()
    {
        //  fatalCut.Show(leftUnit);
    }

    void SkillInformationEnd()
    {
        iTween.MoveTo(spineRoot, iTween.Hash("x", 3, "time", 0.3f, "delay", 0.5f, "easetype", iTween.EaseType.easeInExpo, "oncompletetarget", this.gameObject, "oncomplete", "End"));
    }

    void End()
    {
        skillSpine.DestroySpine();

        skillInforRoot.SetActive(false);
        spineRoot.transform.localPosition = Vector3.zero;

    }
}

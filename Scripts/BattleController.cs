using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    public InBattleUnit leftUnit;
    public InBattleUnit rightUnit;

    Vector3 leftPosition = new Vector3(-200, -150, 0);
    Vector3 rightPosition = new Vector3(200, -150, 0);

    Vector3 leftAttackPosition = new Vector3(100, -150, 0);
    Vector3 leftAttackPosition1 = new Vector3(50, -150, 0);

    Vector3 rightAttackPosition = new Vector3(-100, -150, 0);
    Vector3 rightAttackPosition1 = new Vector3(-50, -150, 0);

    public GameObject skillInforRoot;
    public GameObject spineRoot;
    public GameObject informationRoot;

    public UIFatalCut fatalCut;

    public BattleSpineController skillSpine;

    public void SetUnit(Unit left, Unit right, bool leftTurn)
    {
        leftUnit.SetUnit(left);
        rightUnit.SetUnit(right);

        if (leftTurn)
        {
            SetSkillInformation();
        }

    }

    void SetSkillInformation()
    {
       // skillSpine.target = skillSpine.gameObject;

        //skillSpine.spinePrefabName = string.Format("{0}_big", leftUnit.spineName);

        string strPath = string.Empty;

        string strBase = "Prefabs/FMH/";

        strPath = strBase + "SP_Spine/" + "SP_" + leftUnit.GetUnitData().spineName;

        skillSpine.spineType = ESpineType.SpSpine;
        skillSpine.spinePrefabName = strPath;

        //unitName.text = leftUnit.unitName;

        //string skillString = "-";
        //while (skillString == "-")
        //{
        //    int ran = Random.Range(0, 2);
        //    skillString = leftUnit.skillName[ran];
        //}

        //skillName.text = skillString;
    }

    void SetSkillInformation1()
    {
        // skillSpine.target = skillSpine.gameObject;

        //skillSpine.spinePrefabName = string.Format("{0}_big", leftUnit.spineName);

        string strPath = string.Empty;

        string strBase = "Prefabs/FMH/";

        strPath = strBase + "SP_Spine/" + "SP_" + "10601";

        skillSpine.spineType = ESpineType.SpSpine;
        skillSpine.spinePrefabName = strPath;

        //unitName.text = leftUnit.unitName;

        //string skillString = "-";
        //while (skillString == "-")
        //{
        //    int ran = Random.Range(0, 2);
        //    skillString = leftUnit.skillName[ran];
        //}

        //skillName.text = skillString;
    }


    public void StartSkillInformation()
    {
        skillInforRoot.SetActive(true);

        //if (leftUnit.PlayerNumber == 0)
        //    ;


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
        spineRoot.transform.localPosition = new Vector3(-100, 0, 0);

    }

    public void StartLeftAttack()
    {
        iTween.MoveFrom(leftUnit.gameObject, iTween.Hash("x", -5, "time", 0.5f, "oncompletetarget", this.gameObject, "oncomplete", "StartSkillInformation"));
        iTween.MoveFrom(rightUnit.gameObject, iTween.Hash("x", 5, "time", 0.5f));
        Vector3 attackPosition = Vector3.zero;

        if (leftUnit.unitType == UnitType.Sword)
            attackPosition = leftAttackPosition;
        else if (leftUnit.unitType == UnitType.Spear)
            attackPosition = leftAttackPosition1;


        leftUnit.DealDamage(rightUnit, attackPosition);
    }


    public void StartRightAttack()
    {
        iTween.MoveFrom(leftUnit.gameObject, iTween.Hash("x", -5, "time", 0.5f));
        iTween.MoveFrom(rightUnit.gameObject, iTween.Hash("x", 5, "time", 0.5f));
        Vector3 attackPosition = Vector3.zero;

        if (rightUnit.unitType == UnitType.Sword)
            attackPosition = rightAttackPosition;
        else if (rightUnit.unitType == UnitType.Spear)
            attackPosition = rightAttackPosition1;


        rightUnit.DealDamage(leftUnit, attackPosition);

    }

    void ResetPosition()
    {
        leftUnit.transform.localPosition = leftPosition;
        rightUnit.transform.localPosition = rightPosition;
    //    leftUnit.spineColor.SetColor(new Color(1, 1, 1, 1));
    //    rightUnit.spineColor.SetColor(new Color(1, 1, 1, 1));
    }

    public void Close()
    {
        BattleManager.instance.ExitBatting();

        ResetPosition();
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.V))
        {
            CameraShake.Shake(0.25f, 8f);
        }
    }

}
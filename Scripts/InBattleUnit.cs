using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class InBattleUnit : MonoBehaviour
{

	/*
	* attack skill event
	*/
	public delegate void del_AttackDelegate(GameObject fxAttack ,GameObject fx);
	public static event del_AttackDelegate evtAttackEvent;

	public bool bIsAttacking = false;

	public BattleSpineController spine;

	private SkeletonAnimation targetSpine = null;
	public InBattleUnit targetUnit = null;

	Unit data;

	//[HideInInspector]
	public int PlayerNumber;

//	[HideInInspector]
	public UnitType unitType;


	//public int HitPoints;
	public void SetUnit(Unit unit)
    {
		data = unit;

		this.PlayerNumber = data.PlayerNumber;
		this.unitType = data.unitType;
		//this.HitPoints = data.HitPoints;


		string strPath = "";
		string strBase = "Prefabs/FMH/";

		if (PlayerNumber == 0) // player
			strPath = strBase + "SD_Spine/" + "SD_" + unit.spineName;
		else
			strPath = strBase + "SD_Spine/" + "SDE_" + unit.spineName;

		spine.spineType = ESpineType.SdSpine;
		spine.spinePrefabName = strPath;


		//---
		targetSpine = spine.target.GetComponent<Spine.Unity.SkeletonAnimation>();
		targetSpine.AnimationState.Event += HitEvent;
		//spineColor = spine.target.GetComponent<SpineSlotColorChanger>();

	}

	public Unit GetUnitData() { return data; }

	void HitEvent(TrackEntry trackEntry, Spine.Event e)
	{

		if (bIsAttacking == false)
		{
			Debug.LogError("name = " + data.unitName + " e.Data.Name = " + e.Data.Name);
			bIsAttacking = true;

			evtAttackEvent(data.effectPrefab[0] ,data.effectPrefab[1]);
		}
	}

	public void  MoveAsAttacking(InBattleUnit other, Vector3 position)
	{
		targetUnit = other;

		StartCoroutine(MoveJerk(other, position));
	}

	public void CounterAttack()
    {

    }

	public void DealDamage(InBattleUnit other, Vector3 position)
	{
		//MoveAsAttacking(position);
		MoveAsAttacking(other, position);
		//other.Defend(this);
	}


	private IEnumerator MoveJerk(InBattleUnit other, Vector3 position)
	{

		yield return new WaitForSeconds(1.0f);

        if (PlayerNumber == 0)
            yield return new WaitForSeconds(1.3f);

       	if (unitType == UnitType.Spear || unitType == UnitType.Sword || unitType == UnitType.Axe)
        {
			spine.SetAnimation("03_move");
			iTween.MoveTo(this.gameObject, iTween.Hash("x", position.x, "islocal", true, "time", 0.5f));
			yield return new WaitForSeconds(0.5f);
		}

		spine.SetAnimation("02_attack", false);
	
		//if (PlayerNumber == 0)
		//    ;
		//			SoundManager.PlaySFX(string.Format("{0}_Attack", unit.spineName));

		//spineAnimation.SetAnimation("02_attack", false);

		//targetSpine.AnimationState.SetAnimation(0, "02_attack", false);


		//      if (effectList[0] != null)
		//          effectList[0].SetActive(true);

		//      yield return new WaitForSeconds(1.5f);

		//Reset();

		//	spineAnimation.SetAnimation("01_idle");
	}

	public void MoveAsDefending(Unit other)
	{
		//StartCoroutine(MoveDefense(other));
	}

	//private IEnumerator MoveDefense(Unit other)
	//{
	//      yield return new WaitForSeconds(0.1f);

	// //      if (other.PlayerNumber == 0)
	//	//	yield return new WaitForSeconds(1.5f);
	//	//if (other.unitType == UnitType.Spear || other.unitType == UnitType.Sword)
	//	//	yield return new WaitForSeconds(0.5f);

	//	//yield return new WaitForSeconds(0.5f);

	//	if (PlayerNumber != other.PlayerNumber)
	//	{
	//		effectList[1].SetActive(true);

	//		spineAnimation.SetAnimation("04_hit", false);

	//		HitDamage(other);
	//	}
	//	else
	//		HitBuff(other);

	//	BattleManager.instance.cellGrid.battleInformation.RefreshBattleUnit();

	//       //yield return new WaitForSeconds(1.0f);

	//       this.damage.gameObject.SetActive(false);

	//       this.heal.gameObject.SetActive(false);

	//       if (HitPoints <= 0)
	//       {
	//           Cell.IsTaken = false;
	//           if (PlayerNumber == 0)
	//           {
	//              //BattleManager.instance.cellGrid.battleInformation.battleControll.StartFatalCut();
	//             //  yield return new WaitForSeconds(2.0f);
	//           }
	//           yield return StartCoroutine("OnDestroyedAnimation");
	//          // yield return new WaitForSeconds(0.5f);
	//       }
	//       else
	//       {
	//           spineAnimation.SetAnimation("01_idle");
	//       }
	//       //yield return new WaitForSeconds(0.5f);

	//       Reset();

	//   //    BattleManager.instance.cellGrid.battleInformation.battleControll.Close();
	//   //    BattleManager.instance.cellGrid.TurnOverCheck();
	//   }

	// origin
	//	private IEnumerator MoveGlow(Unit other)
	//	{
	//		yield return new WaitForSeconds(1.0f);

	//		if(other.PlayerNumber == 0)
	//			yield return new WaitForSeconds(1.5f);
	//		if(other.unitType == UnitType.Spear || other.unitType == UnitType.Sword)
	//			yield return new WaitForSeconds(0.5f);
	//        //if (PlayerNumber == 0)
	//        //    ;
	////			SoundManager.PlaySFX(string.Format("{0}_Hit", unit.spineName));

	//        yield return new WaitForSeconds(0.5f);

	//        if(PlayerNumber != other.PlayerNumber)
	//        {
	//			effectList[1].SetActive(true);

	//            spineAnimation.SetAnimation("04_hit", false);

	//            HitDamage(other);
	//        }
	//        else
	//			HitBuff(other);

	//		BattleManager.instance.cellGrid.battleInformation.RefreshBattleUnit();

	//        yield return new WaitForSeconds(1.0f);

	//		this.damage.gameObject.SetActive(false);

	//		this.heal.gameObject.SetActive(false);

	//		if (HitPoints <= 0)
	//		{
	//			Cell.IsTaken = false;
	//			if(PlayerNumber == 0)
	//			{
	//				BattleManager.instance.cellGrid.battleInformation.battleControll.StartFatalCut();
	//				yield return new WaitForSeconds(2.0f);
	//			}
	//			yield return StartCoroutine("OnDestroyedAnimation");
	//			yield return new WaitForSeconds(0.5f);
	//		}
	//		else
	//		{
	//			spineAnimation.SetAnimation("01_idle");
	//		}
	//		yield return new WaitForSeconds(0.5f);

	//		Reset();

	//		BattleManager.instance.cellGrid.battleInformation.battleControll.Close();
	//		BattleManager.instance.cellGrid.TurnOverCheck();
	//	}

	int AdvantageState(int damage, UnitType enemyType)
	{
		float calculateDamage = 1.0f;
		if (unitType == UnitType.Sword && enemyType == UnitType.Axe)
			calculateDamage = 1.5f;
		else if (unitType == UnitType.Spear && enemyType == UnitType.Sword)
			calculateDamage = 1.5f;
		else if (unitType == UnitType.Axe && enemyType == UnitType.Spear)
			calculateDamage = 1.5f;
		//else if (unitType == UnitType.Bow && enemyType == UnitType.Bow)
		//	calculateDamage = 0.5f;

		return (int)((float)damage * calculateDamage);
	}

	public void HitDamage(InBattleUnit other)
	{

		//      int damage = other.data.AttackFactor;
		//      int calculation = Mathf.Clamp(damage - data.DefenceFactor, 1, damage);
		//      HitPoints -= calculation;
		//data.HitPoints = HitPoints;

		int damage = other.data.AttackFactor;

		int calculation = Mathf.Clamp(damage - data.DefenceFactor, 1, damage);

		//--

		int nFinalDmg = this.AdvantageState(calculation, other.unitType);

		data.HitPoints -= nFinalDmg;


		Debug.Log("HitDamage c= " + calculation + " fianl = " + nFinalDmg);

        HitDamageAnimation(nFinalDmg);

		DamageText dt = DamageTextPool.Instance.GetDamageText();

		Vector3 pos = this.transform.position;
		pos.y += 200;
		dt.SetDamageText(pos, calculation, false, false, Color.red);


		if (data.HitPoints <= 0)
        {
			data.Destroyed(other.data);
			data.Cell.PlayerNumber = -1;
        }
		

		
    }

	public void HitBuff(InBattleUnit other)
	{
        int heal = other.data.AttackFactor;
        int calculation = Mathf.Clamp((data.TotalHitPoints - data.HitPoints), 0, heal);
		data.HitPoints += calculation;
        
        HealAnimation(calculation);

		//---

		DamageText dt = DamageTextPool.Instance.GetDamageText();

		Vector3 pos = this.transform.position;
		pos.y += 200;
		dt.SetDamageText(pos, calculation, false, false, Color.green);

		Debug.Log("a");
	}

	public void HitDamageAnimation(int damage)
	{
		//this.damage.gameObject.SetActive(true);

		//iTween.ValueTo(gameObject, iTween.Hash(
		//	"from", 0,
		//	"to", damage,
		//	"onupdatetarget", gameObject,
		//	"onupdate", "damageOnUpdateCallBack",
		//	"time", 0.5f
		//	, "easetype", iTween.EaseType.linear));
	}

	public void HealAnimation(int heal)
	{
		//this.heal.gameObject.SetActive(true);

		//iTween.ValueTo(gameObject, iTween.Hash(
		//	"from", 0,
		//	"to", heal,
		//	"onupdatetarget", gameObject,
		//	"onupdate", "healOnUpdateCallBack",
		//	"time", 0.5f
		//	, "easetype", iTween.EaseType.linear));
	}

	void damageOnUpdateCallBack(int newValue)
	{
	//	this.damage.text = newValue.ToString();
	}

	void healOnUpdateCallBack(int newValue)
	{
	//	this.heal.text = newValue.ToString();
	}

	public void Reset()
	{

		//spineAnimation.SetAnimation("01_idle");

		//hitRoot.SetActive(false);
		//attackRoot.SetActive(false);

		//// effec remove
		//for (int i = 0; i < effectList.Count; i++)
		//{
		//	GameObject obj = effectList[i];
		//	DestroyImmediate(obj);
		//}

		//effectList.Clear();
	}
}

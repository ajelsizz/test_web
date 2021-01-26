using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventSpine : MonoBehaviour
{

	public InBattleUnit srcInBattleUnit;
    public InBattleUnit targetInBattleUnit;

    private void Awake()
    {
        srcInBattleUnit = this.GetComponent<InBattleUnit>();

        InBattleUnit.evtAttackEvent += Attacking;

        Debug.Log("ActionEventSpine Awake");
    }

    public void Attacking(GameObject fxAttack, GameObject fx)
    {
        Debug.LogError("atk = " + srcInBattleUnit.bIsAttacking);
        if (srcInBattleUnit.bIsAttacking == true)
        {
            Debug.LogError("ActionEventSpine Attacking");
            StartCoroutine(coUnitAction(fxAttack, fx));

        }
    }

    private GameObject goHitFX = null;
    private GameObject goAttackFX = null;

    private IEnumerator coUnitAction(GameObject fxAttack, GameObject fx)
    {
        yield return new WaitForSeconds(0.1f);

        if (srcInBattleUnit.targetUnit != null)
        {
            targetInBattleUnit = srcInBattleUnit.targetUnit.GetComponent<InBattleUnit>();

            if (srcInBattleUnit.PlayerNumber != targetInBattleUnit.PlayerNumber)
            {
               
                if (fxAttack != null)
                {
                    goAttackFX = GameObject.Instantiate(fxAttack);
                    goAttackFX.transform.parent = this.transform.parent.transform;
                    goAttackFX.transform.localScale = new Vector3(1, 1, 1);
                    goAttackFX.transform.localPosition = new Vector3(targetInBattleUnit.transform.localPosition.x,
                                                                  targetInBattleUnit.transform.localPosition.y / 2,
                                                                  targetInBattleUnit.transform.localPosition.z);
                    if(srcInBattleUnit.PlayerNumber == 0)
                        goAttackFX.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    else if(srcInBattleUnit.PlayerNumber == 1)
                        goAttackFX.transform.localRotation = new Quaternion(0, -180, 0, 0);

                    goAttackFX.SetActive(true);

                }

                CameraShake.Shake(0.25f, 8f);

                targetInBattleUnit.spine.SetAnimation("04_hit", false);
                targetInBattleUnit.HitDamage(srcInBattleUnit);


            }
            else
            {
                if (fxAttack != null)
                {
                    goAttackFX = GameObject.Instantiate(fxAttack);
                    goAttackFX.transform.parent = this.transform.parent.transform;
                    goAttackFX.transform.localScale = new Vector3(1, 1, 1);
                    goAttackFX.transform.localPosition = new Vector3(targetInBattleUnit.transform.localPosition.x,
                                                              targetInBattleUnit.transform.localPosition.y/2,
                                                              targetInBattleUnit.transform.localPosition.z);

                    goAttackFX.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    goAttackFX.SetActive(true);

                }

                targetInBattleUnit.HitBuff(srcInBattleUnit);

                //--
                srcInBattleUnit.GetUnitData().bCounter = true;
                srcInBattleUnit.GetUnitData().bInBattleEnd = true;

                targetInBattleUnit.GetUnitData().bCounter = true;
                targetInBattleUnit.GetUnitData().bInBattleEnd = true;

         //       BattleManager.instance.cellGrid.battleInformation.RefreshBattleUnit();

                yield return new WaitForSeconds(1.0f);


            }

    //        BattleManager.instance.cellGrid.battleInformation.RefreshBattleUnit();

            yield return new WaitForSeconds(1.0f);

            if (targetInBattleUnit.GetUnitData().HitPoints <= 0)
            {
                targetInBattleUnit.spine.SetAnimation("05_die", false);
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                targetInBattleUnit.spine.SetAnimation("01_idle");
            }

            // src Attack End & idle
            srcInBattleUnit.bIsAttacking = false;
            srcInBattleUnit.spine.SetAnimation("01_idle");
            srcInBattleUnit.GetUnitData().bInBattleEnd = true;

            // Destory FX 
            Destroy_Fx(goHitFX);
            Destroy_Fx(goAttackFX);


            //---------------------------------------------------
            //---right count attack
            /*
             * 첫 반격이거나 살아 있고 반격 범위에 있으면
             * 
             */
            if(targetInBattleUnit.GetUnitData().bCounter == false &&
                targetInBattleUnit.GetUnitData().HitPoints > 0 &&
                IsCounterAttack())
            {
                /*
                 * 반격 후 전투 종료
                 */
                if (srcInBattleUnit.GetUnitData().bInBattleEnd == true &&
                    targetInBattleUnit.GetUnitData().bInBattleEnd == true)
                {
                    // End Process
                    Debug.LogError("InBattle End 1");

                    srcInBattleUnit.spine.SetAnimation("01_idle");
                    targetInBattleUnit.spine.SetAnimation("01_idle");
                    InBattleActionReset();

                    BattleEnd();
                }
                else
                {
                    // counter attack
                    targetInBattleUnit.GetUnitData().bCounter = true;

                    targetInBattleUnit.spine.SetAnimation("02_attack", false);
                    targetInBattleUnit.targetUnit = srcInBattleUnit;

                    Debug.LogError("Counter Attack Start");
                }
            }
            else
            {
                /*
                 * 반격 안하는 경우 전투 종료
                 */
                InBattleActionReset();

                BattleEnd();

                Debug.LogError("InBattle End 3");
            }

        }
    }

    void CounterAttackProcess()
    {

    }

    /*
     * 전투 종료 체크 처리 
     */
    void BattleEnd()
    {
        BattleManager.instance.cellGrid.battleInformation.battleCTR.Close();
        BattleManager.instance.cellGrid.TurnOverCheck();
    }

    // attack range check
    bool IsCounterAttack()
    {
        int nSrcRange = srcInBattleUnit.GetUnitData().AttackRange;
        int nTargetRange = targetInBattleUnit.GetUnitData().AttackRange;

        if (nTargetRange >= nSrcRange)
            return true;
        else
            return false;
    }

    /*
     * Battle Action Reset
     */
    void InBattleActionReset()
    {

        //reset
        srcInBattleUnit.GetUnitData().bInBattleEnd = false;
        targetInBattleUnit.GetUnitData().bInBattleEnd = false;

        srcInBattleUnit.GetUnitData().bCounter = false;
        targetInBattleUnit.GetUnitData().bCounter = false;

    }

    void Destroy_Fx(GameObject fx)
    {
        if (fx != null)
        {
            DestroyImmediate(fx);
            fx = null;
        }
    }
}

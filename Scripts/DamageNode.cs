using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType
{
    Sword,
    Bow,
    Magic,
}

public enum eAttackType
{
    Normal,
    Skill
}

public enum eAttributeType
{
    Normal,
    Fire,
    Ice,
    Poison,
}

public enum eBuffType
{
    None = -1,
    Buff,
    Heal,
    Debuff,         // 디버프는 이 이하로 작성
    Poison,
    Brun,
    Freezing,
    Sleep,
    Stun,
}

[System.Serializable]
public class BuffInfo
{
    public eBuffType        _buffType;   
    public eAttributeType   _attribute;     // 속성 (독, 불, 얼음 등.)
    public int  _buffValue;
    public int  _durationTrun;      // 몇 턴 동안 지속되는가?
    public int  _probability;       // 확률. 30일 시 30%

    public BuffInfo Clone()
    {
        BuffInfo clone = new BuffInfo();
        clone._buffType     = _buffType;
        clone._attribute    = _attribute;
        clone._buffValue    = _buffValue;
        clone._durationTrun = _durationTrun;
        clone._probability  =_probability;

        return clone;
    }
}

[System.Serializable]
public class DamageNode
{
    public PlayerType       _teamType;
    public eAttackType      _attackType;    // 스킬인가 일반 공격인가.
    public eAttributeType   _attribute;     // 공격 속성 (독, 불, 얼음 등.)
    public eWeaponType      _weaponType;    // 무기 속성
    public BuffInfo         _buffInfo;
    public int  _damage;        // 데미지 value,
	public bool	_isCritical;	// 크리티컬
    public Unit _unit;          // 가격한 유닛을 알기위함.
}

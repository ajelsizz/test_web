using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems; 

namespace INVEN_SYS
{

    public delegate void EventDele(object sender, MyEventArgs e);
    public delegate void ExecuteDele();
    public class GEvent : Singleton<GEvent>
    {       // various event related functions
        public event EventDele EventHandlers;
        void Start()
        {

        }
        public void AllEventClear()
        {        // delete all subscribers
            EventHandlers = null;
        }

        public void PublishEvent(object sender, MyEventArgs e)
        {           // publish new event
            if (EventHandlers != null)
                EventHandlers.Invoke(sender, e);
        }
    }
    public class MyEventArgs : EventArgs
    {           // custom event argument class
        public MyEventType ThisType;
        public bool Success;
        public int IntValue;
        public float FloatVal;
        public AllyClass EventAlly;
        public MyEventArgs(MyEventType type)
        {
            ThisType = type;
        }
    }

    public enum MyEventType
    {           // what in-game event?
        None, Move, Mana, Card, SkillEnd, EnemyDied, BeAttacked, Attacking, Camping, TurnEnd,
        TurnStartMana, ManaChoose, BattleEnd, BattleTurn, MainChaHP0, AllyDied, HPChanged, CoinChanged, AllyAdded, AllyRemoved,
    }
}

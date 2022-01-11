using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TurnBasedRPG {
    public class PartyManager
    {
        private List<UnitBehaviour> myUnits = new List<UnitBehaviour>();
        private List<UnitBehaviour> opponentUnits = new List<UnitBehaviour>();

        public UnitBehaviour FindRandomTarget(bool targetFaction)
        {
            List<UnitBehaviour> targetFactionUnits = targetFaction ? myUnits : opponentUnits;
            return targetFactionUnits[Random.Range(0, targetFactionUnits.Count)];
        }

        public void RegisterUnit(UnitBehaviour unit)
        {
            if (unit.IsPlayerFaction)
            {
                myUnits.Add(unit);
            }
            else
            {
                opponentUnits.Add(unit);
            }
        }
        
        public Queue<UnitBehaviour> GetAttackQueue()
        {
            Queue<UnitBehaviour> queue = new Queue<UnitBehaviour>();
            List<UnitBehaviour> list = new List<UnitBehaviour>();
            list.AddRange(myUnits);
            list.AddRange(opponentUnits);
            for(int i = list.Count - 1; i > 0; i--)
            {
                int rand = Random.Range(0, i);
                UnitBehaviour temp = list[i];
                list[i] = list[rand];
                list[rand] = temp;
            }
            for(int i = 0; i < list.Count; i++)
            {
                queue.Enqueue(list[i]);
            }
            return queue;
        }
    }
}

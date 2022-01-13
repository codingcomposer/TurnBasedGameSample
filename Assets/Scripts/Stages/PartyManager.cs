using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TurnBasedRPG {
    public class PartyManager
    {

        public List<UnitBehaviour> attackQueue = new List<UnitBehaviour>();
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
        
        // The function doesn't exactly returns Queue, as some of the units may die while waiting for the queue and needed to be removed from it.
        public void FillAttackQueue()
        {
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
            attackQueue.AddRange(list);
        }

        public UnitBehaviour DequeueWhenAttack()
        {
            UnitBehaviour unit = null;
            if (attackQueue.Count > 0)
            {  
                unit = attackQueue[0];
                attackQueue.RemoveAt(0);
            }
            return unit;
        }

        public void UnitDead(UnitBehaviour deadUnit)
        {
            if (myUnits.Contains(deadUnit))
            {
                myUnits.Remove(deadUnit);
            }
            else if (opponentUnits.Contains(deadUnit))
            {
                opponentUnits.Remove(deadUnit);
            }
            attackQueue.Remove(deadUnit);
            if(myUnits.Count < 1)
            {
                StageManager.lose.Invoke();
            }
            else if(opponentUnits.Count < 1)
            {
                StageManager.win.Invoke();
            }
        }
    }
}

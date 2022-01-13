using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Units/UnitDataModel")]
    public class UnitDataModel : ScriptableObject
    {
        public string unitName;
        public UnitAction attack;
        public UnitAction skill;
        public Stat initialStat;
    }
}

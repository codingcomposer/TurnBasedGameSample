using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Items/DisposableItems")]
    public class DisposableItem : ScriptableObject
    {
        public string itemType;
        public enum AffectedStat { hp, defense }
        public AffectedStat affectedStat;
        public float value;

        public void Use(Stat stat)
        {
            switch (affectedStat)
            {
                case AffectedStat.hp:
                    stat.hp += (long)value;
                    break;
                case AffectedStat.defense:
                    stat.defense += (long)value;
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public enum Targeting { randomEnemy, randomFriendly, designatedEnemy, desginatedFriendly}
    [System.Serializable]
    public class UnitAction
    {
        public Targeting Targeting { get { return targeting; } }
        [SerializeField]
        private Targeting targeting;
        public float ActionRange { get { return actionRange; } }
        [SerializeField]
        private float actionRange;
        public string Name { get { return name; } }
        [SerializeField]
        private string name;

        public long PhysicalAttackPower { get { return physicalAttackPower; } }
        [SerializeField]
        private long physicalAttackPower;

        public long MagicAttackPower { get { return magicAttackPower; } }
        [SerializeField]
        private long magicAttackPower;

    }
}

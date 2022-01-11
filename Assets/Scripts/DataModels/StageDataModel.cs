using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Stages/StageDataModel")]
    public class StageDataModel : ScriptableObject
    {
        public string StageName { get { return stageName; } }
        [SerializeField]
        private string stageName;
        public string[] EnemyUnitNames { get { return enemyUnitNames; } }
        [SerializeField]
        private string[] enemyUnitNames;
    }
}

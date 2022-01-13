using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TurnBasedRPG
{
    public class StatMethods
    {
        public static Stat Copy(Stat stat)
        {
            Stat temp = new Stat
            {
                hp = stat.hp,
                defense = stat.defense,
                magicDefense = stat.magicDefense,
                dexterity = stat.dexterity,
                agility = stat.agility,
                incrementalHP = stat.incrementalAgility,
                incrementalDefense = stat.incrementalDefense,
                incrementalMagicDefense = stat.incrementalMagicDefense,
                incrementalDexterity = stat.incrementalDexterity,
                incrementalAgility = stat.incrementalAgility,
            };
            return temp;
        }

        public static Stat CalculateRunTimeStat(Stat initialStat, uint level)
        {
            if (initialStat == null)
            {
                Debug.LogError("initialStat is null : CalculateRuntimeStat");
            }
            Stat runtimeStat = Copy(initialStat);
            if (level < 1)
            {
                level = 1;
            }
            runtimeStat.hp = initialStat.hp + (long)((level - 1) * initialStat.incrementalHP);
            runtimeStat.defense = initialStat.defense + (long)((level - 1) * initialStat.incrementalDefense);
            runtimeStat.magicDefense = initialStat.magicDefense + (long)((level - 1) * initialStat.incrementalMagicDefense);
            runtimeStat.dexterity = initialStat.dexterity + (long)((level - 1) * initialStat.incrementalDexterity);
            runtimeStat.agility = initialStat.agility + (long)((level - 1) * initialStat.incrementalAgility);
            return runtimeStat;
        }

    }
}
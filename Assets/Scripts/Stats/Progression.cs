using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;

        }

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            // return lookUpTable[characterClass][stat][level];

            BuildLookUpTable();
            float[] levels = lookUpTable[characterClass][stat];

            // Debug.LogError("some problem while looking up dictionary");
            if (levels.Length < level) return 0;

            return levels[level - 1];
        }
        public int GetLevels(Stat stat , CharacterClass characterClass) {
            BuildLookUpTable();
            return lookUpTable[characterClass][stat].Length;
            
        }

        private void BuildLookUpTable()
        {
            if (lookUpTable != null) return;
            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }
                lookUpTable[progressionClass.characterClass] = statLookUpTable;
            }
        }
    }
}
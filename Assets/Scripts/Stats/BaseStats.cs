using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect;
        [SerializeField] bool shouldUseModifier = false;

       LazyValue<int> currentLevel;
        public event Action onLevelUp;

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }


        private void Start()
        {
            currentLevel.ForceInit();
        }
        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                Instantiate(levelUpEffect, transform);
                onLevelUp();
            }
        }
        public int GetLevel()
        {
            return currentLevel.value;
        }

        public float GetStat(Stat stat)
        {
            return (progression.GetStat(stat, characterClass, GetLevel()) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = GetComponent<Experience>().GetPoints();
            int maxLevel;

            maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, CharacterClass.Player);
            for (int level = 1; level <= maxLevel; level++)
            {
                if (progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level) > currentXP)
                    return level;
            }
            return maxLevel;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using UnityEngine.Events;

using System;
using GameDevTV.Utils;
using GameDevTV.Saving;

namespace RPG.Atrributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent OnDie;
        GameObject instigator;
        LazyValue<float> health;
        float maxHealth;
        bool isDead = false;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);

        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);

        }
        private void Start()
        {
            health.ForceInit();
            maxHealth = health.value;

        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            health.value = Mathf.Max(health.value, regenHealthPoints);
        }
        public void Heal(float healthToRestore)
        {
            health.value += healthToRestore;
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            health.value = Mathf.Max(health.value - damage, 0);
            if (isDead) return;

            takeDamage.Invoke(damage);
            if (health.value == 0)
            {
                OnDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }
        public float GetPercentage()
        {
            return (health.value / maxHealth) * 100f;
        }
        public float GetHealthPoints() { return health.value; }
        public float GetMaxHealthPoints() { return maxHealth; }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;
            if (health.value == 0)
            {
                Die();
            }
        }


        public bool IsDead() { return isDead; }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }
    }
}
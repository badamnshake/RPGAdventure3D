using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;
        public void GainExperience(float xp)
        {
            experiencePoints += xp;
            onExperienceGained();
        }
        public float GetPoints() { return experiencePoints; }
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
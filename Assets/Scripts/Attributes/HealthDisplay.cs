using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Atrributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        void Update()
        {
            float healthNow = health.GetHealthPoints();
            float healthMax = health.GetMaxHealthPoints();
            GetComponent<Text>().text = string.Format("{0}/{1}", healthNow, healthMax);
        }
    }
}

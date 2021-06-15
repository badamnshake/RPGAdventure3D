using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Atrributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            if (fighter.GetTarget() != null)
            {
                float healthNow = fighter.GetTarget().GetHealthPoints();
                float healthMax = fighter.GetTarget().GetMaxHealthPoints();
                GetComponent<Text>().text = string.Format("{0}/{1}", healthNow, healthMax);
            }
            else
            {
                GetComponent<Text>().text = "N/A";
            }
        }
    }
}
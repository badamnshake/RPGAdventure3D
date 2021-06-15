using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Atrributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Health health = null;
        [SerializeField] Canvas rootCanvas = null;

        void Update()
        {
            float fraction = health.GetPercentage() / 100f;
            if (Mathf.Approximately(fraction, 0) || Mathf.Approximately(fraction, 1))
            {
                rootCanvas.enabled = false;
            }
            else
            {
                rootCanvas.enabled = true;
                foreground.localScale = new Vector3(health.GetPercentage() / 100f, 1, 1);
            }
        }
    }
}
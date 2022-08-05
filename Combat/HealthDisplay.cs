using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] GameObject healthBarParent;
    [SerializeField] Image healthBarImage;

    private void Awake()
    {
        health.ClientOnHealthUpdate += HandleHealthUpdate;
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdate -= HandleHealthUpdate;
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true);    
    }
    private void OnMouseExit()
    {
        healthBarParent.SetActive(false);
    }

    void HandleHealthUpdate(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}

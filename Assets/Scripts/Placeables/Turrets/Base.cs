using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Base : Entity
{
    [SerializeField] private Image healthBar;

    private void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();
        healthComponent.Initialize(20, 20);
        //healthText.text = "Health: " + healthComponent.HealthValue.ToString();
    }



    protected override void HandleHealthChange(int currentHealth, int maxHealth) {
        base.HandleHealthChange(currentHealth, maxHealth);
        //healthText.text = "Health: " + healthComponent.HealthValue.ToString();
        healthBar.fillAmount = Mathf.Clamp((float)healthComponent.HealthValue / (float)healthComponent.MaxHealth,0,1);
    }

    protected override void Die() {
        SceneManager.LoadScene(3);
    }

    private void ShowGameOverScreen() {

    }
}

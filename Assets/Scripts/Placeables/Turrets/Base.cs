using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Base : Entity
{
    private int healthPoints = 10;
    [SerializeField] private GameObject gameOverButtons;
    [SerializeField] private GameObject playButtons;
    [SerializeField] private Text healthText;

    private void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();
        healthComponent.Initialize(20, 20);
        healthText.text = "Health: " + healthComponent.HealthValue.ToString();
    }



    protected override void HandleHealthChange(int currentHealth, int maxHealth) {
        base.HandleHealthChange(currentHealth, maxHealth);
        healthText.text = "Health: " + healthComponent.HealthValue.ToString();
    }

    protected override void Die() {
        SceneManager.LoadScene(3);
    }

    private void ShowGameOverScreen() {
        if(gameOverButtons != null) {
            gameOverButtons.SetActive(true);
            playButtons.SetActive(false);
        }
    }
}

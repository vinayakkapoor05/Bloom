// Consumer.cs
using UnityEngine;
using UnityEngine.UI;

public class Consumer : MonoBehaviour {
    public float health = 100f;
    public float maxHealth = 100f;
    public float healthRegenRate = 5f;
    public float regenDelay = 3f;
    public Slider healthBar;

    private float timeSinceLastDamage;
    // private bool isRegenerating = false;
    private GameSetup gameSetup;

    private void Start() {
        timeSinceLastDamage = regenDelay;
        gameSetup = FindObjectOfType<GameSetup>();
    }

    private void Update() {
        if (!GameManager.Instance.gameOver) {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= regenDelay && health < maxHealth) {
                health += healthRegenRate * Time.deltaTime;
                health = Mathf.Min(health, maxHealth);
                // isRegenerating = true;
            }

            healthBar.value = health / maxHealth;

            if (health <= 0) {
                if (gameSetup != null) {
                    gameSetup.HandleConsumerDestroyed();
                }
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        timeSinceLastDamage = 0f;
        // isRegenerating = false;
    }
}
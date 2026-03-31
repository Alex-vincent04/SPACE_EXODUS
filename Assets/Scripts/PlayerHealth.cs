using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth;
    public float regenAmount = 1f; // Amount of health regained
    public float regenInterval = 10f; // Time interval for health regeneration

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(RegenerateHealth()); // Start health regeneration
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent going below 0

        // Check if player health reaches 0 or below
        if (currentHealth <= 0)
        {
            Die();
            SceneManager.LoadScene(9);
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        gameObject.SetActive(false); // Disable player object
    }

    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval); // Wait for 10 seconds
            if (currentHealth < maxHealth)
            {
                currentHealth += regenAmount;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure it doesn't exceed max health
            }
        }
    }
}

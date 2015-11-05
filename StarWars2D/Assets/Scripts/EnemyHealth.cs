using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;                 // The player's health.
    public float hurtForce = 10f;               // The force with which the player is pushed when hurt.
    public float damageAmount = 200f;           // The amount of damage to take when enemies touch the player

    private SpriteRenderer healthBar;           // Reference to the sprite renderer of the health bar.
    private Vector3 healthScale;                // The local scale of the health bar initially (with full health).


    void Awake()
    {

        healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();

        // Getting the intial scale of the healthbar (whilst the player has full health).
        healthScale = healthBar.transform.localScale;

    }


    void TakeDamage(Transform enemy)
    {
 
        // Create a vector that's from the enemy to the player with an upwards boost.
        Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

        // Add a force to the player in the direction of the vector and multiply by the hurtForce.
        //GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
        //  transform.Translate(Vector2.up * 5);

        // Reduce the player's health by 10.
        health -= damageAmount;

        // Update what the health bar looks like.
        UpdateHealthBar();

 
    }


    public void UpdateHealthBar()
    {
        // Set the health bar's colour to proportion of the way between green and red based on the player's health.
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

        // Set the scale of the health bar to be proportional to the player's health.
        healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
    }
}

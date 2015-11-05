using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;                 // La vida del jugador
    public float hurtForce = 10f;               // La fuerza con la que es empujado
    public float damageAmount = 200f;           // La cantidad de daño que recibe

    private SpriteRenderer healthBar;           // Referencia al SpriteRenderer de la barra de vida
    private Vector3 healthScale;                // Escala de la barra de vida


    void Awake()
    {
        // Inicialización de los componentes
        healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;

    }


    void TakeDamage(Transform enemy)
    {
 
        // Se crea un vector para impulsar al jugador hacia arriba y ser repelido
        Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

        // Se reduce la vida del personaje
        health -= damageAmount;

        // Se actualiza la barra de vida
        UpdateHealthBar();

 
    }


    public void UpdateHealthBar()
    {
        // Se actualiza la barra de vida
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
        healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
    }
}

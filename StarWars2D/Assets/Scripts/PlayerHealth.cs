using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 10f;               // The force with which the player is pushed when hurt.
    public float bulletHurtForce = 50f;               // The force with which the player is pushed when hurt.
    public float damageAmount = 200f;			// The amount of damage to take when enemies touch the player

	private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	private float lastHitTime;					// The time at which the player was last hit.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private PlayerController playerControl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player
    public GameObject menuGameOver;             // Referencia a la pantalla de GameOver
    private GameObject rg;                      // Referencia al GameObject del Personaje
    private SpriteRenderer ren;                 // Referencia al SpriteRenderer del Personaje

    Rigidbody2D body;                           //Referencia al RigidBody2D del Personaje

    void Awake ()
	{
		// Inicialización de componentes
		playerControl = GetComponent<PlayerController>();
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		healthScale = healthBar.transform.localScale;
        rg = GameObject.Find("Hero");
        body = rg.GetComponent<Rigidbody2D>();
        ren = rg.GetComponent<SpriteRenderer>();

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        // Si el collider es un enemigo
		if(col.tag == "BattleDroid" || col.tag == "Droideka" || col.tag == "Soldier")
		{
            // Y no excede el tiempo máximo de golpe
            if (Time.time > lastHitTime + repeatDamagePeriod) 
            {
                // Y hay suficiente salud
                if (health - damageAmount > 0f)
				{
					// Se hade daño y se resetea el tiempo
					TakeDamage(col.transform); 
					lastHitTime = Time.time; 
				}
				// Si no se tiene suficiente salud
				else
				{
                    anim.SetTrigger("Dead");
                    health = 0;
                    UpdateHealthBar();
                    GetComponent<PlayerController>().enabled = false;

				}
			 }
		}
	}

    public void bulletImpact(Collider2D col)
    {
            // Si el impacto de bala no está dentro del periodo de invulnerabilidad
            if (Time.time > lastHitTime + repeatDamagePeriod)
            {
                // Y se tiene suficiente vida
                if (health - damageAmount > 0f)
                {
                   // Se hade daño y se resetea el tiempo
                    TakeDamage(col.transform);
                    lastHitTime = Time.time;
                }
				// Si no se tiene suficiente salud
                else
                {
                anim.SetTrigger("Dead");
                health = 0;
                UpdateHealthBar();

                GetComponent<PlayerController>().enabled = false;
                //Se activa la pantalla de GameOver
                menuGameOver.SetActive(true);

            }
        }
        
    }
    
    //Parpadeo del sprite
    IEnumerator Blink()
    {
        for (var n = 0; n < 5; n++)
        {
            ren.enabled = false;
            yield return new WaitForSeconds(.1f);
            ren.enabled = true;
            yield return new WaitForSeconds(.1f);

        }
    }

    
    void TakeDamage (Transform enemy)
	{

		// El personaje no puede saltar
		playerControl.jump = false;
        //Recibe fuerza si y solo si no es un bala láser
        if (enemy.tag != "LaserBullet")
            body.velocity = (Vector3) body.velocity + Vector3.up * 2;

        // Se reduce la vida del jugador
        health -= damageAmount;

		// Se actualiza la barra
		UpdateHealthBar();

        //Parpadeo del sprite
        StartCoroutine(Blink());

    }


    public void UpdateHealthBar ()
	{
        //Se actualiza la barra de salud
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}

    void destroyAnimator()
    {
        //Se deshabilita la animación
        anim.enabled = false;
    }


}

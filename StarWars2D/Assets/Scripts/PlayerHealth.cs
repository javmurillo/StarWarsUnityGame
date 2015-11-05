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

    private GameObject rg;
    private Renderer ren;

    Rigidbody2D body;
    Vector3 pushDir;
    float power = 2.0f;
    float minPower = 0.5f;
    float maxPower = 3.0f;

    void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerController>();
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
        rg = GameObject.Find("Hero");
        body = rg.GetComponent<Rigidbody2D>();
        ren = rg.GetComponent<Renderer>();

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("nombre: " + col.gameObject.name);
		// If the colliding gameobject is an Enemy...
		if(col.tag == "BattleDroid" || col.tag == "Droideka")
		{
            // ... and if the time exceeds the time of the last hit plus the time between hits...
            //if (Time.time > lastHitTime + repeatDamagePeriod) 
            if (true)
            {
                // ... and if the player still has health...
                if (health > 0f)
				{
					// ... take damage and reset the lastHitTime.
					TakeDamage(col.transform); 
					lastHitTime = Time.time; 
				}
				// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
				else
				{
					// ... disable user Player Control script
					GetComponent<PlayerController>().enabled = false;

					// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
					//GetComponentInChildren<Gun>().enabled = false;

					// ... Trigger the 'Die' animation state
					anim.SetTrigger("Die");
				}
			 }
		}
	}

    public void serDisparado(Collider2D col)
    {
        // ... and if the time exceeds the time of the last hit plus the time between hits...
       // if (Time.time > lastHitTime + repeatDamagePeriod)
        if (true) {
            // ... and if the player still has health...
            if (health > 0f)
            {
                // ... take damage and reset the lastHitTime.
                TakeDamage(col.transform);
                lastHitTime = Time.time;
            }
            // If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
            else
            {

                // ... disable user Player Control script
                GetComponent<PlayerController>().enabled = false;

                // ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
                //GetComponentInChildren<Gun>().enabled = false;

                // ... Trigger the 'Die' animation state
                anim.SetTrigger("Die");
            }
        }
    }


    void TakeDamage (Transform enemy)
	{


        Debug.Log("TAAAAG: " + enemy.tag);
		// Make sure the player can't jump.
		playerControl.jump = false;

        if (enemy.tag != "LaserBullet")
            body.velocity = (Vector3) body.velocity + Vector3.up * 2;


        /* if (enemy.tag != "LaserBullet") {
              float Xdif = transform.position.x - enemy.position.x;
              float Ydif = transform.position.y - enemy.position.y;
              transform.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Xdif, Ydif).normalized * 500); */




        // Add a force to the player in the direction of the vector and multiply by the hurtForce.
        //GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
        //  transform.Translate(Vector2.up * 5);

        // Reduce the player's health by 10.
        health -= damageAmount;

		// Update what the health bar looks like.
		UpdateHealthBar();

		// Play a random clip of the player getting hurt.
		int i = Random.Range (0, ouchClips.Length);
		//AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
	}


	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}


}

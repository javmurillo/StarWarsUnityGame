using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody;              // Referencia al componente Rigidbody2D del personaje
    private Animator anim;                  // Referencia al componente Animator del personaje
    private Transform groundCheck;          // Referencia para saber si el personaje toca o no suelo.
    private AudioSource[] audioSources;     // Referencia a todas las componentes de audio asociadas a este personaje

    [HideInInspector]
	public bool facingRight = true;         // Para determinar hacia dónde está mirando el personaje

    public bool jump = false;              // Para determinar si el personaje ha de saltar
    private bool jumpAttack = false;        // Para determinar si el personaje ha de realizar un ataque en salto.
    private bool attack = false;            // Para determinar si el personaje ha de realizar un ataque primario.
    private bool attack2 = false;           // Para determinar si el personaje ha de realizar un ataque secundario
    private bool jumped = false;            // Para determinar si el personaje ha saltado
    private bool hit = false;            // Para determinar si el personaje ha saltado


    public float maxSpeed = 7f;             // Para determinar la velocidad de movimiento del personaje
    public float jumpForce = 400f;          // Para determinar la fuerza de salto del personaje
    public float timeBetweenAttack = 0.1f;  // Tiempo de espera entre ataque y ataque

    private bool grounded = false;          // Para determinar si el personaje toca o no suelo

    private float jumpTime, jumpDelay = .5f;    // Tiempo de espera entre salto y salto
    private float timestamp;                // Referencia de tiempo para la espera entre ataque y ataque
	

	void Awake()
	{
        // Inicialización de los componentes
        groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        audioSources = GetComponents<AudioSource>();
	}
	
	
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);

        // El personaje estará en el suelo si una línea de casteo al objeto groundCheck impacta con la capa "Ground"
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		
        // Si se pulsa el botón de saltar y el jugador está en el suelo, entonces salta.
		if (Input.GetButtonDown ("Jump") && grounded && !jumped) {
			jump = true;

        }

        // Si se pulsa el botón de ataque primario y el jugador está en el suelo, entonces ataque primario.
        if (Time.time >= timestamp && Input.GetButtonDown ("Fire1") && grounded) {
			attack = true;
			timestamp = Time.time + timeBetweenAttack;
		}

        // Si se pulsa el botón de ataque primario y el jugador no está en el suelo, entonces ataca en el aire.
        if (Time.time >= timestamp && Input.GetButtonDown ("Fire1") && !grounded) {
			jumpAttack = true;
			timestamp = Time.time + timeBetweenAttack;
		}

        // Si se pulsa el botón de ataque secundario y el jugador está en el suelo, entonces ataque secundario.
        if (Time.time >= timestamp && Input.GetButtonDown ("Fire2") && grounded) {
			attack2 = true;
			timestamp = Time.time + timeBetweenAttack;
		}
	}
	
	void FixedUpdate()
	{

        // Se cachea el input horizontal.
		float h = Input.GetAxis("Horizontal");
		
		// Se actualiza el parámetro velocidad del componente Animator de acuerdo a la entrada horizontal.
		anim.SetFloat("Speed", Mathf.Abs(h));

        /*
        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            // ... add a force to the player.
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * 365);

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            */

        // Se asigna una velocidad al rigidBody dependiendo del parámetro anterior y la velocidad máxima.

        rbody.velocity = new Vector2(h * maxSpeed, rbody.velocity.y);

        // Si se está moviendo el personaje a la derecha pero mira a la izquierda
        // o se está moviendo a la izquierda pero mira a la derecha, entonces flip.
        if ((h > 0 && !facingRight) || (h < 0 && facingRight)) {
            Flip();
        }

		// Si el personahe ha de saltar
		if (jump)
		{
			// Se dispara el trigger de la animación de salto
			anim.SetTrigger("Jump");

            // Se añade una fuerza vertical [jumpForce] al personaje
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			jumpTime = jumpDelay;

            // Asegurar que el personaje no puede saltar hasta que no se cumplan las condiciones impuestas en el Update
			jump = false;
            jumped = true;

        }

        // Si el personaje ha de realizar un ataque en vuelo
        if (jumpAttack) {
            // Se reproduce el sonido del ataque.
            audioSources[1].Play();

            // Se dispara el trigger de la animación de salto en vuelo.
            anim.SetTrigger("JumpAttack");
			jumpAttack = false;
		}
		
		jumpTime -= Time.deltaTime;

        // Si el personaje ha caido tras el salto salimos de la animación de salto disparando el trigger "Land"
        //Debug.Log("Variables: " + jumpTime + "|" + grounded + "|" + jumped + "|" + jump);
        if (jumpTime <= 0 && grounded && jumped) {
			anim.SetTrigger("Land");
			jumped = false;
		}

        // Si el personahe ha de realizar un ataque primario
        if (attack) {
            // Se reproduce el sonido del ataque.
            audioSources[0].Play();

            // Se dispara el trigger de la animación de ataque primario
            anim.SetTrigger("Attack1");
			attack = false;
		}

        // Si el personahe ha de realizar un ataque secundario
        if (attack2) {
            // Se reproduce el sonido del ataque.
            audioSources[1].Play();

            // Se dispara el trigger de la animación de ataque secundario
            anim.SetTrigger("Attack2");
			attack2 = false;
		}
	}
	
	void Flip() {
        // El personaje pasa a mirar al lado contrario
        facingRight = !facingRight;

        // La componente X del vector pasa a ser negativa o positiva.
        Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}



}

using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour
{
    [HideInInspector]
    public Transform target;                // Para determinar el objetivo hacia el cual moverse
    private Rigidbody2D myRigidbody;        // Referencia al componente Rigidbody2D del personaje
    private Animator anim;                  // Referencia al componente Animator del personaje
    private AudioSource[] audioSources;     // Referencia a todas las componentes de audio asociadas a este personaje
    private Score score;                  // Reference to the Score script.

    public GameObject pointsUI;   // A prefab of 100 that appears when the enemy dies.
    private GameObject bulletSpawner;       // Objecto cuya posición será el inicio de la invocación del proyectil
    private float xc = -1.1f;                  // Valor de ajuste del invocador de proyectiles en el eje X                   
    private float yc = -0.3f;               // Valor de ajuste del invocador de proyectiles en el eje Y     

    public Rigidbody2D bullet;              // Prefab del proyectil (Bullet)

    [HideInInspector]
    public bool facingRight = true;        // Para determinar hacia dónde está mirando el personaje
    private bool dead = false;              // Para determinar si el personaje ha muerto o no

    public int maxSpeed;                    // Determina la velocidad de movimiento del personaje
    public float attackDistance;            // Distancia a partir de la cual el personaje atacará
    public float bulletSpeed = 10f;         // Para determinar la velocidad del proyectil
    public float timeBetweenShots = 1f;     // Tiempo de espera entre proyectil y proyectil
    public float HP = 100f;                      // Cuántas veces el personaje puede ser golpeado sin morir
    public int points;
    private float timestamp;                // Referencia de tiempo para la espera entre proyectil y proyectil

    private SpriteRenderer healthBar;           // Reference to the sprite renderer of the health bar.
    private Vector3 healthScale;                // The local scale of the health bar initially (with full health).
    private Transform barrachild;       // Referencia a la barra de vida
    public GameObject sombra;               // Referencia a la sombra debajo del personaje
    private GameObject go;               // GameObject para heredar y desheredar


    void Start()
    {
        // Se asocia como objetivo el GameObject cuya etiquete es "Hero"
        target = GameObject.Find("Hero").transform;

        // Se crea un nuevo GameObject cuya función será determinar la posición de invocación de los proyectiles
        bulletSpawner = new GameObject();
        bulletSpawner.transform.position = new Vector3(gameObject.transform.position.x - xc,
            gameObject.transform.position.y - yc, gameObject.transform.position.z);
        // Se determina que su padre es este mismo objeto (this) para moverse en conjunto a través de la escena.
        bulletSpawner.transform.parent = transform;

        // Inicialización de los componentes
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSources = GetComponents<AudioSource>();
        score = GameObject.Find("Score").GetComponent<Score>();

        barrachild = transform.FindChild("ui_healthDisplayEnemy");
        healthBar = barrachild.FindChild("HealthBarEnemy").GetComponent<SpriteRenderer>();

        // Se coge la referencia de la barra de vida estando ésta llena
        healthScale = healthBar.transform.localScale;

        //Se deshereda para que no rote la barra de vida a la vez que el personaje
        transform.DetachChildren();
        bulletSpawner.transform.parent = transform;

        // Se hereda la sombra para que acompañe al personaje
        go = Instantiate(sombra, new Vector3(transform.position.x - 0.3f, transform.position.y - 1.3f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        go.transform.parent = this.transform;

    }

    void Update()
    {
        // Si tiene 0 puntos o menos y no está muerto
        if (HP <= 0f && !dead)
            Death();
        // Si el personaje no ha muerto
        if (!dead)
        {
            // Se calcula la distancia entre el objetivo y este personaje
            float difPosition = target.position.x - transform.position.x;
            // Si se encuentra en el intervalor [-attackDistance, attackDistance], el personaje dispara.
            if (difPosition > -attackDistance && difPosition < attackDistance)
            {
                // Se dispara el trigger de la animación de disparo
                anim.SetTrigger("Shoot");
            }
            // Si no, camina hacia el objetivo.
            else
            {
                // Se dispara el trigger de la animación de caminar
                anim.SetTrigger("Walk");
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(target.position.x, transform.position.y), maxSpeed * Time.deltaTime);
            }
        }
        // Se cachea la posición en el eje X del heroe y el personaje.
        float heroX = target.position.x;
        float droidX = transform.position.x;

        // Si el héroe está a la derecha y el personaje mira hacia la izquierda, o
        // el héroe está a la izquierda y el personaje mira hacia la derecha, entonces flip.
        if ((!dead && heroX > droidX && !facingRight) || heroX < droidX && facingRight)
        {
            Flip();
        }
    }


    void Shoot()
    {
        // Se reproduce el audio de disparo
        audioSources[1].Play();
        // Si el personaje está mirando hacia la derecha
        if (facingRight)
        {
            // Se instancia el proyectil a partir de la posición de [bulletSpawner] con una velocidad de [rocketSpeed]
            Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.transform.position,
                                                     Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(bulletSpeed, 0);
        }
        // De lo contrario
        else
        {
            // Se instancia el proyectil a partir de la posición de [bulletSpawner] con una velocidad de -[rocketSpeed]
            Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(-bulletSpeed, 0);
        }


    }

    void Flip()
    {
        // El personaje pasa a mirar al lado contrario
        facingRight = !facingRight;

        // La componente X del vector pasa a ser negativa o positiva.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Death()
    {
        //Se inicia la animación de muerte
        anim.SetTrigger("Dead");

        // Se reproduce el audio de muerte
        audioSources[0].Play();

        // Se declara la muerte del personaje, desactivando sus componentes
        dead = true;
        Destroy(GetComponent<BoxCollider2D>());

        // Se incrementa la puntuación
        score.score += points;

        // Se instancia la animación de puntos
        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;

        Instantiate(pointsUI, scorePos, Quaternion.identity);
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - 0.25f, go.transform.position.z);
        go.transform.localScale = new Vector3(2.160157f, 0.8064684f, 0.8064684f);

        // Este personaje se destruirá en dos segundos.
        Invoke("Destroy", 2f);

    }

    public void Hurt()
    {
        // Se reduce la vida
        HP -= 50f;
        if (HP <= 0) { HP = 0; }
        UpdateHealthBar();
    }


    public void UpdateHealthBar()
    {
        //Se actualiza la barra de vida
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - HP * 0.01f);
        healthBar.transform.localScale = new Vector3(healthScale.x * HP * 0.01f, 1, 1);
    }

    void Destroy()
    {
        // Destruye el gameObject y su hijo
        Destroy(gameObject);
        Destroy(barrachild.gameObject);

    }

}
using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public GameObject bulletExplosion;		// Prefab de la explosión
	private Rigidbody2D myRigidbody;        // Referencia al rigidbody2D
    private bool reflected = false;         // Si la bala ha sido o no reflectada

	AudioSource[] allMyAudioSources;        // Componentes de audio
    private PlayerController playerScript;  // Referencia al script del jugador


    void Start () 
	{
		// Destruye si la bala en 5 segundos si no ha sido destruida
		Destroy(gameObject, 5);
        // Inicialización de componentes
		myRigidbody = GetComponent<Rigidbody2D> ();
		allMyAudioSources = GetComponents<AudioSource>();
        GameObject thePlayer = GameObject.Find("Hero");
        playerScript = thePlayer.GetComponent<PlayerController>();


    }


    void OnExplode()
	{
        // Se da una rotación aleatoria
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
		
		// Se instancia la explosion
		Instantiate(bulletExplosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
        //Si colisiona con el jugador
        if (col.tag == "Player") {
            col.GetComponent<PlayerHealth>().bulletImpact(this.GetComponent<Collider2D>());
            OnExplode();
            Destroy(gameObject);

        }
        //Si colisiona con el battledroid
        else if (col.tag == "BattleDroid")
        {
            if (reflected)
            {
                col.GetComponent<BattleDroid>().Hurt();
                OnExplode();
                Destroy(gameObject);
            }

        }
        //Si colisiona con el soldado
        else if (col.tag == "Soldier")
        {
            if (reflected)
            {
                col.GetComponent<Soldier>().Hurt();
                OnExplode();
                Destroy(gameObject);
            }

        }
        //Si colisiona con el Droideka
        else if (col.tag == "Droideka")
        {
            if (reflected)
            {
                OnExplode();
                Destroy(gameObject);
            }

        }
    }

    public void setReflected(bool refl)
    {
        reflected = refl;
    }

}

using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public GameObject bulletExplosion;		// Prefab of explosion effect.
	private Rigidbody2D myRigidbody;
    private bool reflected = false;

	AudioSource[] allMyAudioSources;
    private PlayerController playerScript;


    void Start () 
	{
		// Destroy the rocket after 5 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 5);
		myRigidbody = GetComponent<Rigidbody2D> ();
		allMyAudioSources = GetComponents<AudioSource>();
        GameObject thePlayer = GameObject.Find("Hero");
        playerScript = thePlayer.GetComponent<PlayerController>();


    }


    void OnExplode()
	{
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
		
		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(bulletExplosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
        if (col.tag == "Player") {
            col.GetComponent<PlayerHealth>().serDisparado(this.GetComponent<Collider2D>());
            OnExplode();
            Destroy(gameObject);

        }

        else if (col.tag == "BattleDroid")
        {
            if (reflected)
            {
                col.GetComponent<BattleDroid>().Hurt();
                OnExplode();
                Destroy(gameObject);
            }

        }
        else if (col.tag == "Droideka")
        {
            if (reflected)
            {
                OnExplode();
                Destroy(gameObject);
            }

        }

        // Otherwise if the player manages to shoot himself...
        /*else
		{
			// Instantiate the explosion and destroy the rocket.
			OnExplode();
			Destroy (gameObject);
		}*/
    }

    public void setReflected(bool refl)
    {
        reflected = refl;
    }

    /*
	IEnumerator MyFunction(Collider2D col, float delayTime) {
		yield return new WaitForSeconds(delayTime);
		Destroy (gameObject);

	}*/
}

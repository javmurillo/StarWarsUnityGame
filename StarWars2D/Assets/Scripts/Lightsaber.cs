using UnityEngine;
using System.Collections;

public class Lightsaber : MonoBehaviour {

    private PlayerController playerScript;      // Referencia al script PlayerController
    AudioSource[] allMyAudioSources;            // Fuentes de audio
    private GameObject thePlayer;               // Referencia al gameobject del jugador

    void Start ()
    {
        // Se inicializan los componentes
        thePlayer = GameObject.Find("Hero");
        playerScript = thePlayer.GetComponent<PlayerController>();
        allMyAudioSources = GetComponents<AudioSource>();


    }


    void OnTriggerEnter2D(Collider2D col)
    {
        //Si es un Battledroid
        if (col.tag == "BattleDroid")
        {
            col.GetComponent<BattleDroid>().Hurt();
        }
        //Si es un Droideka
        else if (col.tag == "Droideka")
        {
            col.GetComponent<Droideka>().Hurt();
        }
        //Si es un Soldado
        else if (col.tag == "Soldier")
        {
            col.GetComponent<Soldier>().Hurt();
        }
        //Si es un disparo
        else if (col.tag == "LaserBullet")
        {
            //Se coge la referencia al RigidBody2D del disparo
            Rigidbody2D colrb = col.gameObject.GetComponent<Rigidbody2D>();
            col.gameObject.GetComponent<Bullet>().setReflected(true);
            allMyAudioSources[0].Play();
            //Dependiendo de la dirección donde se mire, la bala sale reflectada
            if (playerScript.facingRight)
            {
                colrb.velocity = new Vector2(10f, colrb.velocity.y);
            }
            else colrb.velocity = new Vector2(-10f, colrb.velocity.y);

        }


    }
}

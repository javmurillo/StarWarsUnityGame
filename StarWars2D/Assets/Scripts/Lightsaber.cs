using UnityEngine;
using System.Collections;

public class Lightsaber : MonoBehaviour {

    private PlayerController playerScript;
    AudioSource[] allMyAudioSources;


    // Use this for initialization
    void Start () {
        GameObject thePlayer = GameObject.Find("Hero");
        playerScript = thePlayer.GetComponent<PlayerController>();
        allMyAudioSources = GetComponents<AudioSource>();


    }

    // Update is called once per frame
    void Update () {
	
	}


    void OnCollisionEnter2D(Collision2D col) {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("entra triger col");

        if (col.tag == "BattleDroid")
        {
            Debug.Log("da al droid");
            col.GetComponent<BattleDroid>().Hurt();
        }
        else if (col.tag == "BdroidBullet")
        {
            Rigidbody2D colrb = col.gameObject.GetComponent<Rigidbody2D>();
            col.gameObject.GetComponent<Bullet>().setReflected(true);
            allMyAudioSources[0].Play();
            if (playerScript.facingRight)
            {
                colrb.velocity = new Vector2(10f, colrb.velocity.y);
            }
            else colrb.velocity = new Vector2(-10f, colrb.velocity.y);
        }

    }
}

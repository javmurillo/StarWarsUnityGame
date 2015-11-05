using UnityEngine;
using System.Collections;

public class Sombra : MonoBehaviour
{
    public Vector3 offset;          // Offset de la sombra para seguir al jugador

    private Transform player;       // Referencia al jugador


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, transform.position.y, player.position.z + offset.z);
            
    }
}

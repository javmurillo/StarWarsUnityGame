using UnityEngine;
using System.Collections;

public class SombraEnemigo : MonoBehaviour
{
    public Vector3 offset;          // Offset de la sombra para seguir al personaje

    private Transform player;       // Referencia al jugador


    void Awake()
    {
        player = transform.parent;
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, transform.position.y, player.position.z + offset.z);

    }
}

using UnityEngine;
using System.Collections;

public class FollowEnemy: MonoBehaviour
{
    public Vector3 offset;          // Offset de la barra de vida para seguir al jugador

    private Transform player;       // Referencia al Transform del jugador


    void Awake()
    {
        player = transform.parent;
    }

    void Update()
    {
        transform.position = player.position + offset;
    }
}

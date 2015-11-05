using UnityEngine;
using System.Collections;

public class Sombra : MonoBehaviour
{
    public Vector3 offset;          // The offset at which the Health Bar follows the player.

    private Transform player;       // Reference to the player.


    void Awake()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Set the position to the player's position with the offset.
        transform.position = new Vector3(player.position.x + offset.x, transform.position.y, player.position.z + offset.z);
            
    }
}

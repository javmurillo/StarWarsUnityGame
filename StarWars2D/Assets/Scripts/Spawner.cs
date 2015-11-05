using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemy;              // El prefab enemigo a ser invocado
    public float spawnTime = 3f;            // Tiempo de spawneo
    public Transform[] spawnPoints;         // Array con los diversos puntos de spawn
    private float health;                   // Referencia a la vida del jugador

    void Start()
    {
        GameObject thePlayer = GameObject.Find("Hero");
        health = thePlayer.GetComponent<PlayerHealth>().health;

        // Se llama a la función Spawn tras el delay impuesto
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
        // Si el jugador no tiene vida
        if (health <= 0f)
        {
            // ... se sale de la función
            return;
        }

        // Genera un índice aleatorio para el spawn y el enemigo
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemy.Length);

        // Se crea una instancia del enemigo
        Instantiate(enemy[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
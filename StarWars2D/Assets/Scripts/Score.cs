using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;					// La puntuación del jugador

	private PlayerController playerControl;	// Referencia al script PlayerController
	private int previousScore = 0;			// La puntuación en el anterior frame


	void Awake ()
	{
		// Inicialización de componentes
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}


	void Update ()
	{
		// Se establece el score
		GetComponent<GUIText>().text = "Score: " + score;
		previousScore = score;
	}

}

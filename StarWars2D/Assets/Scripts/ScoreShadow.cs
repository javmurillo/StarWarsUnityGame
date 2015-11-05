using UnityEngine;
using System.Collections;

public class ScoreShadow : MonoBehaviour
{
	public GameObject guiCopy;		// Referencia a copia del gui a sombrear


	void Awake()
	{
		// Se establece la posición un poco por debajo
		Vector3 behindPos = transform.position;
		behindPos = new Vector3(guiCopy.transform.position.x, guiCopy.transform.position.y-0.005f, guiCopy.transform.position.z-1);
		transform.position = behindPos;
	}


	void Update ()
	{
		// Se establece el texto
		GetComponent<GUIText>().text = guiCopy.GetComponent<GUIText>().text;
	}
}

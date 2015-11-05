using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuGameOver : MonoBehaviour
{


    public void StartLevel()

    {
        Application.LoadLevel(1);

    }

    public void ToMenu()

    {
        Application.LoadLevel(0);

    }

}
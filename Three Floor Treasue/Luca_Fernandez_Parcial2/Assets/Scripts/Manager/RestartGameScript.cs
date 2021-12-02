using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGameScript : MonoBehaviour
{
    public void RestartGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().RestartGame(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().loseLevel);
    }
}

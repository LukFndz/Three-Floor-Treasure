using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTriggerCheckScript : MonoBehaviour
{
    public int level;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().level++;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeLevel(level);
            collision.GetComponent<Player_Controller>().moveSpeed = 0;
        }
    }
}

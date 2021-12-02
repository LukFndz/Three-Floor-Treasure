using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikesScript : MonoBehaviour
{
    public bool isOff = false;
    public GameManager gameManager;
    public Sprite[] pikeStates;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void TurnOff()
    {
        switch (gameManager.level)
        {
            case 1:
                if (gameManager.colorPattron.Count == 4 && gameManager.enemyList.Count == 0)
                {
                    isOff = true;
                    gameObject.GetComponent<SpriteRenderer>().sprite = pikeStates[1];
                }
                break;
            case 2:
                isOff = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = pikeStates[1];
                break;

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (isOff)
            {
                case false:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().UpdateHealth(-GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().health);
                    break;
            }
        }
    }
}

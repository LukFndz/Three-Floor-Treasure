using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public string color;
    public Sprite[] btnStates;
    private GameManager gameManager;
    private SpriteRenderer spriteRen;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        spriteRen = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && spriteRen.sprite == btnStates[0])
        {
            spriteRen.sprite = btnStates[1];
            if (gameManager.CheckButtons(color))
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Button"))
                {
                    g.GetComponent<SpriteRenderer>().sprite = g.GetComponent<ButtonScript>().btnStates[0];
                }
            }
        }
        if (gameManager.colorPattron.Count == 4)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Pike"))
            {
                g.GetComponent<PikesScript>().TurnOff();
            }
        }
    }
}

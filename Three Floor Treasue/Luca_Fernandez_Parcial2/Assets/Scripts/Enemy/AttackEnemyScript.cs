using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyScript : MonoBehaviour
{
    public bool hittingPlayer = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hittingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hittingPlayer = false;
        }
    }

}

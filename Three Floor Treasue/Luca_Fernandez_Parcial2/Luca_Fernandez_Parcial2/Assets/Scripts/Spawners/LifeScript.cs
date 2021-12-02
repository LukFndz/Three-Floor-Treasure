using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeScript : MonoBehaviour
{

    public float lifeRecov;
    void Update()
    {
        transform.Rotate(new Vector3(0, 5, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player_Controller>().UpdateHealth(lifeRecov);
            Destroy(gameObject);
        }
    }
}

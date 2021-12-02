using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLifeScript : MonoBehaviour
{
    public Transform Spawn_1;
    public Transform Spawn_2;
    public Transform Spawn_3;
    public Transform Spawn_4;
    public GameObject life;

    [SerializeField]
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    Instantiate(life, Spawn_1);
                    break;
                case 1:
                    Instantiate(life, Spawn_2);
                    break;
                case 2:
                    Instantiate(life, Spawn_3);
                    break;
                case 3:
                    Instantiate(life, Spawn_4);
                    break;
            }
            timer = 0;
        }
    }
}

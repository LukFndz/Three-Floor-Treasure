using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnOff : MonoBehaviour
{
    public GameObject obj_active;
    public GameObject obj_disable;

    public void OnClick()
    {
        obj_active.SetActive(true);
        obj_disable.SetActive(false);
    }
}

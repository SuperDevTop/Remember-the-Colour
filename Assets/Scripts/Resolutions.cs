using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolutions : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    
    void Start()
    {
        
    }

    void Update()
    {
        mainUI.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1440f, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class fullscreen : MonoBehaviour
{
    void Awake()
    {   
        //Time in system
        /*
        string time = System.DateTime.Now.ToString();
        Debug.Log(System.DateTime.Now);
        */
        //If running platform is Android, set to non-fullscreen; 
        if (Application.platform ==  RuntimePlatform.Android)
        {
            Screen.fullScreen = false;
        }
    }
}

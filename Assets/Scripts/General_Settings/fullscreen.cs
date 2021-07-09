using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullscreen : MonoBehaviour
{
    void Start()
    {   
        //If running platform is Android, set to non-fullscreen; 
        if (Application.platform ==  RuntimePlatform.Android)
        {
            Screen.fullScreen = false;
        }
    }
}

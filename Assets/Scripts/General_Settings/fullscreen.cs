using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullscreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        if (Application.platform ==  RuntimePlatform.Android)
        {
            //ApplicationChrome.statusBarState = ApplAicationChrome.States.Visible;
            //ApplicationChrome.navigationBarState = ApplicationChrome.States.VisibleOverContent;     
            Screen.fullScreen = false;
            //ApplicationChrome.statusBarState = ApplicationChrome.States.Visible;
            //ApplicationChrome.statusBarState = ApplicationChrome.States.
        }
    }
}

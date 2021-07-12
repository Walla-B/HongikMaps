using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private int escapecount = 0;
    [SerializeField]
    private GameObject quitnotification , uicanvas;
   void Update()
    {
        /*
        for testing in Playmode

        if (Input.GetKeyDown(KeyCode.R)){
            escapecount++;
            if (escapecount > 1) {
                Application.Quit();
            }

            GameObject notification = Instantiate(quitnotification,new Vector3(Screen.width/2,Screen.height/12,0f),Quaternion.identity,uicanvas.transform);
            GameObject.Destroy(notification,3f);

            StartCoroutine(ClickTime());
        }
        */
       if (Application.platform == RuntimePlatform.Android) {
           if (Input.GetKeyDown(KeyCode.Escape)){
                escapecount++;

                StartCoroutine(ClickTime());
                if (escapecount > 1) {
                    Application.Quit();
                }

                GameObject notification = Instantiate(quitnotification,new Vector3(Screen.width/2,Screen.height/12,0f),Quaternion.identity,uicanvas.transform);
                GameObject.Destroy(notification,4f);
           }

       }
       else if (Application.platform == RuntimePlatform.IPhonePlayer) {
           //TODO:
           if (Input.GetKeyDown(KeyCode.Escape)) {
               Application.Quit();
           }
       }
    }
    private IEnumerator ClickTime() {
        yield return new WaitForSeconds(4f);
        escapecount = 0;
    }
}

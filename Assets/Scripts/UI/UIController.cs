using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

public class UIController : MonoBehaviour
{

    // if uiDepth is zero, it means canvas status is in map mode.
    // if one, status can be in searchmode, sidepanelmode, information mode.
    // if two, status can be layer under the uiDepth 1
    [SerializeField]
    private int uiDepth = 0;
    private int escapecount = 0;
    [SerializeField]
    private GameObject quitnotification , uicanvas;

    [System.Serializable]
    public class MyEventType : UnityEvent { }

    public MyEventType D1Event, D2Event_example;
    private Stack<MyEventType> EventStack;

    
    [SerializeField]
    private TextMeshProUGUI mode;
    void Update()
    {
        //mode.text = uiDepth.ToString();
        // for testing in Playmode

       if (true) {

           if (uiDepth == 0 && Input.GetKeyDown(KeyCode.Escape)) {
               escapecount++;

               StartCoroutine(ClickTime());
               if (escapecount > 1) {
                   Application.Quit();
               }
                GameObject notification = Instantiate(quitnotification,new Vector3(Screen.width/2,Screen.height/12,0f),Quaternion.identity,uicanvas.transform);
                GameObject.Destroy(notification,4f);
           }
           else if (uiDepth == 1 && Input.GetKeyDown(KeyCode.Escape)){
               D1Event.Invoke();
               DecreaseUIDepth();
           }

       }

        //If platform it Android and Escape button is pressed
       if (Application.platform == RuntimePlatform.Android) {

           if (uiDepth == 0 && Input.GetKeyDown(KeyCode.Escape)) {
               escapecount++;

               StartCoroutine(ClickTime());
               if (escapecount > 1) {
                   Application.Quit();
               }
                GameObject notification = Instantiate(quitnotification,new Vector3(Screen.width/2,Screen.height/12,0f),Quaternion.identity,uicanvas.transform);
                GameObject.Destroy(notification,4f);
           }
           else if (uiDepth == 1 && Input.GetKeyDown(KeyCode.Escape)){
               D1Event.Invoke();
               DecreaseUIDepth();
           }

       }
       else if (Application.platform == RuntimePlatform.IPhonePlayer) {
           //TODO:
           if (Input.GetKeyDown(KeyCode.Escape)) {
               Application.Quit();
           }
       }
    }

    // pretty sure there is a better way of doing this
    // To prevent "scrolling", need to refactor ScrollandPinch.cs , Seperate "Scrolling" funciton.

    public void SetMovableFalse(){
        ScrollAndPinch.movable = false;
    }
    public void SetMovableTrue(){
        ScrollAndPinch.movable = true;
    }



    public void UIDepthtoZero(){
        uiDepth = 0;
    }
    public void UIDepthtoOne(){
        uiDepth = 1;
    }
    public void UIDepthtoTwo(){
        uiDepth = 2;
    }
    
    public void IncreaseUIDepth(){
        uiDepth++;
    }

    public void DecreaseUIDepth(){
        if (uiDepth >= 1) {
            uiDepth--;
        }
    }

    private IEnumerator ClickTime() {
        yield return new WaitForSeconds(4f);
        escapecount = 0;
    }
}

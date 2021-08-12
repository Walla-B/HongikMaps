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
    private int escapecount = 0;
    private bool allowinput = true;
    [SerializeField]
    private GameObject quitnotification , popUpParentCanvas;

    [System.Serializable]
    public class MyEventType : UnityEvent { }


    [SerializeField]
    private MyEventType Close_SearchPopUpActivity, Close_TextAutoCompleteActivity,
    Close_PathSearchPopUpActivity, Close_PathModeActivity, Close_LeftSidePanelActivity;

    private Stack<MyEventType> eventStack = new Stack<MyEventType>();


    /////Debug Objects/////
    private List<string> namestring = new List<string>();

    ///////////////////////
    void Update()
    {   
        //mode.text = uiDepth.ToString();
        // for testing in Playmode
        if (Input.GetKeyDown(KeyCode.Escape) && allowinput == true) {

            allowinput = false;
            StartCoroutine(InputWait());

            if (eventStack.Count == 0) {

                escapecount++;

                StartCoroutine(ClickTime());
                if (escapecount > 1) {
                    Application.Quit();
                }
                GameObject notification = Instantiate(quitnotification,new Vector3(Screen.width/2,Screen.height/12,0f),Quaternion.identity,popUpParentCanvas.transform);
                GameObject.Destroy(notification,3f);
            }
            else if (eventStack.Count >= 0) {
                PopStack_CloseActivities();
            }
        }



    /*
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
    */

        //If platform it Android and Escape button is pressed
       if (Application.platform == RuntimePlatform.Android) {

        if (Input.GetKeyDown(KeyCode.Escape) && allowinput == true) {

            allowinput = false;
            StartCoroutine(InputWait());

            if (eventStack.Count == 0) {

                escapecount++;

                StartCoroutine(ClickTime());
                if (escapecount > 1) {
                    Application.Quit();
                }
                GameObject notification = Instantiate(quitnotification,new Vector3(Screen.width/2,Screen.height/12,0f),Quaternion.identity,popUpParentCanvas.transform);
                GameObject.Destroy(notification,3f);
            }
            else if (eventStack.Count != 0) {
                PopStack_CloseActivities();
            }
        }
    }
       else if (Application.platform == RuntimePlatform.IPhonePlayer) {
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

    public void AddStack_Close_SearchPopUp() {
        eventStack.Push(Close_SearchPopUpActivity);
        
        ///// Debug Obj /////
        namestring.Add("SearchPopUp");
        PrintStack();
        /////////////////////
    }

    public void Addstack_Close_TextAutoComplete() {
        eventStack.Push(Close_TextAutoCompleteActivity);

        ///// Debug Obj /////
        namestring.Add("TextAutoComplete");
        PrintStack();
        /////////////////////
    }

    public void Addstack_Close_PathSearchPopUp() {
        eventStack.Push(Close_PathSearchPopUpActivity);

        ///// Debug Obj /////
        namestring.Add("PathSearch");
        PrintStack();
        /////////////////////
    }

    public void Addstack_Close_PathMode() {
        eventStack.Push(Close_PathModeActivity);

        ///// Debug Obj /////
        namestring.Add("PathMode");
        PrintStack();
        /////////////////////
    }

    public void Addstack_Close_LeftSidePanel() {
        eventStack.Push(Close_LeftSidePanelActivity);

        ///// Debug Obj /////
        namestring.Add("SidePanel");
        PrintStack();
        /////////////////////
    }

    // Pop Activities Pushed before and invoke Closing event
    // Check if stack is empty before Invoking
    public void PopStack_CloseActivities() {
        if (eventStack.Count != 0) {
            MyEventType myevent = eventStack.Pop();
            myevent.Invoke();
        }

        ///// Debug Obj /////
        namestring.Remove(namestring[namestring.Count-1]); 
        PrintStack();
        /////////////////////
    }

    // Same but twice. 
    public void PopStack_CloseActivities_twice() {
        if (eventStack.Count >= 2) {
            MyEventType myevent1 = eventStack.Pop();
            MyEventType myevent2 = eventStack.Pop();
            myevent1.Invoke();
            myevent2.Invoke();
        }
    }


    /////Debug Obj//////
    public void PrintStack() {
        string str = "";
        for (int i = 0; i < namestring.Count; i++) {
            str += "  >>  " + namestring[i];
        }
        Debug.Log(str);
    }

    ////////////////////

    private IEnumerator ClickTime() {
        yield return new WaitForSeconds(3f);
        escapecount = 0;
    }
    private IEnumerator InputWait() {
        yield return new WaitForSeconds(.2f);
        allowinput = true;
    }
}

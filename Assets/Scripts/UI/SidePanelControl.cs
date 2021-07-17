using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class SidePanelControl : MonoBehaviour
{   
    private bool isOriginalPos = false;
    [System.Serializable]
    public class MyEventType : UnityEvent { }
 
    public MyEventType OnExvent;

    public void InvokeEventWhenPositionIsZero(Vector2Int currentposition) {
        if (currentposition == Vector2Int.zero) {
            isOriginalPos = true;
            OnExvent.Invoke();
        }
        else if (currentposition != Vector2Int.zero) {
            isOriginalPos = false;
        }
    }
}

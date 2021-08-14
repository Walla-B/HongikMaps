using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class SidePanelControl : MonoBehaviour
{   
    private bool isOriginalPos = false;
    private bool blockInvoke = false;
    [System.Serializable]
    public class MyEventType : UnityEvent { }
    
    public MyEventType OnEvent;
    

    public void InvokeEventWhenPositionIsZero(Vector2Int currentposition) {

        if (blockInvoke == false) {
            if (currentposition == Vector2Int.zero) {
                isOriginalPos = true;
                OnEvent.Invoke();
            }
            else if (currentposition != Vector2Int.zero) {
                isOriginalPos = false;
            }
        }
    }

    public void BlockInvokeOnPositionChanged() {
        blockInvoke = true;
    }
    public void AllowInvokeOnPositionChanged() {
        blockInvoke = false;
    }
}

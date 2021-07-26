using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEvent : MonoBehaviour
{
    [System.Serializable]
    public class MyEventType : UnityEvent { }
    public MyEventType OnEvent;

    public void InvokeEvents() {
        OnEvent.Invoke();
    }
}

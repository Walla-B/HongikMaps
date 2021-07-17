using System;
using UnityEngine;
using UnityEngine.Events;
public class SidePanelControl : MonoBehaviour
{   
    [Serializable]
    public class MyEventType : UnityEvent { }
 
    public MyEventType OnEvent;

    public void Debugthis(Vector2Int something) {
        if (something == Vector2Int.zero) {
        }
    }
}

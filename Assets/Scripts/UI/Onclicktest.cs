using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onclicktest : MonoBehaviour
{
    public void Onclickvaluechanged() {
        Debug.Log("Value has changed");
    }
    public void Onclickendedit(){
        Debug.Log("Ending edit");
    }
    public void OnclickselectBox(){
        Debug.Log("Box selected");
    }
    public void OnclickDeselectBox(){
        Debug.Log("Box Deselected");
    }
}

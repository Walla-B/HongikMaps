using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPasser : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField from, to_Start, to_Target;
    private bool toTarget = true;

    public void PassText() {
        if (toTarget == true) {
            to_Target.text = from.text;
        }
        else {
            to_Start.text = from.text;
        }
    }
    public void PassTextReverse() {
        if (toTarget == true) {
            from.text = to_Target.text;
        }
        else {
            from.text =  to_Start.text;
        }
    }
    public void SetToTargetTrue() {
        toTarget = true;
        Debug.Log("settotarget TRUE");
    }

    public void SetToTargetFalse() {
        toTarget = false;
        Debug.Log("settotarget FALSE");
    }
}

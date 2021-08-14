using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPasser : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField from, to;

    public void PassText() {
        to.text = from.text;
    }
}

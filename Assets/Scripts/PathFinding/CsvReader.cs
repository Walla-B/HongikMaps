using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvReader : MonoBehaviour
{
    public static StreamReader Csvread(string filename){

        StreamReader sr;
        
        //runningin mobile platform
        if (Application.isMobilePlatform) {
            sr = new StreamReader(Application.persistentDataPath + "\\Scripts\\PathFinding\\" + filename);
            return sr;
        }
        
        sr = new StreamReader(Application.dataPath + "\\Scripts\\PathFinding\\" + filename);
        return sr;
    }
}

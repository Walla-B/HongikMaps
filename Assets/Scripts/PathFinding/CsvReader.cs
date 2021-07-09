using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvReader
{
    public static StringReader Csvread(string filename){

        StringReader sr;
        
        //runningin mobile platform
        /*
        if (Application.isMobilePlatform) {
            sr = new StringReader(Application.persistentDataPath + "\\Scripts\\PathFinding\\" + filename);
            
            return sr;
        }
        */

        //Using Resources.Load is not memory efficient. later needs to be 
        TextAsset sourcefile = Resources.Load<TextAsset>(filename);

        sr = new StringReader(sourcefile.text);
        return sr;
        //sr = new StringReader(Application.dataPath + "\\Scripts\\PathFinding\\" + filename);
    }
}

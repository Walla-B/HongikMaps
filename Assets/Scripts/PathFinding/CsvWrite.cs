using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvWrite : MonoBehaviour
{
    // Start is called before the first frame update
    const int numberofnode = 105;
    const string nameofobj = "Node";
    void Start()
    {

        GameObject temp = new GameObject();
        string data_lines;
        StreamWriter sw = new StreamWriter(@"C:\Users\dev\Desktop\HongikMaps_git\Assets\Scripts\PathFinding\Node.csv",true);
        for (int i = 0; i < numberofnode; i++) {
            temp = GameObject.Find(nameofobj + " (" + i + ")");
            //Debug.Log("positon of " + i + " : " + "x = " + temp.transform.position.x + "  y = " + temp.transform.position.y + "  z = " + temp.transform.position.z);
            data_lines = (i.ToString() + "," + temp.transform.position.x.ToString() + "," + temp.transform.position.y.ToString() + "," + temp.transform.position.z.ToString());
            //Debug.Log(data_lines);
            sw.WriteLine(data_lines);
        }
        sw.Close();
        
        //initialize array
        for (int i = 0; i < numberofnode; i++) {
            dataarray[i] = new int[6] {i , -1, -1, -1, -1, -1};
            indexarray[i] = 1;
            //Debug.Log(dataarray[i][0] + "  " + dataarray[i][1] + dataarray[i][2]);
        }
        
    }
    int[][] dataarray = new int[numberofnode][];
    int[] indexarray = new int[numberofnode];
    //private List<int[]> edgelist = new List<int[]>();
    private int nodecounter = 0;
    int[] twonodes = new int[2];
    GameObject[] twonodeobjects = new GameObject[2];

    public Camera DebugCamera;
    private Ray myRay;
    private RaycastHit hitray;



    // check true to export current nodes
    [SerializeField]
    private bool export = false;
    
    private Vector3 midpoint = new Vector3(Screen.width/2,Screen.height/2,0f);

    
    void Update(){

        //psudocode
        myRay = DebugCamera.ScreenPointToRay(midpoint);
        

        if (Input.GetKeyDown (KeyCode.Space) && Physics.Raycast(myRay,out hitray)) {

            //Debug.DrawRay(myRay.origin,myRay.direction,Color.green,3f);

            if (hitray.collider != null) {
                if (nodecounter < 2) {
                    string tempname = hitray.collider.gameObject.name;
                    string search1 = "(";
                    string search2 = ")";

                    int index1 = tempname.IndexOf(search1);
                    int index2 = tempname.IndexOf(search2);


                    twonodes[nodecounter] = int.Parse(tempname.Substring(index1 + 1,index2 - index1 - 1) );
                    twonodeobjects[nodecounter] = hitray.collider.gameObject;

                    //Debug.Log(twonodes[nodecounter]);
                    Debug.Log(nodecounter);
                    hitray.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

                    nodecounter++;
                }
                if (nodecounter >= 2) {
                    nodecounter = 0;
                    Debug.DrawLine(twonodeobjects[0].transform.position,twonodeobjects[1].transform.position,Color.red,10000f);
                    //edgelist.Add(twonodes);
                    dataarray[twonodes[0]][indexarray[twonodes[0]]] = twonodes[1];
                    indexarray[twonodes[0]] += 1;

                    dataarray[twonodes[1]][indexarray[twonodes[1]]] = twonodes[0];
                    indexarray[twonodes[1]] += 1;

                }
            }
        }
        
        if (export == true) 
        {
            export = false;

            StreamWriter sw1 = new StreamWriter(@"C:\Users\dev\Desktop\HongikMaps_git\Assets\Scripts\PathFinding\Edge.csv",true);
            string data_lines;

            for (int i = 0; i < numberofnode; i++) {
                data_lines = (dataarray[i][0].ToString() + "," + dataarray[i][1].ToString() + "," + dataarray[i][2].ToString() + "," + dataarray[i][3].ToString() + "," + dataarray[i][4].ToString()+ "," +dataarray[i][5].ToString());
                sw1.WriteLine(data_lines);
            }
            sw1.Close();
            Debug.Log("exporting");    
        }
    }

    
}

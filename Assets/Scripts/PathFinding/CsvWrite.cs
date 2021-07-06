using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CsvWrite : MonoBehaviour
{
    // This script is used as a Plugin for Easy Initialization of Node.csv and Edge.csv
    // might be not the cleanest code, but it works
    private Vector3 midpoint = new Vector3(Screen.width/2,Screen.height/2,0f);

    // Checklist to initialize before making Graph
    // 0. Is Path of StreamWriter correct?
    // 1. Is total number of nodes correct? => const int numberofnode
    // 2. Is name of Gameobject used as nodes correct? => nameofobj
    // 3. [OPTIONAL] size of csv's row is hard-coded into this script. 6 in this case.
    //      if number of Node's adjacent node exceeds 6, you can change it.

    const int numberofnode = 105;
    const string nameofobj = "Node";
    //Actually, this is just the row size of dataarray. max numbers of nodes can connect is maxconnectednods -1
    const int maxconnectednodes = 6;
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
            dataarray[i] = new int[maxconnectednodes] {i , -1, -1, -1, -1, -1};
            indexarray[i] = 1;
            //Debug.Log(dataarray[i][0] + "  " + dataarray[i][1] + dataarray[i][2]);
        }
        
        //psudocode
        /*
        if (overwriteEdge == true)
        {
            Import Edge.csv and parse into dataarray.   //dataarray already initalized to match current size of numberofnodes, so additional nodes will not be a problem.
            
            for (i = 0 to i < numberofnode) {
                for (j = 1 to j < 6) {
                    if (dataarray[i][j] == -1) {
                        indexarray[i] = j; 
                    }
                }
            }
        }
        */
        
        //Parse Edge.csv and copy its components to dataarray[][]
        if (overwriteEdge == true) {
            bool endoffile = false;
            StreamReader sr = CsvReader.Csvread("Edge.csv");

            while (!endoffile) {
                string line=sr.ReadLine();
                if (line == null) {
                    endoffile = true;
                    break;
                }
                string[] input_data_lines = line.Split(',');

                int currenindex = int.Parse(input_data_lines[0]);
                
                for (int i = 1; i < maxconnectednodes; i++) {
                    if (int.Parse(input_data_lines[i]) != -1) {
                        dataarray[currenindex][i] = int.Parse(input_data_lines[i]);
                    }
                    else if (int.Parse(input_data_lines[i]) == -1) {
                        indexarray[currenindex] = i;
                        break;
                    }
                }
            }
        }
        
        //color nodes that have at least 1 connected nodes
        for (int i = 0; i < numberofnode; i++) {
            if (indexarray[i] > 1) {
                GameObject.Find(nameofobj + " (" + i + ")").GetComponent<MeshRenderer>().material.color = Color.magenta;
            }
        }

        
    }
    int[][] dataarray = new int[numberofnode][];
    int[] indexarray = new int[numberofnode];
    //private List<int[]> edgelist = new List<int[]>();

    // nodecounter and twonodes used for different purposes twice. for Adding Edge and  Removing edge;
    private int nodecounter = 0;
    int[] twonodes = new int[2];
    GameObject[] twonodeobjects = new GameObject[2];


    //Raycasting Components
    public Camera DebugCamera;
    private Ray myRay;
    private RaycastHit hitray;



    // check true to export current nodes (Used as a button in playmode)
    [SerializeField]
    private bool export = false;

    // check true to read data from edge.csv and use that data
    [SerializeField]
    private bool overwriteEdge = true;
    [SerializeField] 
    private bool removeedge = false;
    

    //After Initializing the positions of Nodes, Playmode is used to add edges to graph
    //TODO: 
    //Problem 1: Edge cannot be Added over pre-existing Edge, so everything must be done within first try.
    //Solution to Problem 1 : Read Existing Edge.csv, Initialize dataarray AND indexarray also.
    //                             Changing Node's color to indicate might be tricky, but using pre-existing DebugGraph() we can simply draw existing graph
    //                             to distinguish existing graph.

    //Problem 2: Selection Error can be re-done. manually manipulating csv files will do the job, but it is time consuming.
    //Solution to Problem 2 : ...


    void Update(){

        //psudocode
        myRay = DebugCamera.ScreenPointToRay(midpoint);
        
        //remove edges from exsiting grpah
        
        //psudocode
        /*
            if (removeedge == true && input.getkey(r) && physics.raycast && input.getkey(r)) {    
                // maybe adding function when only works int nodecounter == 0 situation might be helpful
                if (nodecounter < 2) {
                    //no need to be colored, because it might confuse between already-node existing nodes.
                    parse the hitray object's name and put it in twonodes[]
                    Debug.Log(nodecounter);
                    nodecounter ++;
                }
                if (nodecounter == 2) {
                    nodecounter = 0;

                    twonodes[0] , twonodes[1];

                    findindex = Array.IndexOf(dataarray[twonodes[0]],twonodes[1])
                    dataarray[twonodes[0]][findindex] = -1;

                    Array.Sort(dataarray[twonodes[0]],1,maxconnectednodes -1);

                    Array.reverse(dataarray[twonodes[0]],1 maxconndectednodes -1)

                    findindcx = Array.indexof(dataarray[twonodes[1]],twonodes[0])
                    dataarray[twonodes[0]][findindex] = -1;

                    Array.sort
                    Array.reverse

                    removeedge == false;

                }
            }
            

        */

        if (removeedge == true && Input.GetKeyDown(KeyCode.R) && Physics.Raycast(myRay, out hitray)) {
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
                    nodecounter++;
            }
            if (nodecounter == 2) {
                nodecounter = 0;
                Debug.DrawLine(twonodeobjects[0].transform.position, twonodeobjects[1].transform.position,Color.black,10000f);

                int findindcx = Array.IndexOf(dataarray[twonodes[0]],twonodes[1]);

                Debug.Log("findindex : " +findindcx);                


                dataarray[twonodes[0]][findindcx] = -1;

                Array.Sort(dataarray[twonodes[0]],1,maxconnectednodes - 1);
                Array.Reverse(dataarray[twonodes[0]],1,maxconnectednodes -1);

                findindcx = Array.IndexOf(dataarray[twonodes[1]],twonodes[0]);

                Debug.Log("findindex 2 : " + findindcx);
                dataarray[twonodes[1]][findindcx] = -1;

                Array.Sort(dataarray[twonodes[1]],1,maxconnectednodes -1);
                Array.Reverse(dataarray[twonodes[1]],1,maxconnectednodes -1 );


                removeedge = false;
            }
        }

        
        //add graph to exsitigin graph
        if (Input.GetKeyDown (KeyCode.Space) && Physics.Raycast(myRay,out hitray) && removeedge == false) {

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
        
        //Exported files are stored in EdgeTemp in case of messing up.

        if (export == true) 
        {
            export = false;

            StreamWriter sw1 = new StreamWriter(@"C:\Users\dev\Desktop\HongikMaps_git\Assets\Scripts\PathFinding\EdgeTemp.csv",true);
            string data_lines;

            for (int i = 0; i < numberofnode; i++) {
                data_lines = (dataarray[i][0].ToString() + "," + dataarray[i][1].ToString() + "," + dataarray[i][2].ToString() + "," + dataarray[i][3].ToString() + "," + dataarray[i][4].ToString()+ "," +dataarray[i][5].ToString());
                sw1.WriteLine(data_lines);
            }
            sw1.Close();
            Debug.Log("export complete");    
        }
    }

    
}

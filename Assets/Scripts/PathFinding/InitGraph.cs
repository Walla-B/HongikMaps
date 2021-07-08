using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InitGraph : MonoBehaviour
{
    // Start is called before the first frame update
    private Graph graphcomp;
    void Awake()
    {
        Graph graph = new Graph();
        ///////////////////////////////
        ///////////////////////////////
        ///////////////////////////////
        var stream = CsvReader.Csvread("Node");
        ///////////////////////////////
        ///////////////////////////////
        ///////////////////////////////
        string line;
        bool endOfFile = false;

        //Initializing NodeID and Node's posiiton data
        while (!endOfFile) {
            line = stream.ReadLine();
            //Debug.Log(line);

            if(line == null) {
                endOfFile = true;
                break;
            }
            
            string[] data_lines = line.Split(',');
            //Debug.Log(data_lines[0]); 
            int nodeID = int.Parse(data_lines[0]);
            
            Vector3 nodeTransform = new Vector3(float.Parse(data_lines[1]),float.Parse(data_lines[2]),float.Parse(data_lines[3]));
            Node tempnode = new Node(nodeID,nodeTransform);


            graph.AddNodeToGraph(tempnode);


        }
        //Initializing relationship between nodes with Edge.csv
        endOfFile = false;
        stream = CsvReader.Csvread("Edge");

        while (!endOfFile) {
            line = stream.ReadLine();

            if (line == null) {
                endOfFile = true;
                break;
            }
            string [] data_lines = line.Split(',');

            int currentNodeIndex = int.Parse(data_lines[0]);
            Node currentNode = graph.GetNodeInGraph(currentNodeIndex);
            
            for (int i = 1; i < 5; i++) {
                int targetNodeIndex = int.Parse(data_lines[i]);
                if (targetNodeIndex == -1) {
                    break;
                }

                //Debug.Log(targetNodeIndex);
                currentNode.AddAdjNode(graph.GetNodeInGraph(targetNodeIndex));
            }
        }


        
        DebugGraph.DrawGraph(graph);


        ////this part of code should be not in Awake()
        /*
        Dijkstra pathfinding = new Dijkstra();

        List<Node> path = pathfinding.Dijkstrasolve(graph,sttnodeindex,tgtnodeindex);
        VisualizePathResult.VisulaizePath(path,nodeobject,edgeobject);
        


        //Draw pathfinding result in scene view node with green line
        for (int i = 0; i < path.Count; i++)
        {
            
            if (i < path.Count - 1)
                Debug.DrawLine(path[i].Coordinate,path[i+1].Coordinate,Color.green,100f);
            else if (i == path.Count - 1 ) {
                Debug.Log("Total cost : " + path[i].Weight);
                break;
            }
                
 
        }
        */

        graphcomp = graph;

    }
    void Update(){
        if (calculate == true && Input.GetMouseButtonDown(0)) {
            if (nodeselectindex < 2) {
                myray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(myray,out hitray);
                Debug.DrawRay(myray.origin,myray.direction,Color.red,100f);

                string tempname = hitray.collider.gameObject.name;
                string search1 = "(";
                string search2 = ")";

                int index1 = tempname.IndexOf(search1);
                int index2 = tempname.IndexOf(search2);


                int nodeindex = int.Parse(tempname.Substring(index1 + 1,index2 - index1 - 1) );

                if (nodeselectindex == 0) {
                    sttnodeindex = nodeindex;
                }
                else if (nodeselectindex == 1) {
                    tgtnodeindex = nodeindex;
                }
                nodeselectindex++;
            }
            else if (nodeselectindex == 2) {
                nodeselectindex = 0;
                Dijkstra pathfind = new Dijkstra();
                visualize.VisulaizePath(pathfind.Dijkstrasolve(graphcomp,sttnodeindex,tgtnodeindex), nodeobject,edgeobject);

                calculate = false;
            }
            //Debug.Log("called");
        }
    }
    VisualizePathResult visualize = new VisualizePathResult();

    private Ray myray;
    private RaycastHit hitray;
    [SerializeField]
    private bool calculate = false;
    [SerializeField]
    private int nodeselectindex = 0;
    [SerializeField] 
    int sttnodeindex = 0, tgtnodeindex = 30;
    [SerializeField]
    private GameObject nodeobject;
    [SerializeField]
    private GameObject edgeobject;

}

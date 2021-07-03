using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InitGraph : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Graph graph = new Graph();

        StreamReader stream = CsvReader.Csvread("Node.csv");

        string line;
        bool endOfFile = false;

        //Initializing NodeID and Node's posiiton data
        while (!endOfFile) {
            line = stream.ReadLine();

            if(line == null) {
                endOfFile = true;
                break;
            }
            
            string[] data_lines = line.Split(',');
            
            int nodeID = int.Parse(data_lines[0]);
            Vector3 nodeTransform = new Vector3(float.Parse(data_lines[1]),float.Parse(data_lines[2]),float.Parse(data_lines[3]));
            Node tempnode = new Node(nodeID,nodeTransform);


            graph.AddNodeToGraph(tempnode);


        }
        //Initializing relationship between nodes with Edge.csv
        endOfFile = false;
        stream = CsvReader.Csvread("Edge.csv");

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

                Debug.Log(targetNodeIndex);
                currentNode.AddAdjNode(graph.GetNodeInGraph(targetNodeIndex));
            }
        }



    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickPathFind : MonoBehaviour
{   
    private Graph graph;
    void Awake(){
        graph = InitGraph.InitiateGraphFromData();
    }
    [SerializeField]
    private GameObject nodeobject,edgeobject;
    private InitGraph showgraph;
    public void PathfindOnClick(){
        Debug.Log("Hello");
        //Graph graph = showgraph.GetGraph();
        VisualizePathResult.VisulaizePath(Dijkstra.Dijkstrasolve(graph,0,40),nodeobject,edgeobject);
    }
}

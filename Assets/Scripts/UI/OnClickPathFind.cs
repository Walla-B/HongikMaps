using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnClickPathFind : MonoBehaviour
{   
    [SerializeField]
    private GameObject nodeobject,edgeobject;
    [SerializeField]
    private TMP_InputField input1,input2;
    [SerializeField]
    private TextMeshProUGUI outputdist;
    private Graph graph;
    void Awake(){
        graph = InitGraph.InitiateGraphFromData();
        DebugGraph.DrawGraph(graph);
    }
    public void PathfindOnClick(){
        int sttnodeindex = int.Parse(input1.text);
        int tgtnodeindex = int.Parse(input2.text);
        List<Node> path = Dijkstra.Dijkstrasolve(graph,sttnodeindex,tgtnodeindex);
        outputdist.text = "Distance : " + path[path.Count - 1].Weight.ToString();
        VisualizePathResult.VisulaizePath(path,nodeobject,edgeobject);
    }
}

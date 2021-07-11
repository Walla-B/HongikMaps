using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnClickPathFind : MonoBehaviour
{   
    [SerializeField]
    private GameObject nodeobject,edgeobject = null;
    [SerializeField]
    private TMP_InputField input1,input2 = null;
    [SerializeField]
    private TextMeshProUGUI outputdist, outputtime, pathmode;
    private Graph graph;
    void Awake(){
        graph = InitGraph.InitiateGraphFromData();
        DebugGraph.DrawGraph(graph);
    }
    public void PathfindOnClick(){
        int sttnodeindex = int.Parse(input1.text);
        int tgtnodeindex = int.Parse(input2.text);
        List<Node> path = Dijkstra.Dijkstrasolve(graph,sttnodeindex,tgtnodeindex);
        //Round the Float distance to integer and add "m" to display unit
        float totalweight = path[path.Count-1].Weight;

        //this pathmode should get information from outside and switch-case
        pathmode.text = "Minimum Distance";
        outputdist.text = (Mathf.Round(totalweight)).ToString() + "m";
        outputtime.text = (Mathf.Floor(totalweight/84)).ToString() + "min " + (Mathf.Round(totalweight%84)).ToString() + "seconds";
        VisualizePathResult.VisulaizePath(path,nodeobject,edgeobject);
    }
}

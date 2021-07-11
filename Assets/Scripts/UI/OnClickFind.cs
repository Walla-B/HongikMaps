using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnClickFind : MonoBehaviour
{   
    [SerializeField]
    private GameObject nodeobject,edgeobject,targetpointer,uiobj = null;
    [SerializeField]
    private TMP_InputField start,target = null;
    [SerializeField]
    private TextMeshProUGUI outputdist, outputtime, pathmode;
    private Graph graph;
    void Awake(){
        graph = InitGraph.InitiateGraphFromData();
        DebugGraph.DrawGraph(graph);
        //Debug.Log("grpah initialized");
    }
    public void PathfindOnClick(){
        int sttnodeindex = int.Parse(start.text);
        int tgtnodeindex = int.Parse(target.text);
        List<Node> path = Dijkstra.Dijkstrasolve(graph,sttnodeindex,tgtnodeindex);
        //Round the Float distance to integer and add "m" to display unit
        float totalweight = path[path.Count-1].Weight;
        int totalseconds = Mathf.RoundToInt(totalweight/1.4f);

        //this pathmode should get information from outside and switch-case
        pathmode.text = "Minimum Distance";
        outputdist.text = (Mathf.RoundToInt(totalweight)).ToString() + "m";
        outputtime.text = (totalseconds/60).ToString() + "min " + (totalseconds%60).ToString() + "seconds";
        VisualizePathResult.VisulaizePath(path,nodeobject,edgeobject);

        /*
            Test Scripts
        */
        Vector3 planepos = Camera.main.WorldToScreenPoint(path[0].Coordinate);
        //Debug.Log(planepos.x + "  " + planepos.y + "  " + planepos.z);
        GameObject targetobj;
        targetobj = Instantiate(targetpointer,planepos,Quaternion.identity);
        targetobj.transform.SetParent(uiobj.transform,true);
    }
    public void FindLocationOnClick(){
        //targetpointer.transform.SetParent(canvas.transform);
        Debug.Log("findobjonclick");
        Vector3 planepos = Camera.main.WorldToScreenPoint(graph.GetNodeInGraph(0).Coordinate);
        //Debug.Log(planepos.x + "  " + planepos.y + "  " + planepos.z);
        GameObject targetobj;
        targetobj = Instantiate(targetpointer,planepos,Quaternion.identity) as GameObject;
        targetobj.transform.SetParent(uiobj.transform,false);
        //targetobj = Instantiate(targetpointer,);
        //var canvass = new GameObject("canvas",30f);
    }
}

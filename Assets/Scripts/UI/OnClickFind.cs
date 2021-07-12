using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnClickFind : MonoBehaviour
{   
    [SerializeField]
    private GameObject nodeobject,edgeobject,startpoint,endpoint,uiobj = null;
    [SerializeField]
    private TMP_InputField start,target = null;
    [SerializeField]
    private TextMeshProUGUI outputdist, outputtime, pathmode;
    private Graph graph;

    private bool displaypointer = false;
    private GameObject instantiatedstart, instantiatedend;
    private Vector3 startcoord, endcoord;
    void Awake(){
        graph = InitGraph.InitiateGraphFromData();
        DebugGraph.DrawGraph(graph);
        //Debug.Log("grpah initialized");
    }
    public void PathfindOnClick(){

        OnCallDestroyPrevious_PointerObj();

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

        //Get screen position of start, end node
        Vector3 planepos1 = Camera.main.WorldToScreenPoint(path[0].Coordinate);
        Vector3 planepos2 = Camera.main.WorldToScreenPoint(path[path.Count-1].Coordinate);
        //Debug.Log(planepos.x + "  " + planepos.y + "  " + planepos.z);

        instantiatedstart = Instantiate(startpoint,planepos1,Quaternion.identity,uiobj.transform);
        instantiatedstart.tag = "PointerObj";

        instantiatedend = Instantiate(endpoint,planepos2,Quaternion.identity,uiobj.transform);
        instantiatedend.tag = "PointerObj";

        //Set scale to 1,1,1
        instantiatedstart.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
        instantiatedend.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);

        startcoord = path[0].Coordinate;
        endcoord = path[path.Count - 1].Coordinate;

        //FollowNode(Gameobj targetobj, Gameobj targetobj2,Vector3 path[0].coordinate, Vector3 path[path.Count-1].Coordinate);


        displaypointer = true;
        
        //Destroy for debug
        //Destroy(targetobj,5.0f);
        //Destroy(targetobj2,5.0f);
    }
    public void FindLocationOnClick(){
        //targetpointer.transform.SetParent(canvas.transform);
        Debug.Log("findobjonclick");
        Vector3 planepos = Camera.main.WorldToScreenPoint(graph.GetNodeInGraph(0).Coordinate);
        //Debug.Log(planepos.x + "  " + planepos.y + "  " + planepos.z);
        GameObject targetobj;
        targetobj = Instantiate(endpoint,planepos,Quaternion.identity) as GameObject;
        targetobj.transform.SetParent(uiobj.transform,false);
        //targetobj = Instantiate(targetpointer,);
        //var canvass = new GameObject("canvas",30f);
    }

    public void OnCallDestroyPrevious_PointerObj(){
        displaypointer = false;
        var previousObjects = GameObject.FindGameObjectsWithTag("PointerObj");
            for (int i = 0; i < previousObjects.Length ; i++) {
                GameObject.Destroy(previousObjects[i]);
            }
    }


    void LateUpdate(){
        if (displaypointer == true) {
            instantiatedstart.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(startcoord);
            instantiatedend.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(endcoord);
        }

    }
}

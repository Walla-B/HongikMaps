using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnClickFind : MonoBehaviour
{   
    [SerializeField]
    private GameObject nodeobject,edgeobject,startpoint,endpoint,uiobj = null;
    [SerializeField]
    private TMP_InputField target, pathfind_start,pathfind_target = null;
    [SerializeField]
    private TextMeshProUGUI outputdist, outputtime, pathmode;
    private Graph graph;

    private bool displaypointer = false;
    private GameObject instantiatedstart, instantiatedend;
    private Vector3 startcoord, endcoord;
    void Awake(){
        plane.SetNormalAndPosition(Vector3.up,new Vector3(0,plane_y_position,0));
        graph = InitGraph.InitiateGraphFromData();
        DebugGraph.DrawGraph(graph);
        //Debug.Log("grpah initialized");
    }
    public void PathfindOnClick(){

        OnCallDestroyPrevious_PointerObj();

        int sttnodeindex = int.Parse(pathfind_start.text);
        int tgtnodeindex = int.Parse(pathfind_target.text);
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
        
        
        targetPosition = Camera.main.transform.position + Planeposition(planepos1) - Planeposition(new Vector3(Screen.width/2,Screen.height/2,0));
        moveinertiatoggle = true;
        //Destroy for debug
        //Destroy(targetobj,5.0f);
        //Destroy(targetobj2,5.0f);
    }
    public void FindLocationOnClick(){

        OnCallDestroyPrevious_PointerObj();
        VisualizePathResult.OnCallDestroyPrevious_PathObj();
        
        int tgtnodeindex = int.Parse(target.text);
        Vector3 worldpos = graph.GetNodeInGraph(tgtnodeindex).Coordinate;
        //targetpointer.transform.SetParent(canvas.transform);
        //Debug.Log("findobjonclick");
        Vector3 planepos = Camera.main.WorldToScreenPoint(worldpos);
        instantiatedend = Instantiate(endpoint,planepos,Quaternion.identity,uiobj.transform);
        instantiatedend.tag = "PointerObj";

        instantiatedend.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
        displaypointer = true;

        endcoord = worldpos;


        targetPosition = Camera.main.transform.position + Planeposition(planepos) - Planeposition(new Vector3(Screen.width/2,Screen.height/2,0));
        moveinertiatoggle = true;
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
    private Vector3 Planeposition(Vector3 screenpos) {
        var rayNow = Camera.main.ScreenPointToRay(screenpos);
        if (plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private Plane plane;
    private float plane_y_position = -10;
    private float elapsedtime = 0f;
    private bool moveinertiatoggle;
    private Vector3 targetPosition;
    private void MoveCameraToPoint(){

    }
    void Update(){
        if (Input.touchCount >= 1 && elapsedtime >= 0.3f) {
            moveinertiatoggle = false;
            elapsedtime = 0;
        }
        if (moveinertiatoggle == true) {
            elapsedtime += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,targetPosition, 5 * Time.deltaTime);

            if (elapsedtime >= 2f) {
                elapsedtime = 0;
                moveinertiatoggle = false;
            }
        }
    }
    //Using LateUpdate(), pointer's jiggly motion is now fixed.
    //TODO:
    //this code might can be optimized by checking current maincamera's movement is zero or not.
    void LateUpdate(){
        if (displaypointer == true) {
            instantiatedend.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(endcoord);
            if (instantiatedstart != null) {
                instantiatedstart.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(startcoord);
            }
        }

    }

}

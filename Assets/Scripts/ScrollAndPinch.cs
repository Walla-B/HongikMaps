using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndPinch : MonoBehaviour
{   
    
    public Camera Camera;
    public bool Rotate;
    
    protected Plane Plane = new Plane(Vector3.up, new Vector3(0,100,0));
    //protected Plane Plane;
    [SerializeField]
    private float zoomthreshhold = 0;
    [SerializeField]
    //threshhold factor of camera when scrolling;
    private float movethershhold_factor = 1;
    [SerializeField]
    private float move_sensitivity;
    [SerializeField]
    private float t;

    private float elapsedtime = 0f;
    private float move_inertia = 0;
    private bool inertiatoggle = false;
    private Vector3 Touchphase_endposition = Vector3.zero;
    private Vector3 Delta3 = Vector3.zero;



    public GameObject Debugobject;

    //if camera is null, put maincamera in Camera
    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;
    }

    private void Update()
    {
        
        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;

        //Scroll
        //Scroll when touchcount is 1
        if (Input.touchCount == 1)
        {
            /* 
            elapsedtime += Time.deltaTime;
            if (elapsedtime >= 0.3f){
                elapsedtime = elapsedtime % elapsedtime;
                //Debug.Log("second has been elapsed" + elapsedtime);
                Delta2 = PlanePositionNow(Input.GetTouch(0));
                //Debug.Log("Delta2   " + Delta2);
            }
            */


            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Began){
                inertiatoggle = false;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Ended){
                Camera.transform.Translate(Delta1 * movethershhold_factor, Space.World);
                if (Input.GetTouch(0).phase == TouchPhase.Ended){
                    Touchphase_endposition = Camera.transform.position;
                    Delta3 = Delta1;
                    Debug.Log(Delta3);
                    if (Mathf.Abs(Delta3.x) + Mathf.Abs(Delta3.z) > 3f){
                        inertiatoggle = true;
                        elapsedtime = 0f;
                        Debug.Log("Passed threshold");
                    }
                }
                //Debug.Log("Endede"+ Delta1);
            }


                //Debug.Log("x  :  " + Mathf.Abs(Delta1.x) + "  z  :  " + Mathf.Abs(Delta1.z));
            //if (Mathf.Abs(Delta1.x) + Mathf.Abs(Delta1.z) >= 10){
                //Delta2 = Delta1;
                //Debug.Log("Delta1 is over the treshold" + Delta2);
                //Debug.Log("Delta1 is  over the threshold");
            //}
            //else
                //Delta2 = Vector3.zero;
                //Debug.Log("xvalue : " + Delta2.x + "   yvalue : " + Delta2.y + "    zvalue : " + Delta2.z);
            
            /*
            if(Input.GetTouch(0).phase == TouchPhase.Ended){
                Vector3 recentDelta = PlanePositionNow(Input.GetTouch(0)); //- Delta2;
                Debug.Log("recentDelta   " + recentDelta);
                Vector3 recentmovementDelta = Delta2 - recentDelta;
                Debug.Log("recnetmovementeDelta" + recentmovementDelta);
            
                //Debug.Log(Delta2);
                //Camera.transform.position = Vector3.Lerp(transform.position, transform.position + Delta2 * 0.1f ,t*Time.deltaTime);
                //Debug.Log("xvalue : " + Delta1.x + "   yvalue : " + Delta1.y + "    zvalue : " + Delta1.z);
            }
            */
            //Debug.Log(Mathf.Abs(Delta1.x + Delta1.z));
            /*
            if (Mathf.Abs(Delta1.x)+Mathf.Abs(Delta1.z)>=10)
            {over the threshol
                Debug.Log("Inertia assigned");
                move_inertia = 1;   
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {   
                Debug.Log("inertia toggled on");
                inertiatoggle = true;
            }

            while (inertiatoggle == true)
            {
                Camera.transform.Translate(Delta1 * movethershhold_factor * move_inertia , Space.World);
                move_inertia -= 0.0001f;
                //Debug.Log(move_inertia);
                
                if (move_inertia <= 0.001f)
                {
                    Debug.Log("inertia toggled off");
                    inertiatoggle = false;   
                }
            }
            */
        }
        

        //TODO:
        //OK//일정 시간이 지나면 멈추도록 구현
        //OK//멈추면 inertia toggle off
        //OK//Inertiatogggle 이 켜지는 조건이 특정 treshold 를 넘도록 설정하여 과도한 리소르를 잡아먹지 않도록 최적화
            //zoom factor 고려해야함
        //OK//inertia motion이 진행되는 중 touchinput이 들어오면 interrupt해야함.
        //Camera Max zoom & Min zoom 고려해야함

        if (inertiatoggle == true){
            Camera.transform.position = Vector3.Lerp(Camera.transform.position , Touchphase_endposition + (Delta3 * 3f), 100*t*Time.deltaTime );
            //Debug.Log("inertia applied");
            elapsedtime += Time.deltaTime;
            if(elapsedtime >=1f){
                inertiatoggle = false;
                //Debug.Log("inertia toggle off");
            } 
        }

        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1  = PlanePosition(Input.GetTouch(0).position);
            var pos2  = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            /*var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            */
            //calc zoom for orthographic
            var zoom = Vector3.Distance(pos1b,pos2b) / Vector3.Distance(pos1,pos2);

            //edge case
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            //Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);
            
            //Debug.Log(zoom);
            /*
            if (zoom >= 1 + zoomthreshhold && zoom  <= 1 + zoomthreshhold)
            {
            }
            */
            Camera.orthographicSize = Camera.orthographicSize * zoom;

            //zoom camera for orthographic
            //TODO:
            // 첫번째로 손가락을 댄 부분을 기준으로 도는것이 아닌, 두 지점의 중간점을 기준으로 회전해야함
            
            //TODO:
            //zoom이 끝난 후, 일정시간 scroll을 neglect하도록 하여 zoom시 튐현상 방지해야함.
            
            
            //Vector3 pos3 = pos1;
            //Vector3 position = (pos1 + pos2) / 2;
            if (Rotate && pos2b != pos2) {
                Vector3 position = (pos1 + pos2) / 2;
                Debug.Log(pos1 +" pos "+ pos2 + " ops " + position);
                Vector3 newplaneposition = (Camera.transform.position + position) /2;
                Camera.transform.RotateAround(newplaneposition, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
                Instantiate(Debugobject,newplaneposition,Quaternion.identity);
                //Object.Destroy(Debugobject,0.5f);
            }
        }

    }
    
    //Camera.rotation 38.68 -175.459 0
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        /*
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;
        */
        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);

        Debug.DrawRay(rayNow.origin , rayNow.direction * 100, Color.red, 5f);
        Debug.DrawRay(rayBefore.origin, rayBefore.direction * 50, Color.green, 5f);

        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }
    protected Vector3 PlanePositionNow(Touch touch){
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayNow, out var enterNow)){
            return rayNow.GetPoint(enterNow);
        }
        return Vector3.zero;
    }
    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}

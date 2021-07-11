using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//CHANGE ALL INERTIA MOVEMENTS IMPLEMENTED WITH TIME-TRIGGER TO COUROUTINE()
public class ScrollAndPinch : MonoBehaviour
{   
    //Visible Components
    public Camera Camera;
    protected Plane Plane; // = new Plane(Vector3.up, new Vector3(0,100,0));

    [SerializeField]
    //threshhold factor of camera when scrolling;
    private float movethershhold_factor = 1.5f;
    [SerializeField]
    private float t = 1.0f;

    //Average Y position of Map
    [SerializeField]
    private float plane_y_position = -10;
    private float elapsedtime = 0f;
    private bool moveinertiatoggle, zoominertiatoggle , rotateinertiatoggle = false;
    private int recent_touch_is_two = 0;
    private Vector3 Touchphase_endposition ,Delta3 = Vector3.zero;


    private bool zoomflag, rotateflag, initdataflag = false;

    private float init_zoomdist, temp_zoomdist, init_theta = 0f;
    private Vector3 init_pivot;
    private Vector2 ScreenCenter = new Vector2(Screen.width/2,Screen.height/2);


    [SerializeField]
    const float threshhold_ZIn = 1.13f;
    [SerializeField]
    const float threshhold_ZOut = 0.87f;
    [SerializeField]
    const float threshhold_theta = 3.5f;
    const float MAXZOOM = 30f;
    const float MINZOOM = 200f;

    private float lastzoomfactor , lastrotatetheta = 0f;
    private Vector3 lastzoommotion = Vector3.zero;
    

    //public GameObject Debugobject;

    //if camera is null, put maincamera in Camera
    private void Awake() {
        if (Camera == null)
            Camera = Camera.main;
    }

    private void Update() {   
        
        //TODO: 움직이는 것도 일정 treshold 설정해서 클릭 동작과 구분할수 있어야함
        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, new Vector3(0,plane_y_position,0));

        var Delta1 = Vector3.zero;

        //Scroll
        //Scroll when touchcount is 1
        if (Input.touchCount == 1) {

            if (recent_touch_is_two >= 0) {
                recent_touch_is_two -= 1;
                return;   
            }

            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Began) {
                ClearInertiaToggleAndParam();
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Ended) {
                
                Camera.transform.Translate(Delta1 * movethershhold_factor, Space.World);

                if (Input.GetTouch(0).phase == TouchPhase.Ended) {
                    Touchphase_endposition = Camera.transform.position;
                    Delta3 = Delta1;
                    //Debug.Log(Delta3);
                    if (Mathf.Abs(Delta3.x) + Mathf.Abs(Delta3.z) > 3f) {
                        moveinertiatoggle = true;
                        elapsedtime = 0f;
                        //Debug.Log("Passed threshold");
                    }
                }
                //Debug.Log("Endede"+ Delta1);
            }
        }
        

        //TODO:
        //OK//일정 시간이 지나면 멈추도록 구현
        //OK//멈추면 inertia toggle off
        //OK//Inertiatogggle 이 켜지는 조건이 특정 treshold 를 넘도록 설정하여 과도한 리소스를 잡아먹지 않도록 최적화
            //zoom factor 고려해야함
        //OK//inertia motion이 진행되는 중 touchinput이 들어오면 interrupt해야함.
        //Camera Max zoom & Min zoom 고려해야함

        if (moveinertiatoggle || zoominertiatoggle || rotateinertiatoggle == true) {
            elapsedtime += Time.deltaTime;
            if (moveinertiatoggle == true) {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position , Touchphase_endposition + (Delta3 * 4f) ,100 * t * Time.deltaTime);
            }
            if (zoominertiatoggle == true) {
                //Camera.transform.position = Vector3.Lerp(Camera.transform.position , )
                //Camera.orthographicSize = Camera.orthographicSize * lastzoomfactor;
                if (Mathf.Abs(lastzoomfactor - 1f) <= 0.005f) {
                    zoominertiatoggle = false;
                }

                lastzoomfactor = Mathf.Lerp(lastzoomfactor , 1,10 * Time.deltaTime);
                lastzoommotion = Vector3.Lerp(lastzoommotion, Vector3.zero, 10 * Time.deltaTime);
                ZoomCamera(lastzoomfactor, lastzoommotion);
                //Mathf.Lerp(lastzoomfactor,1,100 * Time.deltaTime);
                //ZoomCamera(Mathf.Lerp(lastzoomfactor,1,100 * Time.deltaTime), Vector3.Lerp(lastzoommotion,Vector3.zero,100*Time.deltaTime));
                //Debug.Log("Zoom_interp Working");
            }
            if (rotateinertiatoggle == true) {
                lastrotatetheta = Mathf.Lerp(lastrotatetheta, 0f, 10*Time.deltaTime);
                RotateCamera(init_pivot,lastrotatetheta);
            }

            if(elapsedtime >= 2f) {
                ClearInertiaToggleAndParam();
            }
        }

        //Pinch
        //TODO:
        //Zoom, Rotate에도 inertia 구현할수있으면 해야함
        //Zoom시 현재는 화면 중심을 기준으로 확대됨, 확대한 중심점이 기준이 되어 확대될수 있으면 좋을것같음
        //ZoomOrRotate() 가 아직 뭔가 미흡함. 적절한 값을 찾아야함.

        if (Input.touchCount >= 2) {

            var pos1  = PlanePosition(Input.GetTouch(0).position);
            var pos2  = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
            var centerray = PlanePosition(ScreenCenter);
            int ZorR;


            //Debug.Log("DeltaPos1 : " + Input.GetTouch(0).deltaPosition);
            //Debug.Log("DeltaPos2 : " + Input.GetTouch(1).deltaPosition);

            //touch가 2곳 일어났을때 초기화
            if (initdataflag == false) {
                //튐현상 막기위한 조치, may be a better solution
                recent_touch_is_two = 5;

                ClearInertiaToggleAndParam();

                init_zoomdist = Vector3.Distance(pos1,pos2);
                init_pivot = (pos1 + pos2) / 2;
                
                temp_zoomdist = init_zoomdist;
                
                initdataflag = true;
            }


            //calc zoom for orthographic camera
            var zoom = Vector3.Distance(pos1b,pos2b) / Vector3.Distance(pos1,pos2);
            //var position = (pos1 + pos2) / 2;
            float rotatetheta = Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal);

            if (zoom == 0 || zoom > 10)
                return;

            if (zoomflag == true) {
                ZoomCamera(zoom,(init_pivot-centerray));
            }
            else if (rotateflag == true) {
                RotateCamera(init_pivot,rotatetheta);
            }
            else {
                ZorR = ZoomOrRotate(rotatetheta,zoom);
                if (ZorR == 0) { //zoom
                    zoomflag = true;
                    rotateflag = false;
                    //ZoomCamera(zoom);
                }
                else if (ZorR == 1) { // rotate
                    zoomflag = false;
                    rotateflag = true;
                    //RotateCamera(position,rotatetheta);
                }
                else
                   return; 
            }

            //zoom camera for orthographic
            //TODO:
            //OK//첫번째로 손가락을 댄 부분을 기준으로 도는것이 아닌, 두 지점의 중간점을 기준으로 회전해야함
            //OK//zoom 이 끝난 후, 일정시간 scroll 을 neglect 하도록 하여 zoom 시 튐현상 방지할 수 있어야함.


            //둘중 한손가락이 떼어지면 초기화 
            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended) {
                
                //초기화 하기 전, 기존 motion에 따라 어떤 inertia motion을 가질것인지 결정함
                if (zoomflag == true) {
                    lastzoomfactor = zoom;
                    lastzoommotion = (init_pivot-centerray);

                    zoominertiatoggle = true;
                    //Debug.Log("Zoominertia set to true");
                    elapsedtime = 0f;
                }
                if (rotateflag == true) {
                    lastrotatetheta = rotatetheta;

                    rotateinertiatoggle = true;
                    //Debug.Log("Rotateinertia set to true");
                    elapsedtime = 0f;
                }
                ClearFlag();
                ClearZoomOrRotateParam();
            }
        }
    }
    
    //Camera.rotation 38.68 -175.459 0
    protected Vector3 PlanePositionDelta(Touch touch) {
        
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);

        //Debug.DrawRay(rayNow.origin , rayNow.direction * 100, Color.red, 5f);
        //Debug.DrawRay(rayBefore.origin, rayBefore.direction * 50, Color.green, 5f);

        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos) {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    protected void ClearFlag() {
        zoomflag = false;
        rotateflag = false;
        initdataflag = false;
    }
    //protected int Setmotion(float theta, float zoom, float leantheta)
    protected int ZoomOrRotate(float theta, float zoom) {
         
        temp_zoomdist *= zoom;
        init_theta += theta;

        if (temp_zoomdist >= threshhold_ZIn * init_zoomdist || temp_zoomdist <= threshhold_ZOut * init_zoomdist) {
            return 0;
        }
        else if (Mathf.Abs(init_theta) >= threshhold_theta) {
            return 1;    
        }
      /*else if (Passes Camera Lean threshold) {
            return 2;
        } */

        else
            return 2;
            //return 3;
        
    }
    protected void ClearZoomOrRotateParam() {
        init_theta = 0f;
        init_zoomdist = 0f;
        temp_zoomdist = 0f;
    }

    protected void ClearInertiaToggleAndParam(){
        moveinertiatoggle = false;
        zoominertiatoggle = false;
        rotateinertiatoggle = false;

        lastzoomfactor = 0f;
        lastrotatetheta = 0f;
        lastzoommotion = Vector3.zero;
    }

    protected void MoveCamera() {
        //TODO:
        //Set Move Boundary
        //Set Threshold to define it as "moving"



    }
    protected void ZoomCamera(float zoom,Vector3 deltaPos) {
        
        //zoom limit, 더 나은 방법 존재 가능
        if (Camera.orthographicSize * zoom <= MAXZOOM) {
            return;
        }
        else if (Camera.orthographicSize * zoom >= MINZOOM) {
            return;
        }
        
        Camera.orthographicSize = Camera.orthographicSize * zoom;
        //while zooming, also moves camera to "zoom" into desired point
        Camera.transform.Translate(deltaPos*(1-zoom),Space.World);
        //Move Camera DeltaPos*(1-zoom)
    }

    protected void RotateCamera(Vector3 position , float theta) {
        Camera.transform.RotateAround(position,Plane.normal, theta / 2);
    }

    protected void LeanCamera(Vector3 position, float theta) {
        
        /*
        if (CameraAngle += theta > Maxangle) {
            return;
        }
        else if (CameraAngle -= theta < Minangle) {
            return;
        }
        //Axis should be Cross Product of two vectors
        //Vector3 axis = Vector3.Cross(Plane.normal,current positon - Planeposition(Screenpos))
        Camera.transform.RotateAround(position,axis,theta);
        */
    }


}

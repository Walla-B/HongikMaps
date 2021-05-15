using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndPinch : MonoBehaviour
{   
    
    public Camera Camera;
    public bool Rotate;
    protected Plane Plane;

    [SerializeField]
    private float zoomthreshhold = 0;
    [SerializeField]
    //threshhold factor of camera when scrolling;
    private float movethershhold_factor = 1;
    [SerializeField]
    private float move_sensitivity;


    private float move_inertia = 0;
    private bool inertiatoggle = false;

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
        var Delta2 = Vector3.zero;

        //Scroll
        //Scroll when touchcount is 1
        if (Input.touchCount == 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                //Debug.Log("xvalue : " + Delta1.x + "   yvalue : " + Delta1.y + "    zvalue : " + Delta1.z);
                Camera.transform.Translate(Delta1 * movethershhold_factor, Space.World);

            
            //Debug.Log(Mathf.Abs(Delta1.x + Delta1.z));
            /*
            if (Mathf.Abs(Delta1.x)+Mathf.Abs(Delta1.z)>=10)
            {
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

            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
        }

    }
    
    //Camera.rotation 38.68 -175.459 0
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

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

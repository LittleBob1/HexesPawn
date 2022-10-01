using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateAroundAndZoom : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float sensitivity = 3; // чувствительность мышки
    public float sensitivityY = 3; // чувствительность мышки
    public float limit = 80; // ограничение вращения по Y
    public float zoom = 0.25f; // чувствительность при увеличении, колесиком мышки
    public float zoomMax = 10; // макс. увеличение
    public float zoomMin = 3; // мин. увеличение
    public float X, Y;
    public float ratioBack = 0.1f;
    public float lerpZ = -9;
    public float lerpY = -61;
    public float lerpX = 180;

    private float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    private Vector2 firstTouchPrevPos, secondTouchPrevPos;

    public bool sideIsChange = false;

    void Start()
    {
        limit = Mathf.Abs(limit);
        if (limit > 90) limit = 90;
        offset = new Vector3(offset.x, offset.y, -11.5f);
        transform.position = target.position + offset;
        transform.position = new Vector3(6, 8, 11.5f);
        transform.rotation = Quaternion.Euler(53, 179, 0);
    }

    void Update()
    {

        if (Input.touchCount == 2)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
            {
                Touch firstTouch = Input.GetTouch(0);
                Touch secondTouch = Input.GetTouch(1);

                firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
                secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

                touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
                touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

                zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoom;

                if (touchesPrevPosDifference > touchesCurPosDifference)
                    offset.z -= zoomModifier;
                if (touchesPrevPosDifference < touchesCurPosDifference)
                    offset.z += zoomModifier;
            }
        }

        if (Input.touchCount == 1)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    X = transform.localEulerAngles.y + touch.deltaPosition.x * sensitivity;
                    Y += touch.deltaPosition.y * sensitivityY;
                    Y = Mathf.Clamp(Y, -limit, limit);
                    transform.localEulerAngles = new Vector3(-Y, X, 0);
                }
            }
        }
       
        if (Input.touchCount == 0 || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            if (sideIsChange)
            {
                X = Mathf.Lerp(X, lerpX, ratioBack);
            }
            Y = Mathf.Lerp(Y, lerpY, ratioBack);
            offset.z = Mathf.Lerp(offset.z, lerpZ, ratioBack);
            Y = Mathf.Clamp(Y, -limit, limit);
            transform.localEulerAngles = new Vector3(-Y, X, 0);
        }
        offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));
        transform.position = transform.localRotation * offset + target.position;
    }

    /*
    public Camera Camera;
    public bool Rotate;
    protected Plane Plane;
    public float DecreaseCameraPanSpeed = 1; //Default speed is 1
    public float CameraUpperHeightBound; //Zoom out
    public float CameraLowerHeightBound; //Zoom in

    private Vector3 cameraStartPosition;
    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;

        cameraStartPosition = Camera.transform.position;
    }

    private void Update()
    {
        Camera.transform.position = new Vector3(Mathf.Clamp(Camera.transform.position.x, -15, 15), Camera.transform.position.y, Mathf.Clamp(Camera.transform.position.z, -15, 15));

        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll (Pan function)
        if (Input.touchCount >= 1)
        {
            //Get distance camera should travel
            Delta1 = PlanePositionDelta(Input.GetTouch(0)) / DecreaseCameraPanSpeed;
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                Camera.transform.Translate(Delta1, Space.World);
            Camera.transform.position = new Vector3(Mathf.Clamp(Camera.transform.position.x, -15, 15), Camera.transform.position.y, Mathf.Clamp(Camera.transform.position.z, -15, 15));
        }

        //Pinch (Zoom Function)
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);

            //edge case
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            Vector3 camPositionBeforeAdjustment = Camera.transform.position;
            Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);

            //Restricts zoom height 

            //Upper (ZoomOut)
            if (Camera.transform.position.y > (cameraStartPosition.y + CameraUpperHeightBound))
            {
                Camera.transform.position = camPositionBeforeAdjustment;
            }
            //Lower (Zoom in)
            if (Camera.transform.position.y < (cameraStartPosition.y - CameraLowerHeightBound) || Camera.transform.position.y <= 1)
            {
                Camera.transform.position = camPositionBeforeAdjustment;
            }


            //Rotation Function
            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
        }

    }

    //Returns the point between first and final finger position
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;


        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
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
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class HandStateTracker : MonoBehaviour
{
    public bool mockHand = false;
    GameObject grabHand;
    GameObject trackHand;
    private Kinect.HandState state = Kinect.HandState.Unknown;
    HandSide side = HandSide.LEFT;

    GameObject grabbedObject;
    Mover moverScript;
    float dragValue = 0;
    Vector3 startPos;

    bool grabFlag = false;
    bool releaseFlag = false;
    bool flipFlop = true;

    public Kinect.HandState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            switch (state)
            {
                case Kinect.HandState.Closed:
                    if (flipFlop){
                        grabFlag = true;
                        flipFlop = false;
                    }

                    break;
                case Kinect.HandState.Open:
                    if (!flipFlop) {
                        releaseFlag = true;
                        flipFlop = true;
                    }
                    break;
                default:
                    flipFlop = true;
                    releaseFlag = true;
                    break;

            }
        }
    }

    public HandSide Side { get => side; set => side = value; }

    // Start is called before the first frame update
    void Start()
    {
        startPos = Vector3.zero;
        grabHand = Instantiate(Resources.Load<GameObject>("Objects/Grab_indicator") as GameObject,transform);
        grabHand.GetComponent<Collider>().enabled = false;
        trackHand = Instantiate(Resources.Load<GameObject>("Objects/Track_indicator") as GameObject, transform);
        trackHand.GetComponent<Collider>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        bool rayHit = Physics.Raycast(ray, out hit, 100.0f, 1<<LayerMask.NameToLayer("KinectTouchable"));

        if (mockHand) {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                State = Kinect.HandState.Closed;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                State = Kinect.HandState.Open;
            }
        }

        if (grabFlag)
        {
            grabHand.SetActive(true);
            trackHand.SetActive(false);
            grabFlag = false;
            if (rayHit && hit.transform != null && hit.transform.GetComponent<Mover>() != null && hit.transform.GetComponent<Mover>().grabber == null)
            {
                grabbedObject = hit.transform.gameObject;
                moverScript = hit.transform.GetComponent<Mover>();
                moverScript.Grab(gameObject);
                startPos = screenPoint;
            }
        }
        if (releaseFlag)
        {
            grabHand.SetActive(false);
            trackHand.SetActive(true);
            releaseFlag = false;
            if (moverScript != null)
            {
                moverScript.Release();
                grabbedObject = null;
            }
        }

        if (grabbedObject != null)
        {
            dragValue = screenPoint.x - startPos.x;
            if (moverScript.grabber = gameObject) {
                moverScript.Drag(dragValue, .01f, gameObject);
            }
            }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class HandStateTracker : MonoBehaviour
{
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
                    grabHand.SetActive(true);
                    trackHand.SetActive(false);
                    if (flipFlop){
                        grabFlag = true;
                        flipFlop = false;
                    }

                    break;
                case Kinect.HandState.Open:
                    grabHand.SetActive(false);
                    trackHand.SetActive(true);
                    if (!flipFlop) {
                        releaseFlag = true;
                        flipFlop = true;
                    }
                    break;
                default:
                    flipFlop = true;
                    releaseFlag = true;
                    grabHand.SetActive(false);
                    trackHand.SetActive(false);
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

        if (grabFlag)
        {
            grabFlag = false;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null && hit.transform.GetComponent<Mover>() != null)
                {
                    grabbedObject = hit.transform.gameObject;
                    moverScript = hit.transform.GetComponent<Mover>();
                    startPos = screenPoint;
                }
            }
        }
        if (releaseFlag)
        {
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
            moverScript.Drag(dragValue, .01f);
        }

    }
}

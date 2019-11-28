using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;
using UnityEngine.UI;

public class HandStateTracker : MonoBehaviour
{
    public bool mockHand = false;
    GameObject hand;
    GameObject ring;
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
    bool visible = true;
    Vector3 ringScaleTarget;
    GameLogic gameLogic;
    
    float dampenedState = 0; // 0 not grabbed, 1 grabbed
    float switchAfter = 0.2f;

    public Kinect.HandState State
    {
        get
        {
            return state;
        }
        set
        {
            switch (value)
            {
                case Kinect.HandState.Closed:
                        dampenedState = 1;
                    

                    break;
                case Kinect.HandState.Open:
                    break;
                default:
                    break;

            }
        }
    }

    public HandSide Side { get => side; set => side = value; }

    // Start is called before the first frame update
    void Start()
    {
        ringScaleTarget = Vector3.one;
        startPos = Vector3.zero;
        hand = Instantiate(Resources.Load<GameObject>("Objects/Hand_marker") as GameObject, GameObject.Find("Canvas").transform);
        Color holder = new Color(0, 1, 0);
        //holder.a = 0.5f;
        foreach (Transform t in hand.transform)
        {
            t.gameObject.GetComponent<Image>().color = holder;
        }
        ring = hand.transform.GetChild(hand.transform.childCount-1).gameObject;
        if (mockHand) {
            visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameLogic = GameObject.FindObjectOfType<GameLogic>();

        dampenedState = Mathf.Clamp01(dampenedState - Time.deltaTime / switchAfter);

        if (dampenedState > 0)
        {
            if (state != Kinect.HandState.Closed) {
                state = Kinect.HandState.Closed;
                grabFlag = true;
            }
        }
        else
        {
            if (state != Kinect.HandState.Open)
            {
                state = Kinect.HandState.Open;
                releaseFlag = true;
            }

        }



        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        bool rayHit = Physics.Raycast(ray, out hit, 100.0f, 1<<LayerMask.NameToLayer("KinectTouchable"));

        if (mockHand) {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                visible = true;
                State = Kinect.HandState.Closed;
                gameLogic.SleepRobot(gameObject);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                visible = false;
                State = Kinect.HandState.Open;
                gameLogic.SleepRobot(gameObject);
            }
        }

        if (visible)
        {
            ring.transform.localScale = Vector3.Lerp(ring.transform.localScale, ringScaleTarget, Time.deltaTime*10);
            hand.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            hand.SetActive(true);
            Debug.Log(hand.transform.position);
        }
        else
        {
            hand.SetActive(false);
        }

        if (grabFlag)
        {
            ringScaleTarget = Vector3.one * 0.5f;
            grabFlag = false;
            if (rayHit && hit.transform != null && !gameLogic.waitingForReset && hit.transform.GetComponent<Mover>() != null && hit.transform.GetComponent<Mover>().grabber == null)
            {
                grabbedObject = hit.transform.gameObject;
                moverScript = hit.transform.GetComponent<Mover>();
                moverScript.Grab(gameObject);
                startPos = screenPoint;
                gameLogic.SleepRobot(gameObject);
            }
        }
        if (releaseFlag)
        {
            ringScaleTarget = Vector3.one;
            releaseFlag = false;
            if (moverScript != null)
            {
                moverScript.Release();
                grabbedObject = null;
                moverScript = null;
                gameLogic.SleepRobot(gameObject);
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
    private void OnDestroy()
    {
        Debug.Log("Handstate tracker destroyed");
        if (moverScript != null) { 
        moverScript.Release();
        }
        Destroy(hand);
        Destroy(ring);
    }
}

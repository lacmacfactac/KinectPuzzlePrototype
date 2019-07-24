using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    Animator animator;
    public bool grabbed = false;
    int stateCount;
    public int state = 0;
    Transform[] children;


    public float phase = 0;
    float targetPhase = 0;
    float deltaPhase = 0;
    public float normalizedPhase;
    int temporaryState;
    bool settled = false;
    bool scrambling = false;
    public GameObject grabber;
    Mover[] otherOrigins;
    float scaleSetpoint;
    float minimumScale = 0.8f;
    float externalScaleModifier = 1;


    // Start is called before the first frame update
    void Start()
    {
        scaleSetpoint = 1;
        stateCount = transform.childCount;
        animator = transform.GetComponent<Animator>();
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            children[i] = transform.GetChild(i);
        }
        otherOrigins = GameObject.FindObjectsOfType<Mover>();
    }

    // Update is called once per frame
    void Update()
    {

            if (!grabbed && !settled)
            {
                phase = Mathf.Lerp(phase, targetPhase, Time.deltaTime * 10);
                if (Mathf.Abs(phase - targetPhase) < 0.01f)
                {
                    phase = targetPhase;
                    settled = true;
                }

            }
        float localPhase = phase + deltaPhase;
        

        if (localPhase >= 0)
        {
            localPhase = localPhase % stateCount;
        }
        else {
            localPhase = stateCount - (Mathf.Abs(localPhase)%stateCount);
        }


        normalizedPhase = localPhase % 1;

        temporaryState = (int)localPhase;

        if (!grabbed && settled)
        {
            state = temporaryState;
        }



        animator.SetFloat("State", normalizedPhase);


        for (int i = 0; i < stateCount; i++)
        {
            int wrappedState = (i + temporaryState + stateCount - 1) % stateCount;

            //??????
            /*
            while (wrappedState < 0) {
                wrappedState += stateCount;
            }
            */
            //??????
            // was 4 before ˇˇˇˇˇˇˇˇ
            if (i < 4)
            {
                if (i == 1 || grabber != null || !settled)
                {

                    if (!children[wrappedState].gameObject.activeSelf)
                    {
                        children[wrappedState].gameObject.SetActive(true);
                    }
                    children[wrappedState].localRotation = Quaternion.Euler(0, (i - 1) * 90, 0);
                }
                else
                {
                    children[wrappedState].gameObject.SetActive(false);

                }

            }
            else
            {
                children[wrappedState].gameObject.SetActive(false);
            }
        }

        foreach (Mover origin in otherOrigins) {
            if (origin != this) {
                float dist = (origin.transform.position-transform.position).magnitude;
                if (dist < 3) {
                    origin.ScaleTo(1-dist/3);
                }
            }
        }

        externalScaleModifier = Mathf.Lerp(externalScaleModifier, 1, Time.deltaTime);
        float localScaleSetpoint = Mathf.Min(externalScaleModifier, scaleSetpoint);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one*localScaleSetpoint, Time.deltaTime*5);
        /*
        if (grabber != null)
        {
            transform.localScale = new Vector3(.8f, .8f, .8f);
        }
        else
        {
            transform.localScale = Vector3.one;

        }
        */
    }

    public void ScaleTo(float t) {
        /*
        if (t*(1-minimumScale)+minimumScale < externalScaleModifier) {
            externalScaleModifier = (1-t) * (minimumScale);
        }
        */

    }

    public void Grab(GameObject grabberObject) {
        grabber = grabberObject;
        scaleSetpoint = minimumScale;
    }

    public void Drag(float val, float scale, GameObject source)
    {
        if (source == grabber) {
            deltaPhase = val * scale;
            grabbed = true;
            settled = false;
        }
        
    }
    public void Release()
    {
        scaleSetpoint = 1;
        grabbed = false;
        phase += deltaPhase;
        deltaPhase = 0;
        targetPhase = Mathf.Round(phase);
        grabber = null;
    }
    public void Set(int s)
    {
        Debug.Log("Jumpig to state " + s);
        grabbed = false;
        settled = false;
        targetPhase = phase + (s - (phase % stateCount));

    }
    public int GetState() {
        if (!scrambling && settled && !grabbed) {
            return state;
        } else
        {
            return -1;
        }

    }
}

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


    // Start is called before the first frame update
    void Start()
    {
        stateCount = transform.childCount;
        animator = transform.GetComponent<Animator>();
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            children[i] = transform.GetChild(i);
        }
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
            localPhase = stateCount - Mathf.Abs(localPhase);
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
            if (i < 4)
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
    }

    public void Drag(float val, float scale)
    {
        
        deltaPhase = val * scale;
        grabbed = true;
        settled = false;
        
    }
    public void Release() {
        grabbed = false;
        phase += deltaPhase;
        targetPhase = Mathf.Round(phase);
        deltaPhase = 0;
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

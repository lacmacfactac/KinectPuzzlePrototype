
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public AudioSource pingSource;
    public AudioSource dragSource;
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
    public bool settled = false;
    public bool reset = false;
    bool scrambling = false;
    public GameObject grabber;
    Mover[] otherOrigins;
    float scaleSetpoint;
    float minimumScale = 0.8f;
    float externalScaleModifier = 1;
    int[] order;
    Vector3 defaultPos;
    Vector3 displacement;

    float lastInteracted = 0;


    // Start is called before the first frame update

    private void Awake()
    {
        //   clip = Resources.LoadAsync("Audio/Drag.wav") as AudioClip;
    }


    void Start()
    {
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = clip;
        scaleSetpoint = 1;
        stateCount = transform.childCount;
        order = new int[stateCount];
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }
        order = ShuffleOrder(order);
        animator = transform.GetComponent<Animator>();
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        otherOrigins = GameObject.FindObjectsOfType<Mover>();
        defaultPos = transform.position;
        displacement = new Vector3(0,0,-0.1f);
    }

    // Update is called once per frame
    void Update()
    {

        if (dragSource != null)
        {
            if (settled)
            {
                if (dragSource.isPlaying)
                {
                    Debug.Log("Audio stopped on " + gameObject.name);

                    dragSource.Pause();
                }
            }
            else
            {
                if (!dragSource.isPlaying)
                {
                    Debug.Log("Audio started on " + gameObject.name);
                    if (dragSource.time == 0)
                    {
                        dragSource.Play();
                    }
                    dragSource.UnPause();
                }
            }
        }



        if (!grabbed && !settled)
        {
            phase = Mathf.Lerp(phase, targetPhase, (Time.deltaTime * 6));
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
        else
        {
            localPhase = stateCount - (Mathf.Abs(localPhase) % stateCount);
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
            //int wrappedState = order[(i + temporaryState + stateCount - 1) % stateCount];
            int wrappedState = (i + temporaryState + stateCount - 1) % stateCount;
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
        float localScaleSetpoint = !grabbed || settled ? 1 : 0.8f;
        Vector3 posSetpoint = !grabbed || settled ? defaultPos : defaultPos + displacement;
        transform.position = Vector3.Lerp(transform.position, posSetpoint, Time.deltaTime * 5);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * localScaleSetpoint, Time.deltaTime * 5);
    }

    public void ScaleTo(float t)
    {

    }

    public void Grab(GameObject grabberObject)
    {
        grabber = grabberObject;
        scaleSetpoint = minimumScale;
        reset = false;
    }

    public void Drag(float val, float scale, GameObject source)
    {
        if (source == grabber)
        {
            deltaPhase = val * scale;
            grabbed = true;
            settled = false;
            lastInteracted = Time.time;
            GameObject.FindObjectOfType<GameLogic>().EnableRobot(false);
        }

    }

    public float TimeSinceLastIntraction()
    {
        return Time.time - lastInteracted;
    }

    public void Release()
    {
        scaleSetpoint = 1;
        grabbed = false;
        phase += deltaPhase;
        deltaPhase = 0;
        targetPhase = Mathf.Round(phase);
        grabber = null;
        if (pingSource != null)
        {
            pingSource.Play();
        }
    }
    public void Set(int s)
    {
        Debug.Log("Jumpig to state " + s);
        grabbed = false;
        settled = false;
        targetPhase = phase + (s - (phase % stateCount));
        lastInteracted = Time.time;

    }
    public void SetToRandom()
    {
        int s = (int)((Random.value * (stateCount) + 1));
        Set(s);

    }

    public void MockInteract() {
        lastInteracted = Time.time;
    }
    public int GetState()
    {
        if (settled && !grabbed)
        {
            //return order[state];
            return state;
        }
        else
        {
            return -1;
        }

    }

    int[] ShuffleOrder(int[] order)
    {

        for (int i = 0; i < order.Length; i++)
        {
            int other = (int)(Random.value * (order.Length - 1));
            int holder = order[i];
            order[i] = order[other];
            order[other] = holder;
        }
        return order;
    }
}
/* 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public AudioSource pingSource;
    public AudioSource dragSource;
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
    public bool settled = false;
    public bool reset = false;
    bool scrambling = false;
    public GameObject grabber;
    Mover[] otherOrigins;
    float scaleSetpoint;
    float minimumScale = 0.8f;
    float externalScaleModifier = 1;
    int[] order;


    // Start is called before the first frame update

    private void Awake()
    {
        //   clip = Resources.LoadAsync("Audio/Drag.wav") as AudioClip;
    }


    void Start()
    {
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = clip;
        scaleSetpoint = 1;
        stateCount = transform.childCount;
        order = new int[stateCount];
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }
        order = ShuffleOrder(order);
        animator = transform.GetComponent<Animator>();
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        otherOrigins = GameObject.FindObjectsOfType<Mover>();
    }

    // Update is called once per frame
    void Update()
    {

        if (dragSource != null)
        {
            if (settled)
            {
                if (dragSource.isPlaying)
                {
                    Debug.Log("Audio stopped on " + gameObject.name);

                    dragSource.Pause();
                }
            }
            else
            {
                if (!dragSource.isPlaying)
                {
                    Debug.Log("Audio started on " + gameObject.name);
                    if (dragSource.time == 0)
                    {
                        dragSource.Play();
                    }
                    dragSource.UnPause();
                }
            }
        }



        if (!grabbed && !settled)
        {
            phase = Mathf.Lerp(phase, targetPhase, (Time.deltaTime * 6));
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
        else
        {
            localPhase = stateCount - (Mathf.Abs(localPhase) % stateCount);
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
            int wrappedState = order[(i + temporaryState + stateCount - 1) % stateCount];
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
        float localScaleSetpoint = !grabbed || settled ? 1 : 0.8f;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * localScaleSetpoint, Time.deltaTime * 5);
    }

    public void ScaleTo(float t)
    {

    }

    public void Grab(GameObject grabberObject)
    {
        grabber = grabberObject;
        scaleSetpoint = minimumScale;
        reset = false;
    }

    public void Drag(float val, float scale, GameObject source)
    {
        if (source == grabber)
        {
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
        if (pingSource != null)
        {
            pingSource.Play();
        }
    }
    public void Set(int s)
    {
        Debug.Log("Jumpig to state " + s);
        grabbed = false;
        settled = false;
        targetPhase = phase + (s - (phase % stateCount));

    }
    public void SetToRandom()
    {
        int s = (int)((Random.value * (stateCount) + 1));
        Debug.Log("Jumpig to state " + s);
        grabbed = false;
        settled = false;
        targetPhase = (int)(phase + s);

    }
    public int GetState()
    {
        if (!scrambling && settled && !grabbed)
        {
            return order[state];
        }
        else
        {
            return -1;
        }

    }

    int[] ShuffleOrder(int[] order)
    {

        for (int i = 0; i < order.Length; i++)
        {
            int other = (int)(Random.value * (order.Length - 1));
            int holder = order[i];
            order[i] = order[other];
            order[other] = holder;
        }
        return order;
    }
}

 */

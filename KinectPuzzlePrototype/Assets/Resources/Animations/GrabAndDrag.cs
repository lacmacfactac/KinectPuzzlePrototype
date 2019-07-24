using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndDrag : MonoBehaviour
{
    GameObject grabbedObject;
    Mover moverScript;
    float dragValue = 0;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null && hit.transform.GetComponent<Mover>() != null && hit.transform.GetComponent<Mover>().grabber == null)
                {
                    grabbedObject = hit.transform.gameObject;
                    moverScript = hit.transform.GetComponent<Mover>();
                    moverScript.grabber = transform.gameObject;
                    startPos = Input.mousePosition;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (moverScript != null) {
                moverScript.Release();
                grabbedObject = null;
            }
        }

        if (grabbedObject != null) {
            dragValue = Input.mousePosition.x - startPos.x;
            moverScript.Drag(dragValue, .01f);
        }

    }

    public void Grab() {

    }

    public void Release() {


    }
}

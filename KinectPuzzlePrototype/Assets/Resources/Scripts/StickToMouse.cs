using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToMouse : MonoBehaviour
{
    public GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction*100);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 40) && hit.collider.gameObject == plane) {
            transform.position = hit.point;
        }
    }
}

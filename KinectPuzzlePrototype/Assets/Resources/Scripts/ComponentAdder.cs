using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentAdder : MonoBehaviour
{

    RuntimeAnimatorController animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = Resources.Load("Animations/Cube") as RuntimeAnimatorController;


        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Origin"))
        {
            g.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            g.AddComponent<MeshCollider>().sharedMesh = g.transform.parent.GetComponent<MeshFilter>().sharedMesh;
            g.AddComponent<Animator>().runtimeAnimatorController = animator;
            g.AddComponent<Mover>();
        }

        GameObject.Find("Solution").AddComponent<SolutionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

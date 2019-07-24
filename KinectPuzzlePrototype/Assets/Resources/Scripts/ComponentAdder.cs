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
            GameObject colliderMeshSource = null;
            foreach (GameObject source in GameObject.FindGameObjectsWithTag("Collider")) {
                if (source.transform.parent == transform.parent) {
                    colliderMeshSource = source;
                }
            }

            colliderMeshSource.GetComponent<MeshRenderer>().enabled = false;
            g.AddComponent<MeshCollider>().sharedMesh = colliderMeshSource.GetComponent<MeshFilter>().sharedMesh;
            g.AddComponent<Animator>().runtimeAnimatorController = animator;
            g.AddComponent<Mover>();
        }

        Debug.Log("Origins: " + GameObject.FindGameObjectsWithTag("Origin").Length);
        //GameObject.Find("Solution").AddComponent<SolutionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

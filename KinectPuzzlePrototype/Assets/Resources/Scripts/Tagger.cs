﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tagger : MonoBehaviour
{
    public PartialNameTagPair[] partialNameTagPair;
    public string loadObject = "Assets/Resources/Objects/template.glb";
    public GameObject loadedObject;
    RuntimeAnimatorController animator;
    public bool loadGltf = false;
    // Start is called before the first frame update

    private void Awake()
    {
        if (loadGltf) {
            loadedObject = UniGLTF.gltfImporter.Load(loadObject).Root;
            loadedObject.transform.parent = transform;
        }
    }


    void Start()
    {
        Transform[] allImported = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform item in allImported)
        {
            // add object to hold parameters before final tagging and parenting
            ObjectCategory holder = item.gameObject.AddComponent<ObjectCategory>();

            // if item has @, it needs parenting
            if (item.name.Contains("@"))
            {
                // to the left of @ is the name, to the right is the parents name
                string[] splitName = item.name.Split('@');

                // get rid of blender's numbering
                for (int i = 0; i < splitName.Length; i++)
                {
                    if (splitName[i].Contains("."))
                    {
                        splitName[i] = splitName[i].Split('.')[0];
                    }
                }

                // store names
                holder.temporaryName = splitName[0];
                holder.parentName = splitName[1];

                // match names with possible tags
                foreach (PartialNameTagPair nt in partialNameTagPair)
                {

                    if (holder.temporaryName.Contains(nt.partialName))
                    {
                        holder.temporaryTag = nt.tag;
                        item.tag = nt.tag;
                    }
                }

                // otherwise it might be a root object still needing to be tagged, or a misc object, that needs to be left untouched
            }
            else
            {

                holder.temporaryName = item.name;
                foreach (PartialNameTagPair nt in partialNameTagPair)
                {
                    if (item.name.Contains(nt.partialName))
                    {
                        holder.temporaryTag = nt.tag;
                        item.tag = nt.tag;
                    }
                }

            }

        }

        foreach (Transform child in allImported)
        {
            foreach (Transform parent in allImported)
            {
                if (child.gameObject.GetComponent<ObjectCategory>().parentName == parent.gameObject.GetComponent<ObjectCategory>().temporaryName) {
                    child.parent = parent;
                }
            }
        }



        animator = Resources.Load("Animations/Cube") as RuntimeAnimatorController;

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Origin"))
        {
            g.AddComponent<Mover>();
            GameObject colliderMeshSource = null;
            foreach (GameObject source in GameObject.FindGameObjectsWithTag("Collider"))
            {
                Debug.Log("Object with collider tag: " + source.name);
                if (source.transform.parent == g.transform.parent)
                {
                    colliderMeshSource = source;
                    Debug.Log("Collider found at " + source.name);
                }
            }

            colliderMeshSource.GetComponent<MeshRenderer>().enabled = false;
            g.AddComponent<MeshCollider>().sharedMesh = colliderMeshSource.GetComponent<MeshFilter>().sharedMesh;
            g.AddComponent<Animator>().runtimeAnimatorController = animator;
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Element"))
        {
            if (g.GetComponent<MeshRenderer>() != null)
            {
                g.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Origin"))
        {
            if (g.GetComponent<MeshRenderer>() != null)
            {
                g.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Slot"))
        {
            if (g.GetComponent<MeshRenderer>() != null)
            {
                g.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        
        //GameObject.Find("Solution").AddComponent<SolutionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

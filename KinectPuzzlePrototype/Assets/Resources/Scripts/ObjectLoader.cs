using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour
{

    public GameObject loadedObject;
    // Start is called before the first frame update
    private void Awake()
    {
        loadedObject = UniGLTF.gltfImporter.Load("Assets/Resources/Objects/template.glb").Root;
        loadedObject.transform.parent = transform;
            
            }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour
{
    public string loadObject = "Assets/Resources/Objects/template.glb";
    public GameObject loadedObject;
    // Start is called before the first frame update
    private void Awake()
    {
        loadedObject = UniGLTF.gltfImporter.Load(loadObject).Root;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagger : MonoBehaviour
{
    public PartialNameTagPair[] partialNameTagPair;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>()) {
            foreach (PartialNameTagPair nt in partialNameTagPair) {
                if (g.name.Contains(nt.partialName) ){
                    g.tag = nt.tag;
                }
            }

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

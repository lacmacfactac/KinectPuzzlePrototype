using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipFlop : MonoBehaviour
{
    public GameObject easyModeRoot;
    public GameObject normalModeRoot;
    bool startUp = true;
    bool easyModeEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (startUp || Input.GetKeyDown(KeyCode.M))
        {
            startUp = false;
            easyModeRoot.SetActive(easyModeEnabled);
            normalModeRoot.SetActive(!easyModeEnabled);
            easyModeEnabled = !easyModeEnabled;
        }        
    }
}

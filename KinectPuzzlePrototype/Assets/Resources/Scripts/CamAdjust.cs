using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using System.IO;

[SerializeField]
public class CamParameters {


    public int stepSizePower = -1;
    public Vector3 pos;
    public Quaternion rot;
    public float focal = 50;

}

public class CamAdjust : MonoBehaviour
{
    CamParameters param;
    Camera cam;

    bool changed = true;
    float printUntil = 0;
    float stepSize;

    // Start is called before the first frame update
    void Start()
    {
        param = new CamParameters();
        if (File.Exists(Application.dataPath + "/Saves/CamParam.json"))
        {
            param = Load(Application.dataPath + "/Saves/CamParam.json");
        }
        else {
            param.rot = transform.rotation;
            param.pos = transform.position;
            Save(param, Application.dataPath + "/Saves/CamParam.json");
        }

        stepSize = Mathf.Pow(10, param.stepSizePower);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            printUntil = Time.time + 5;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            changed = true;
            param = Load(Application.dataPath + "/Saves/CamParam.json");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            changed = true;
            param.stepSizePower += 1;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            changed = true;
            param.stepSizePower -= 1;
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            changed = true;
            param.pos += new Vector3(0, 0, -stepSize);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            changed = true;
            param.pos += new Vector3(0, 0, stepSize);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            changed = true;
            param.pos += new Vector3(stepSize, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            changed = true;
            param.pos += new Vector3(-stepSize, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            changed = true;
            param.pos += new Vector3(0, stepSize, 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            changed = true;
            param.pos += new Vector3(0, -stepSize, 0);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            changed = true;
            param.rot.eulerAngles += new Vector3(0, -stepSize, 0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            changed = true;
            param.rot.eulerAngles += new Vector3(0, stepSize, 0);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            changed = true;
            param.focal += stepSize;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            changed = true;
            param.focal -= stepSize;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            changed = true;
            Save(param, Application.dataPath + "/Saves/CamParam.json");
        }




        if (changed) {
            changed = !changed;
            stepSize = Mathf.Pow(10, param.stepSizePower);
            transform.position = param.pos;
            cam.focalLength = param.focal;
            transform.rotation = param.rot;
        }
    }

    public CamParameters Load(string path)
    {
        return JsonUtility.FromJson<CamParameters>(File.ReadAllText(path));

    }
    public void Save(CamParameters param, string path)
    {
        string jsonData = JsonUtility.ToJson(param, true);
        File.WriteAllText(path, jsonData);
    }

    private void OnGUI()
    {
        if (Time.time < printUntil)
        {
            Rect posDataArea = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 100, 200, 500);
            string data = "Pos: " + param.pos.ToString() + " [a-d w-s r-f]\n" +
                "Rot: " + param.rot.eulerAngles.ToString() + " [q-e]\n" +
                "Focal l: " + param.focal.ToString() + " [z-h]\n" +
                "Stepsize: " + Mathf.Pow(10, param.stepSizePower).ToString() + " [t-g]\n" +
                "Load/Save: [i-o]";
            GUI.Label(posDataArea, data);
        }
    }
}

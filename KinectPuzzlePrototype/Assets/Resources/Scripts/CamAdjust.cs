using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using System.IO;

[SerializeField]
public class CamParameters {


    public int stepSizePower = -1;
    public Vector3 pos = new Vector3(0,0,8.84f);
    public Quaternion rot = new Quaternion(0,1,0,0);
    public float focal = 17.7f;
    public float kinectZoom = 2;

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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changed = true;
            param.rot.eulerAngles += new Vector3(0, -stepSize, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changed = true;
            param.rot.eulerAngles += new Vector3(0, stepSize, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            changed = true;
            param.rot.eulerAngles += new Vector3(stepSize,0, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            changed = true;
            param.rot.eulerAngles += new Vector3(-stepSize,0, 0);
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            changed = true;
            param.kinectZoom += stepSize;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            changed = true;
            param.kinectZoom -= stepSize;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            changed = true;
            param = new CamParameters();
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
            GameObject.FindObjectOfType<KinectInput>().zoom = param.kinectZoom;
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
            string data =
                "   [W]   [R]\n" +
                "[A][S][D][F]\n" +
                "Pos: " + param.pos.ToString() + "\n" +
                "\n" +
                "   [^]   \n" +
                "[<][ˇ][>]\n" +
                "Rot: " + param.rot.eulerAngles.ToString() + "\n" +
                "\n" +
                "[Z]\n" +
                "[H]\n" +
                "Focal l: " + param.focal.ToString() + "\n" +
                "\n" +
                "[T]\n" +
                "[G]\n" +
                "Stepsize: " + Mathf.Pow(10, param.stepSizePower).ToString() + "\n" +
                "\n" +
                "[Y][X]\n" +
                "Kinect zoom: " + GameObject.FindObjectOfType<KinectInput>().zoom.ToString() + "\n"+ 
                "\n" +
                "[I][O]\n" +
                "Load/Save\n" +
                "\n" +
                "[SPACE]\n" +
                "Scramble now" +
                "\n" +
                "[ESC]\n" +
                "Quit"
                ;
            GUI.Label(posDataArea, data);
        }
    }
}

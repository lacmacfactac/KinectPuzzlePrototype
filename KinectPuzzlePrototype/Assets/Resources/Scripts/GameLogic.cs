﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    public bool correctSolution = false;
    public int solutionIndex = -1;
    GameObject[] solutions;
    List<GameObject> puzzleSockets;
    // GameObject[] puzzleSockets;
    GameObject puzzle;
    SolutionHandler solutionHandler;
    public bool waitingForReset = false;
    bool firstRun = true;
    public GameObject winAnimation;
    float lastPerformedRobot = 0;
    public float averageAllowedIdleTime = 3;
    float realIdleTime;
    bool robotEnabled = false;
    bool scrambleRan = false;
    float robotSleepTime = 0;
    int runningCoroutines = 0;
    // Start is called before the first frame update
    void Start()
    {
        realIdleTime = averageAllowedIdleTime;
        //solutionHandler = GameObject.FindObjectOfType<SolutionHandler>();
        //solutions = GameObject.FindGameObjectsWithTag("Complete");
        puzzleSockets = new List<GameObject>();
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform item in allChildren) {
            if (item.CompareTag("Origin"))
            {
                puzzleSockets.Add(item.gameObject);
            }
        }

        //puzzleSockets = GameObject.FindGameObjectsWithTag("Origin");
        puzzle = GameObject.Find("Puzzle");


        waitingForReset = true;
        Scramble();
        SleepRobot(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        robotSleepTime -= Time.deltaTime;
        robotSleepTime = robotSleepTime < 0 ? 0 : robotSleepTime;
        if (robotSleepTime == 0)
        {
            robotEnabled = true;
        }
        else {
            robotEnabled = false;
        }
        /*
        if (firstRun) {
            firstRun = false;
            Scramble();
        }
        */
        

        if (!waitingForReset)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scramble();
            }

            correctSolution = true;
            float maxIdleTime = 0;
            for (int i = 0; i < puzzleSockets.Count; i++)
            {
                int next = (i + 1) % puzzleSockets.Count;
                if (
                    puzzleSockets[i].GetComponent<Mover>().GetState() != -1 &&
                    puzzleSockets[i].GetComponent<Mover>().GetState() != puzzleSockets[next].GetComponent<Mover>().GetState()
                    )
                {
                    correctSolution = false;
                }
            }

            if (robotEnabled && Time.time-lastPerformedRobot > realIdleTime) {
                lastPerformedRobot = Time.time;
                realIdleTime = Mathf.Lerp(averageAllowedIdleTime / 2, averageAllowedIdleTime * 2, Random.value);
                puzzleSockets[(int)Random.Range(0, puzzleSockets.Count - 1)].GetComponent<Mover>().SetToRandom();
            }

            if (correctSolution)
            {
                waitingForReset = true;
                gameObject.GetComponent<Animator>().SetTrigger("Trigger");
                //solutionHandler.Solve(solutionIndex);
            }
        }


        if (scrambleRan && runningCoroutines <= 0) {
            runningCoroutines = 0;
            waitingForReset = false;
            scrambleRan = false;
        }
    }

    public void Scramble()
    {
        Debug.Log("Scrambling");
        for (int i = 0; i < puzzleSockets.Count; i++)
        {
            StartCoroutine(ScramblingRoutine(puzzleSockets[i], i * 0.15f));
            runningCoroutines += 1;
            StartCoroutine(ScramblingRoutine(puzzleSockets[i],1 + i * 0.15f));
            runningCoroutines += 1;
        }
        scrambleRan = true;
    }

    public void Reset()
    {
        Debug.Log("Game logic reset");
    }

    IEnumerator ScramblingRoutine(GameObject g, float delay)
    {
        yield return new WaitForSeconds(delay);
        g.GetComponent<Mover>().SetToRandom();
        runningCoroutines -= 1;
    }

    /*
    public void EnableRobot(bool value)
    {
        if (value != robotEnabled) {
            lastPerformedRobot = Time.time;
        }
        robotEnabled = value;
    }
    */

    public void SleepRobot(GameObject caller)
    {
        Debug.Log("Robot put to sleep by " + caller.name);
        SleepRobot(30);

    }
    public void SleepRobot(float delay)
    {
        robotSleepTime = delay;
        lastPerformedRobot = Time.time;

    }

    public bool IsRobotAwake() {

        return robotEnabled;
    }
}

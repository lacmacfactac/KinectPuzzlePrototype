using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionHandler : MonoBehaviour
{
    GameObject[] solutions;
    GameLogic gameLogic;
    GameObject puzzle;
    int solutionIndex = 0;
    bool toggle = false;
    float startTime;
    float interval = 2;
    // Start is called before the first frame update
    void Start()
    {
        //solutions = GameObject.FindGameObjectsWithTag("Complete");
        gameLogic = GameObject.FindObjectOfType<GameLogic>();
        //puzzle = GameObject.FindGameObjectWithTag ("Puzzle");
        /*
        foreach (GameObject g in solutions)
        {
            g.SetActive(false);
        }
        */

    }

    // Update is called once per frame
    void Update()
    {

        if(toggle && Time.time-startTime >= interval)
        {

            toggle = false;
            Unsolve();
        }
        
    }

    public void Solve(int index)
    {
        Debug.Log("Solving for " + index);
        //solutionIndex = solutions.Length-1-index;
        toggle = true;
        startTime = Time.time;
        //solutions[solutionIndex].SetActive(true);
        //puzzle.SetActive(false);
    }

    public void Unsolve()
    {
        Debug.Log("Finished solution anim");
        //puzzle.SetActive(true);
        //solutions[solutionIndex].SetActive(false);
        gameLogic.Reset();
    }
}

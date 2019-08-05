using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    public bool correctSolution = false;
    public int solutionIndex = -1;
    GameObject[] solutions;
    GameObject[] puzzleSockets;
    GameObject puzzle;
    SolutionHandler solutionHandler;
    public bool waitingForReset = false;
    bool firstRun = true;

    // Start is called before the first frame update
    void Start()
    {
        //solutionHandler = GameObject.FindObjectOfType<SolutionHandler>();
        //solutions = GameObject.FindGameObjectsWithTag("Complete");
        puzzleSockets = GameObject.FindGameObjectsWithTag("Origin");
        puzzle = GameObject.Find("Puzzle");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (firstRun) {
            firstRun = false;
            Scramble();
        }
        */
        if (!waitingForReset)
        {
            correctSolution = true;
            for (int i = 0; i < puzzleSockets.Length; i++)
            {
                int next = (i + 1) % puzzleSockets.Length;
                if (
                    !puzzleSockets[i].GetComponent<Mover>().settled || 
                    puzzleSockets[i].GetComponent<Mover>().GetState() == -1 ||
                    puzzleSockets[i].GetComponent<Mover>().GetState() != puzzleSockets[next].GetComponent<Mover>().GetState()
                    )
                {
                    correctSolution = false;
                    continue;
                }
                else
                {
                    solutionIndex = puzzleSockets[i].GetComponent<Mover>().GetState();
                }
            }

            if (correctSolution)
            {
                Scramble(2.0f);
                //solutionHandler.Solve(solutionIndex);
            }
        }
    }

    private void Scramble(float delay)
    {
        Debug.Log("Scrambling");
        for (int i = 0; i < puzzleSockets.Length; i++)
        {
            StartCoroutine(ScramblingRoutine(puzzleSockets[i], i * 0.15f));
        }
    }

    public void Reset()
    {
        Debug.Log("Game logic reset");
       // Scramble();
    }
    IEnumerator ScramblingRoutine(GameObject g, float delay)
    {
        yield return new WaitForSeconds(delay);
            g.GetComponent<Mover>().SetToRandom();
    }
}

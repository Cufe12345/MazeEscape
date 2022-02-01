using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardScript : MonoBehaviour
{
    GameObject player;
    GameObject solver;
    GameObject flag;
    GameObject camera;
    GameObject killText;
    GameObject elapsedText;
    GameObject deathText;
    GameObject solvedText;
    bool first = true;
    GameObject firstWall;
    GameObject lastWall;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        solver = GameObject.Find("EnemySolver");
        flag = GameObject.Find("FinishFlag");
        camera = GameObject.Find("Main Camera");
        killText = GameObject.Find("KillText1");
        elapsedText = GameObject.Find("ElapsedText");
        deathText = GameObject.Find("DeathText1");
        solvedText = GameObject.Find("EnemyText");
    }

    // Update is called once per frame
    void Update()
    {
        //runs when the maze has finished generating
        if (camera.GetComponent<MazeGeneration>().complete == true)
        {
            //sets the values of the stats
            killText.GetComponent<Text>().text = "KILLS: " + player.GetComponent<Health>().kills;
            deathText.GetComponent<Text>().text = "DEATHS: " + player.GetComponent<Health>().deaths;
            elapsedText.GetComponent<Text>().text = "TIME ELAPSED: " + flag.GetComponent<FinishGame>().timeElapsed + " SECONDS";
            if(first == true)
            {
                first = false;
                firstWall = GameObject.Find("Map").transform.GetChild(0).gameObject;
                lastWall = GameObject.Find("Map").transform.GetChild(GameObject.Find("Map").transform.childCount-1).gameObject;
            }
            solvedText.GetComponent<Text>().text = "ENEMY ESCAPED: " + CalculateSolver().ToString() + "%";
        }
    }
    //calculates how close enemy is to solving maze;
    public float CalculateSolver()
    {
        float actualDistance = (Vector3.Distance(solver.transform.position, flag.transform.position) / Vector3.Distance(firstWall.transform.position,lastWall.transform.position));
        actualDistance *= 100;
        actualDistance = 100 - actualDistance;  
        float final = (actualDistance);
        final = Mathf.Round(final);
        if(final > 100)
        {
            final = 100;
        }
        return final;
    }
}

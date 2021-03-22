using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    GameObject camera;
    MazeGeneration mazeGenerationScript;
    public bool generationComplete;
    bool first;
    bool[,] map;
    int i;
    int j;
    bool reset = false;
    bool reset2 = false;
    Animator walking;
    // Start is called before the first frame update
    void Start()
    {
        i = 49;
        j = 49;
        
        first = true;
        camera = GameObject.Find("Main Camera");
        mazeGenerationScript = camera.GetComponent<MazeGeneration>();
        generationComplete = false;
        transform.position = new Vector3(i * 50, 1, j * 50);
        walking = transform.GetComponent<Animator>();
        walking.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(reset == true)
        {
            if (mazeGenerationScript.resetMaze == false)
            {
                if (reset2 == false)
                {
                    reset2 = true;
                    Debug.Log("RESTTING MAZE");
                    first = true;
                    reset = false;
                    mazeGenerationScript.resetMaze = true;
                    mazeGenerationScript.complete = false;
                    generationComplete = false;
                    i = 49;
                    j = 49;
                    transform.position = new Vector3(i * 50, 1, j * 50);
                }
            }
        }
        if(mazeGenerationScript.complete == true)
        {
            generationComplete = true;
        }
        if(first == true)
        {
            if(generationComplete == true)
            {
                first = false;
                generationComplete = false;
                map = mazeGenerationScript.map;
                StartSolving();
                reset2 = false;
                
            }
        }
        Vector3 tempPos;
        tempPos.x = i*50;
        tempPos.y = transform.position.y;
        tempPos.z = j*50;
        if(transform.position.x != tempPos.x || transform.position.z != tempPos.z)
        {
            float rotationSpeed = Time.deltaTime * 50;
            Vector3 direction = (tempPos - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
            walking.enabled = true;
        }
        else
        {
            walking.enabled = false;
        }
        
        
        float speed = Time.deltaTime * 50;
        transform.position = Vector3.MoveTowards(transform.position, tempPos, speed);
        
    }

    public void Move(string direction)
    {
        try
        {
            if (direction.Equals("left"))
            {
                j -= 1;
            }
            if (direction.Equals("right"))
            {
                j += 1;
            }
            if (direction.Equals("up"))
            {
                i -= 1;
            }
            if (direction.Equals("down"))
            {
                i += 1;
            }
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("direction is " + direction);
        }
    }

    public List<string> CheckMoveOptions()
    {
        List<string> options = new List<string>();
        if(map[i+1,j] == false)
        {
            options.Add("down");
        }
        if (map[i - 1, j] == false)
        {
            options.Add("up");
        }
        if (map[i, j + 1] == false)
        {
            options.Add("right");
        }
        if (map[i, j - 1] == false)
        {
            options.Add("left");
        }
        return options;

    }
    
    public string Opposite(string theValue)
    {
        if (theValue.Equals("left"))
        {
            return "right";
        }
        if (theValue.Equals("right"))
        {
            return "left";
        }
        if (theValue.Equals("up"))
        {
            return "down";
        }
        if (theValue.Equals("down"))
        {
            return "up";
        }
        return null;
    }

    public void StartSolving()
    {
        List<string> previousMove = new List<string>();
        List<List<string>> decisions = new List<List<string>>();
        bool escaped = false;
        int moveNum = 0;
        bool goingBack = false;
        bool readDecisions = false;
        float startPosX = 49 * 50;
        float startPosY = 49 * 50; 
        previousMove.Add("null");
        List<string> firstDecision = new List<string>();
        firstDecision.Add("PlaceHolder");
        decisions.Add(firstDecision);
        moveNum++;
      
        StartCoroutine(Solver());
        IEnumerator Solver()
        {
            
            while(escaped == false) {
                
                Debug.Log("MoveNum: " + moveNum);

                if(i == 0 || i == 99 || j == 0 || j == 99)
                {
                    escaped = true;
                    break;
                }
                List<string> options = CheckMoveOptions();
                if(i*50 == startPosX && j*50 == startPosY)
                {
                    bool done = false;
                    foreach(string o in options)
                    {
                        bool temp = false;
                        foreach(string a in decisions[0])
                        {
                            if (a.Equals(o))
                            {
                                temp = true;
                            }
                        }
                        if(temp == false)
                        {
                            goingBack = false;
                            Move(o);
                            previousMove.Add(o);
                            moveNum++;
                            decisions[0].Add(o);
                            done = true;
                            break;
                        }
                    }
                    if(done == false)
                    {
                        Debug.LogError("Failed to find solution to maze");
                        Debug.LogError("Decisiouse are: ");
                        foreach(string t in decisions[0])
                        {
                            Debug.LogError(t);
                        }
                       
                    }
                }
                else {
                    if (goingBack == true)
                    {
                        Debug.LogError("GoBAck2");
                        if (options.Count < 3)
                        {

                            Move(Opposite(previousMove[moveNum - 1]));
                            previousMove.RemoveAt(moveNum - 1);
                            moveNum -= 1;


                        }
                        else
                        {
                            goingBack = false;
                            readDecisions = true;
                        }
                    }
                    else
                    {
                        if (options.Count > 1)
                        {
                            if (options.Count < 3)
                            {
                                if (options[0] != Opposite(previousMove[moveNum - 1]))
                                {
                                    Move(options[0]);
                                    previousMove.Add(options[0]);
                                    moveNum++;
                                }
                                else
                                {
                                    Move(options[1]);
                                    previousMove.Add(options[1]);
                                    moveNum++;
                                }
                            }
                            else
                            {
                                while (true)
                                {
                                    Debug.Log(readDecisions);
                                    if (readDecisions == false)
                                    {
                                        int randomNum = Random.Range(0, options.Count);
                                        if (options[randomNum] != Opposite(previousMove[moveNum - 1]))
                                        {
                                            Move(options[randomNum]);
                                            previousMove.Add(options[randomNum]);
                                            moveNum++;
                                            List<string> temp = new List<string>();
                                            //direction to go back from where you came frome before decision
                                            temp.Add(Opposite(previousMove[moveNum - 2]));
                                            int currentNumMoves = moveNum;
                                            temp.Add(currentNumMoves.ToString());
                                            temp.Add(options[randomNum]);
                                            Debug.Log("Added1 " + Opposite(previousMove[moveNum - 2]) + "  Added2 " + currentNumMoves.ToString() + " Added3 " + options[randomNum]);

                                            decisions.Add(temp);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("Reached");
                                        int randomNum = Random.Range(0, options.Count);
                                        bool possible = false;
                                        for (int i = 0; i < options.Count; i++)
                                        {
                                            bool temp = false;
                                            Debug.Log(decisions[decisions.Count - 1][0]);
                                            Debug.Log(decisions[decisions.Count - 1][1]);
                                            for (int j = 2; j < decisions[decisions.Count - 1].Count; j++)
                                            {
                                                Debug.Log(decisions[decisions.Count - 1][j]);

                                                if (options[i] == decisions[decisions.Count - 1][j])
                                                {
                                                    temp = true;
                                                }
                                            }

                                            if (options[i] == decisions[decisions.Count - 1][0])
                                            {
                                                temp = true;
                                            }
                                            if (temp == false)
                                            {
                                                possible = true;
                                            }

                                        }
                                        if (possible == true)
                                        {
                                            Debug.Log("Reached2");
                                            if (options[randomNum] != Opposite(previousMove[moveNum - 1]))
                                            {
                                                bool moveAble = true;
                                                for (int i = 2; i < decisions[decisions.Count - 1].Count; i++)
                                                {
                                                    if (options[randomNum] == decisions[decisions.Count - 1][i])
                                                    {
                                                        moveAble = false;
                                                    }
                                                }
                                                if (options[randomNum] == decisions[decisions.Count - 1][0])
                                                {
                                                    moveAble = false;
                                                }
                                                if (moveAble == true)
                                                {
                                                    Debug.Log("Reached3 " + options[randomNum]);
                                                    Move(options[randomNum]);
                                                    previousMove.Add(options[randomNum]);
                                                    moveNum++;
                                                    decisions[decisions.Count - 1].Add(options[randomNum]);
                                                    readDecisions = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log("Reached4");
                                            goingBack = true;
                                            //int removeNum = moveNum - decisions[decisions.Count - 1][1];
                                            decisions.RemoveAt(decisions.Count - 1);
                                            readDecisions = false;
                                            Move(Opposite(previousMove[moveNum - 1]));
                                            previousMove.RemoveAt(moveNum - 1);
                                            moveNum -= 1;
                                            break;
                                        }
                                    }
                                    walking.enabled = false;
                                    yield return new WaitForSeconds(2f);

                                }
                            }
                        }
                        else
                        {

                            goingBack = true;
                            Move(Opposite(previousMove[moveNum - 1]));
                            previousMove.RemoveAt(moveNum - 1);
                            moveNum -= 1;
                            Debug.LogError("GoBAck1");



                        }
                    }
                }
                walking.enabled = false;
                yield return new WaitForSeconds(2f);

            }
            
            Debug.Log("THIS IS RUUNIGN FOR SOME REASON");
            reset = true;
            StopCoroutine(Solver());
            
            
        }
    }
}

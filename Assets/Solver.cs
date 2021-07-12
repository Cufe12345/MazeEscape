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
    float solverTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
        
        first = true;
        camera = GameObject.Find("Main Camera");
        //Gets the maze generation script
        mazeGenerationScript = camera.GetComponent<MazeGeneration>();
        generationComplete = false;
        //sets the AI to the starting position
        transform.position = new Vector3(i * 50, 1, j * 50);
        //stores the walking animation into a variable and disables it
        walking = transform.GetComponent<Animator>();
        walking.enabled = false;
        i = (mazeGenerationScript.mapSize / 2) - 1;
        j = i;
    }

    // Update is called once per frame
    void Update()
    {
        //Is ran when the ai has solved the maze. Tells the maze generation script to generate a new maze
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
                    i = (mazeGenerationScript.mapSize / 2) - 1;
                    j = i;
                    transform.position = new Vector3(i * 50, 1, j * 50);
                }
            }
        }
        //Ran once the maze is generated in the MazeGeneration script
        if(mazeGenerationScript.complete == true)
        {
            generationComplete = true;
        }
        //Calls the StartSolving method and resets some variables
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
        //The position where the enemy needs to move to
        Vector3 tempPos;
        tempPos.x = i*50;
        tempPos.y = transform.position.y;
        tempPos.z = j*50;
        //Ran when the enemy needs to move
        if(transform.position.x != tempPos.x || transform.position.z != tempPos.z)
        {
            //makes the enemy face the direction its going to walk in
            float rotationSpeed = Time.deltaTime * 50;
            Vector3 direction = (tempPos - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
            //activates the walking animation
            walking.enabled = true;
        }
        else
        {
            //disables the walking animation
            walking.enabled = false;
        }
        
        //moves the enemy to the target location over time
        float speed = 250/solverTime;
        speed = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, tempPos, speed);
        

    }
    //A function used to move the enemys location in the array
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
    //Gets all the possible moves the enemy has and returns it
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
    //Gives the opposite direction of the string passed to it
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
    //The solving method where most of the enemy maze solving takes place in
    public void StartSolving()
    {
        /*stores a list of all the moves the enemy has made in order, with the most recent move the final item in the 
         * array*/ 
        List<string> previousMove = new List<string>();
        /*A list which basically stores every decision the enemy had to make and stores what direction the enemy came 
         * from before reaching the decision and what direction the enemy decided to take*/
        List<List<string>> decisions = new List<List<string>>();
        //if enemy has escaped from the maze
        bool escaped = false;
        //what move number the enemy is on also used to keep track of the previous move array
        int moveNum = 0;
        bool goingBack = false;
        bool readDecisions = false;
        float startPosX = i * 50;
        float startPosY = j * 50; 
        previousMove.Add("null");
        List<string> firstDecision = new List<string>();
        firstDecision.Add("PlaceHolder");
        decisions.Add(firstDecision);
        moveNum++;
        //starts the solving coroutine
        StartCoroutine(Solver());
        IEnumerator Solver()
        {
            
            while(escaped == false) {
                
                Debug.Log("MoveNum: " + moveNum);
                //if its at any of these coordinates it means the enemy has escaped so breaks loop
                if(i == 0 || i == mazeGenerationScript.mapSize-1 || j == 0 || j == mazeGenerationScript.mapSize-1)
                {
                    escaped = true;
                    break;
                }
                //stores all possible move options for the enemy by calling the CheckMoveOptions function
                List<string> options = CheckMoveOptions();
                //checks if enemy is at start position
                if(i*50 == startPosX && j*50 == startPosY)
                {
                    //checks if enemy has any move options
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
                            /*moves enemy and stores the direction the enemy went in, into the previous move and
                             * decisions array*/
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
                        /*Means the enemy has tried every path meaning there is no solution to the maze or something
                         * went wrong*/
                        Debug.LogError("Failed to find solution to maze");
                        Debug.LogError("Decisiouse are: ");
                        foreach(string t in decisions[0])
                        {
                            Debug.LogError(t);
                        }
                       
                    }
                }
                else {
                    //Runs if enemy reached deadend and has to go back
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
                        //runs if the enemy has more than one move option
                        if (options.Count > 1)
                        {
                            //runs if enemy has two move options
                            if (options.Count < 3)
                            {
                                //checks which move option isnt going back to the direction it came from
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
                                    //Checks if this is the enemy's first time at this decision or if its been here before
                                    if (readDecisions == false)
                                    {
                                        /*randomly decides on a direction to move in from its options and makes sure
                                         * that the move isnt going back to the direction it came from*/
                                        int randomNum = Random.Range(0, options.Count);
                                        if (options[randomNum] != Opposite(previousMove[moveNum - 1]))
                                        {
                                            //Moves the enemy and stores the direction in the previous move array
                                            Move(options[randomNum]);
                                            previousMove.Add(options[randomNum]);
                                            moveNum++;
                                            List<string> temp = new List<string>();
                                            /*stores the direction the enemy came from so that the enemy can go back from 
                                             * where the nemy came frome before the decision and stores the move the
                                             enemy decided to do*/
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
                                        //This is ran if enemy has been to this decision before
                                        Debug.Log("Reached");
                                        int randomNum = Random.Range(0, options.Count);
                                        bool possible = false;
                                        //Checks if there are an moves which the enemy hasnt made yet at this decision
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
                                        //if there is a possible move runs this
                                        if (possible == true)
                                        {
                                            Debug.Log("Reached2");
                                            /*checks if the choosen move isnt the direction the enemy came from
                                             * and makes sure that the enemy didnt decide to go that way before
                                             * at this decision*/

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
                                                    /*this runs if the move is valid and moves the enemy and stores
                                                     * its decision in the previous move and decisions array*/
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
                                            /*ran if no move options are left, it moves the enemy back to the direction
                                             * it came from when it reached the decision for the first time
                                             * it also removes the decision from the decision array.*/
                                            Debug.Log("Reached4");
                                            goingBack = true;
                                            
                                            decisions.RemoveAt(decisions.Count - 1);
                                            readDecisions = false;
                                            Move(Opposite(previousMove[moveNum - 1]));
                                            previousMove.RemoveAt(moveNum - 1);
                                            moveNum -= 1;
                                            break;
                                        }
                                    }
                                    //disables the walking animation and waits for the specified time below
                                    walking.enabled = false;
                                    yield return new WaitForSeconds(solverTime);

                                }
                            }
                        }
                        else
                        {
                            //makes the enemy move back as it has reached a deadend.
                            goingBack = true;
                            Move(Opposite(previousMove[moveNum - 1]));
                            previousMove.RemoveAt(moveNum - 1);
                            moveNum -= 1;
                            Debug.LogError("GoBAck1");



                        }
                    }
                }
                //disables the walking animation and waits for the specified time below
                walking.enabled = false;
                yield return new WaitForSeconds(solverTime);

            }
            
            //tells the script that the enemy has solved the maze and to reset the maze.
            reset = true;
            StopCoroutine(Solver());
            
            
        }
    }
}

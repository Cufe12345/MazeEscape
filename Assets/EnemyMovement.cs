using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public int[,] map;
    int i;
    int j;
    int mapSize;
    GameObject camera;
    MazeGeneration mazeScript;
    int movesAway;
    float moveDelay;
    Animator walking;
    public void Start()
    {
        moveDelay = 3f;
        movesAway = 0;
        i = Convert.ToInt32(transform.position.x/50);
        j = Convert.ToInt32(transform.position.z / 50);
        camera = GameObject.Find("Main Camera");
        mazeScript = camera.transform.GetComponent<MazeGeneration>();
        map = mazeScript.map;
        mapSize = mazeScript.mapSize;
        walking = transform.GetComponent<Animator>();
        StartMovement();

    }
    public void Update()
    {
        //The position where the enemy needs to move to
        Vector3 tempPos;
        tempPos.x = i * 50;
        tempPos.y = transform.position.y;
        tempPos.z = j * 50;
        //Ran when the enemy needs to move
        if (transform.position.x != tempPos.x || transform.position.z != tempPos.z)
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
        float speed = 50;
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
    //Checks what moves the enemy can currently make.
    public List<string> CheckMoveOptions()
    {
        List<string> options = new List<string>();
        if (i < mapSize - 1)
        {
            if (map[i + 1, j] == 1 || map[i + 1, j] == 3)
            {
                options.Add("down");
            }
        }
        if (i > 0)
        {
            if (map[i - 1, j] == 1 || map[i - 1, j] == 3)
            {
                options.Add("up");
            }
        }
        if (j < mapSize - 1)
        {
            if (map[i, j + 1] == 1 || map[i, j + 1] == 3)
            {
                options.Add("right");
            }
        }
        if (j > 0)
        {
            if (map[i, j - 1] == 1 || map[i, j - 1] == 3)
            {
                options.Add("left");
            }
        }
        return options;

    }
    //returns the opposite of whats passed into it
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
    //does the first move of the enemy.
    public string FirstMove()
    {
        List<string> firstMoves = new List<string>();
        firstMoves = CheckMoveOptions();
        System.Random rnd = new System.Random();
        int index = rnd.Next(0, firstMoves.Count - 1);
        Move(firstMoves[index]);
        movesAway++;
        return firstMoves[index];
        
    }
    public void StartMovement()
    {
        List<string> previousMoves = new List<string>();
        previousMoves.Add(FirstMove());
        StartCoroutine(theMovement());
        IEnumerator theMovement()
        {
            while (true)
            {
                //moves the enemy until they are 5 moves away then brings them back to where they started
                if (movesAway < 5)
                {
                    List<string> moves = new List<string>();
                    List<string> finalMoves = new List<string>();
                    moves = CheckMoveOptions();
                    foreach (string move in moves)
                    {
                        if (move != Opposite(previousMoves[previousMoves.Count - 1]))
                        {
                            finalMoves.Add(move);
                        }
                    }
                    if (finalMoves.Count > 0)
                    {
                        System.Random rnd = new System.Random();
                        Debug.LogError(finalMoves.Count - 1);
                        int index = rnd.Next(0, finalMoves.Count - 1);
                        Move(finalMoves[index]);
                        previousMoves.Add(finalMoves[index]);
                        movesAway++;
                        yield return new WaitForSeconds(moveDelay);
                    }
                    else
                    {
                        movesAway = 5;
                    }


                }
                else
                {
                    movesAway = 0;
                    while (previousMoves.Count >0)
                    {
                        Move(Opposite(previousMoves[previousMoves.Count - 1]));
                        previousMoves.RemoveAt(previousMoves.Count - 1);
                        yield return new WaitForSeconds(moveDelay);

                    }
                    previousMoves.Add(FirstMove());
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour
{
    public int difficulty = 0;
    public int mapSize = 0;
    int maxPaths = 0;
    public int[,] map;
    int centre = 0;
    GameObject wall;
    List<Path> paths = new List<Path>();
    bool first = true;
    int finalI;
    int finalI2;
    public bool complete = false;
    public bool resetMaze = false;
    GameObject floor;
    GameObject enemy;
    GameObject mapObject;
    GameObject enemiesObject;
    GameObject finishFlag;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Initialise's the variables and sets the whole map to true to fill it up.
        //hard coded difficulty later on once main menu created it doesnt have to be hard coded
        //map size and max paths initialised based on difficulty value
        difficulty = 3;
        complete = false;
        enemy = GameObject.Find("Enemy");
        mapSize = difficulty * 30;
        maxPaths = difficulty * 20;
        centre = (mapSize / 2) - 1;
        map = new int[mapSize, mapSize];
        wall = GameObject.Find("Wall");
        floor = GameObject.Find("Floor");
        mapObject = GameObject.Find("Map");
        enemiesObject = GameObject.Find("Enemies");
        finishFlag = GameObject.Find("FinishFlag");
        for (int i = 0; i < mapSize; i++)
        {
            for (int i2 = 0; i2 < mapSize; i2++)
            {
                map[i, i2] = 0;
            }
        }
        //Sets the centre to 1 so it isnt filled in
        map[centre, centre] = 1;
        int random = UnityEngine.Random.Range(1, 5);
        for (int i = 0; i < random; i++)
        {
            Generate(centre, centre);
        }
    }
    public void Reset()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int i2 = 0; i2 < mapSize; i2++)
            {
                map[i, i2] = 0;
            }
        }
        //Sets the centre to 1 so it isnt filled in
        map[centre, centre] = 1;
        int random = UnityEngine.Random.Range(1, 5);
        for (int i = 0; i < random; i++)
        {
            Debug.Log("RANNANAKSA");
            Generate(centre, centre);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (first == true)
        {
            //Checks if all the paths are finished and if they are runs the CheckExit function and stops this code repeating
            bool fin = true;
            foreach (Path p in paths)
            {
                if (p.done == true)
                {

                }
                else
                {
                    fin = false;
        
                }
            }
            if (fin == true)
            {
                fin = false;
                first = false;
                CheckExit();
            }
        }
        if (resetMaze == true)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach(GameObject g in walls)
            {
                Destroy(g);
            }
            GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
            foreach(GameObject g2 in floors)
            {
                Destroy(g2);
            }
            paths = new List<Path>();
            first = true;
            complete = false;
            resetMaze = false;
            Debug.Log("Ruijrddjfnjfndkfjd");
            Reset();
        }



    }
    public void CheckExit()
    {
        //Checks if there is an exit to the maze and then randomly chooses a side to have the only exit
        int preI = finalI;
        int preI2 = finalI2;
        if (finalI == mapSize - 1 || finalI == 0 || finalI2 == mapSize - 1 || finalI2 == 0)
        {
            Debug.Log("Map DONE");
            buildMap();
        }
        else
        {
            // randomly selects which side will have the exit
            int r = UnityEngine.Random.Range(1, 5);

            if (r == 2)
            {
                for (int a = mapSize-1; a > -1; a--)
                {
                    //finds the exit on this side and stores its coordinates
                    if (map[a, mapSize - 1] == 1 || map[a,mapSize -1] == 3)
                    {
                        finalI = a;
                        finalI2 = mapSize - 1;
                        break;
                    }
                }
            }
            else if (r == 1)
            {
                for (int a = 0; a < mapSize; a++)
                {
                    if (map[a, 0] == 1 || map[a,0] == 3)
                    {
                        finalI = a;
                        finalI2 = 0;
                        break;
                    }
                }
            }
            else if (r == 3)
            {
                for (int a = mapSize-1; a > -1; a--)
                {
                    if (map[0, a] == 1 || map[0,a] == 3)
                    {
                        finalI = 0;
                        finalI2 = a;
                        break;
                    }
                }
            }
            else if (r == 4)
            {
                for (int a = 0; a < mapSize; a++)
                {
                    if (map[mapSize - 1, a] == 1 || map[mapSize-1,a]== 3)
                    {
                        finalI = mapSize - 1;
                        finalI2 = a;
                        break;
                    }
                }
            }
            //if couldnt find a exit on the selected side it trys again
            if (preI == finalI && preI2 == finalI2)
            {
                CheckExit();
            }
            else
            {
                //if exit found it runs buildMap function
                buildMap();
            }


        }

    }

    public void buildMap()
    {
        //fills in sides of the grid
        for (int i = 0; i < mapSize; i++)
        {
            map[i, 0] = 0;
            map[i, mapSize - 1] = 0;
        }
        for (int i2 = 0; i2 < mapSize; i2++)
        {
            map[0, i2] = 0;
            map[mapSize - 1, i2] = 0;
        }
        Debug.LogWarning(finalI + " " + finalI2);
        //hollows out the exit selected in the checkExit function
        map[finalI, finalI2] = 1;
        finishFlag.transform.position = new Vector3(finalI * 50, 0, finalI2 * 50);
        int enemyCount = 1;
        //iterates over the array spawning in walls in the correct places
        for (int i = 0; i < mapSize; i++)
        {
            for (int i2 = 0; i2 < mapSize; i2++)
            {
                if (map[i, i2] == 0)
                {
                    GameObject temp = Instantiate(wall);
                    temp.transform.position = new Vector3(i * 50, 0, i2 * 50);
                    temp.transform.tag = "Wall";
                    temp.transform.parent = mapObject.transform;

                }
                else if(map[i, i2] == 1)
                {
                    GameObject temp2 = Instantiate(floor);
                    temp2.transform.position = new Vector3(i * 50, 0, i2 * 50);
                    temp2.transform.tag = "Floor";
                    temp2.transform.parent = mapObject.transform;
                }
                else
                {
                    GameObject temp2 = Instantiate(floor);
                    temp2.transform.position = new Vector3(i * 50, 0, i2 * 50);
                    temp2.transform.tag = "Floor";
                    temp2.transform.parent = mapObject.transform;
                    GameObject temp3 = Instantiate(enemy);
                    temp3.transform.position = new Vector3(i*50,10,i2 * 50);
                    temp3.transform.parent = enemiesObject.transform;
                    temp3.GetComponent<EnemyAttack>().enabled = true;
                    temp3.name = "Enemy" + enemyCount;
                    temp3.GetComponent<EnemyMovement>().enabled = true;
                    enemyCount++;

                }
            }
        }
        complete = true;
    }
    public void Generate(int i, int i2)
    {
        //Creates a new path object
        if (paths.Count < maxPaths)
        {

            Path p = new Path(i, i2);
            paths.Add(p);
            if (paths.Count == 1)
            {

                p.original = true;
            }
        }

    }
    public void Run(IEnumerator a)
    {
        StartCoroutine(a);
    }
    public class Path
    {
        static GameObject main2 = GameObject.Find("Main Camera");
        static MazeGeneration script = main2.GetComponent<MazeGeneration>();
        public int[,] map = script.map;
        public bool done = false;
        public bool original = false;
        int finalI;
        int finalI2;
        public Path(int a, int b)
        {
            script.Run(Generate2(a, b));
        }


        IEnumerator Generate2(int a, int b)
        {


            int i = a;
            int i2 = b;
            //selects starting direction of the path object
            int firstRandom = UnityEngine.Random.Range(0, 4);
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;
            int preI = 0;
            int preI2 = 0;

            while (i > 0 && i2 > 0 && i < script.mapSize - 1 && i2 < script.mapSize-1)
            {
                preI = i;
                preI2 = i2;
                //random decides the direction of the path and random2 decides whever or not a new path is to be created
                int random = UnityEngine.Random.Range(0, 4);
                int random2 = UnityEngine.Random.Range(0, 50);
                int random3 = UnityEngine.Random.Range(0, 10);
                if (random == 0)
                {
                    /*stops the path going in the opposite direction it started going in so for example
                     * if it started going up it cant go down etc*/
                    if (firstRandom != 1)
                    {
                        try
                        {
                            /*runs checks to make sure that if it was to hollow out the direction it wants to go
                             * it wouldnt be touching another hollowed out square*/

                            if (map[i - 2, i2] == 1 || map[i - 2, i2] == 3)
                            {

                                up = true;
                            }
                            else if (map[i - 1, i2 - 1] == 1 || map[i - 1, i2 - 1] == 3 )
                            {
                                up = true;
                            }
                            else if (map[i - 1, i2 + 1] == 1 || map[i - 1, i2 + 1] == 3 )
                            {
                                up = true;
                            }
                            else
                            {
                                down = false;
                                up = false;
                                left = false;
                                right = false;
                                i -= 1;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {

                            i -= 1;
                        }
                    }
                }
                else if (random == 1)
                {
                    if (firstRandom != 0)
                    {
                        try
                        {
                            if (map[i + 2, i2] == 1 || map[i + 2, i2] == 3)
                            {
                                down = true;
                            }
                            else if (map[i + 1, i2 - 1] == 1 || map[i + 1, i2 - 1] == 3 )
                            {
                                down = true;
                            }
                            else if (map[i + 1, i2 + 1] == 1 || map[i + 1, i2 + 1] == 3 )
                            {
                                down = true;
                            }
                            else
                            {
                                down = false;
                                up = false;
                                left = false;
                                right = false;
                                i += 1;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {

                            i += 1;
                        }

                    }
                }
                else if (random == 2)
                {
                    if (firstRandom != 3)
                    {
                        try
                        {
                            if (map[i, i2 - 2] == 1 || map[i,i2 -2] == 3)
                            {
                                left = true;
                            }
                            else if (map[i - 1, i2 - 1] == 1 || map[i - 1, i2 - 1] == 3 )
                            {
                                left = true;
                            }
                            else if (map[i + 1, i2 - 1] == 1 || map[i + 1, i2 - 1] == 3 )
                            {
                                left = true;
                            }
                            else
                            {
                                down = false;
                                up = false;
                                left = false;
                                right = false;
                                i2 -= 1;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {

                            i2 -= 1;
                        }

                    }

                }
                else
                {
                    if (firstRandom != 2)
                    {
                        try
                        {
                            if (map[i, i2 + 2] == 1 || map[i, i2 + 2] == 3 )
                            {
                                right = true;
                            }
                            else if (map[i - 1, i2 + 1] == 1 || map[i - 1, i2 + 1] == 3 )
                            {
                                right = true;
                            }
                            else if (map[i + 1, i2 + 1] == 1 || map[i + 1, i2 + 1] == 3 )
                            {
                                right = true;
                            }
                            else
                            {
                                down = false;
                                up = false;
                                left = false;
                                right = false;
                                i2 += 1;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {

                            i2 += 1;
                        }

                    }
                }
                //Checks if the path has tried to go up, down,left and right without being able to meaning its trapped so it breaks loop
                if ((up == true && down == true && left == true) || (up == true && down == true && right == true) || (up == true && left == true && right == true) || (down == true && left == true && right == true))
                {
                    break;
                }
                if (random2 == 2)
                {
                    //Create new path
               
                    script.Generate(i, i2);

                }
                
                //Reached end of maze
                if (i >= script.mapSize || i <= -1 || i2 >= script.mapSize || i2 <= -1)
                {

                    break;
                }
                else
                {

                    map[i, i2] = 1;
                    if (random3 == 2)
                    {
                        map[i, i2] = 3;
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
            if (original == true)
            {
                finalI = preI;
                finalI2 = preI2;
                script.finalI = finalI;
                script.finalI2 = finalI2;
            }

            done = true;


        }
    }
}

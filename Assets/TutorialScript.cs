using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    GameObject enemies;
    GameObject tutorialText;
    GameObject player;
    GameObject gun;
    GameObject solver;
    GameObject finishFlag;
    GameObject camera;
    bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        tutorialText = GameObject.Find("TutorialText");
        enemies = GameObject.Find("Enemies");
        player = GameObject.Find("Player");
        solver = GameObject.Find("EnemySolver");
        finishFlag = GameObject.Find("FinishFlag");
        //problem two with sasme name.!
        gun = GameObject.FindGameObjectWithTag("PlayerGun");
        camera = GameObject.Find("Main Camera");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(first == true)
        {
            first = false;
            StartCoroutine(movement());
        }
    }
    //this function sets the tutorial text to whatever string is passed to it.
    public void setText(string text)
    {
        tutorialText.GetComponent<Text>().text = " ";
        StartCoroutine(scrollText(text));
        
    }
    IEnumerator scrollText(string text)
    {
        string tempText = " ";
        foreach(char c in text)
        {
            tempText += c.ToString();
            tutorialText.GetComponent<Text>().text = tempText;
            if (text.Length > 50)
            {
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    //function for the movement and looking tutorial
    IEnumerator movement()
    {
        gun.GetComponent<ADS>().enabled = false;
        gun.GetComponent<GenerateBullet>().enabled = false;
        gun.GetComponent<Reload>().enabled = false;
        solver.GetComponent<Solver>().tutorial = true;
        finishFlag.GetComponent<FinishGame>().enabled = false;
        solver.SetActive(false);
        enemies.SetActive(false);
        setText("Use the WASD keys to move and the mouse to look around!");
        bool w = false;
        bool a = false;
        bool s = false;
        bool d = false;
        bool looked = false;
        Vector3 previous = transform.rotation.eulerAngles;
        //runs till WASD keys pressed and mouse moved
        while (true)
        {
            Vector3 current = transform.rotation.eulerAngles;
            if (Input.GetKey(KeyCode.W))
            {
                w = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                a = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                s = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                d = true;
            }
            if(current != previous)
            {
                looked = true;
            }
            if(w == true && a == true && s == true && d == true && looked == true)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
            
            
        }
        yield return new WaitForSeconds(2f);
        player.GetComponent<Movement>().enabled = false;
        StartCoroutine(Aim());
    }
    //This function runs the aiming of the gun tutorial
    IEnumerator Aim()
    {
        setText("Use the right mouse button to aim down the gun!");
        gun.GetComponent<ADS>().enabled = true;
        //runs till the right mouse button is pressed
        while (true)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(Fire());

    }
    //this function runs the gun firing tutorial
    IEnumerator Fire()
    {
        setText("Use the left mouse button to fire the gun");
        gun.GetComponent<GenerateBullet>().enabled = true;
        //runs till left mouse button is pressed
        while (true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(Reload());
    }
    //this function runs the gun reloading tutorial
    IEnumerator Reload()
    {
        setText("Press the R Key to reload the weapon");
        gun.GetComponent <GenerateBullet>().enabled = false;
        gun.GetComponent<ADS>().enabled = false;
        gun.GetComponent<Reload>().enabled = true;
        gun.GetComponent<Reload>().bulletCount = 0;
        //runs till gun magazine is full
        while (true)
        {
            if(gun.GetComponent<Reload>().bulletCount == 6)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(Enemies());

    }
    //this function runs the enemies tutorial
    IEnumerator Enemies()
    {
        enemies.SetActive(true);
        setText("Find an enemy in the maze and kill them");
        //turns of firing and movement for all enemies in the maze
        for(int i = 0; i < enemies.transform.childCount; i++)
        {
            enemies.transform.GetChild(i).gameObject.GetComponent<EnemyAttack>().enabled = false;
            enemies.transform.GetChild(i).gameObject.GetComponent<EnemyMovement>().enabled = false;
        }
        player.GetComponent<Movement>().enabled = true;
        gun.GetComponent<GenerateBullet>().enabled = true;
        gun.GetComponent<ADS>().enabled = true;
        int enemyCount = enemies.transform.childCount;
        //runs till an enemy is killed by comparing the current enemy count to the initial one
        while (true)
        {
            if(enemyCount != enemies.transform.childCount)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(Death());
    }
    //this function runs the death tutorial
    IEnumerator Death()
    {
        //sets the players health to zero so that they die
        setText("You have died, when you die you get teleported back to the centre of the maze!");
        player.GetComponent<Health>().health = 0;
        yield return new WaitForSeconds(7);
        StartCoroutine(Regenerate());

    }
    //this function runs the regenerate tutorial
    IEnumerator Regenerate()
    {
        setText("You have lost health, your health regenerates over time when you dont take damage for 10 seconds");
        player.GetComponent<Health>().health = 50;
        enemies.SetActive(false);
        //runs till health is back at max
        while (true)
        {
            if(player.GetComponent<Health>().health == 100)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(AimOfGame());

    }
    //this function runs the aim of the game tutorial
    IEnumerator AimOfGame()
    {
        setText("The aim of the game is to escape out the maze before the enemy does." +
            " Watch the enemy you are competing against solve the maze below");
        solver.SetActive(true);
        //turns of gravity for the player and sets the cameras rotation to forward.
        player.GetComponent<Rigidbody>().useGravity = false;
        camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        player.GetComponent<Movement>().enabled = false;
        //runs till the ai solves the maze and constantly makes the player follow the ai above
        while (true)
        {
            player.transform.position = new Vector3(solver.transform.position.x, solver.transform.position.y + 500, solver.transform.position.z);
            player.transform.rotation = Quaternion.Euler(90, 0, 0);
            if(solver.GetComponent<Solver>().complete == true)
            {
                finishFlag.GetComponent<FinishGame>().gameOverPanel.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(End());
    }
    //this function tells the user that they have completed the tutorial
    IEnumerator End()
    {
        setText("Well done you have completed the tutorial!! Press the space key to exit");
        while (true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("MainMenuScene");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}

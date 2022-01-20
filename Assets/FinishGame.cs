using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    GameObject enemySolver;
    GameObject player;
    GameObject enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemySolver = GameObject.Find("EnemySolver");
        enemies = GameObject.Find("Enemies");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Ends the game
    public void EndGame()
    {
        enemies.SetActive(false);
        enemySolver.SetActive(false);
        player.GetComponent<Movement>().enabled = false;
    }
    public void OnCollisionEnter(Collision collision)
    {
        //detects collision with player
        if(collision.gameObject.name == "Player")
        {
            EndGame();
        }
    }
}

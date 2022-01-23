using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour
{
    GameObject enemySolver;
    GameObject player;
    GameObject enemies;
    GameObject gameOverPanel;
    GameObject killText;
    GameObject deathText;
    GameObject timeElapsedText;
    GameObject outcomeText;
    int timeElapsed;
    bool end;
    // Start is called before the first frame update
    void Start()
    {
        end = false;
        timeElapsed = 0;
        enemySolver = GameObject.Find("EnemySolver");
        enemies = GameObject.Find("Enemies");
        player = GameObject.Find("Player");
        gameOverPanel = GameObject.Find("GameOverPanel");
        killText = GameObject.Find("KillText");
        deathText = GameObject.Find("DeathText");
        timeElapsedText = GameObject.Find("TimeElapsedText");
        outcomeText = GameObject.Find("OutComeText");
        gameOverPanel.SetActive(false);
        StartCoroutine(time());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Ends the game
    public void EndGame(bool win)
    {
        end = true;
        enemies.SetActive(false);
        enemySolver.SetActive(false);
        player.GetComponent<Movement>().enabled = false;
        gameOverPanel.SetActive(true);
        killText.SetActive(false);
        deathText.SetActive(false);
        timeElapsedText.SetActive(false);
        if(win != true)
        {
            outcomeText.GetComponent<Text>().text = "YOU LOST";
            outcomeText.GetComponent<Text>().color = Color.red;
        }
        StartCoroutine(DisplayText());
        
        

    }
    IEnumerator DisplayText()
    {
        string kText = "Kills: " + player.GetComponent<Health>().kills;
        string dText = "Deaths: " + player.GetComponent<Health>().deaths;
        string eText = "Time Elapsed: " + timeElapsed;
        char[] newtext = new char[kText.Length];
        int index = 0;
        killText.SetActive(true);       
        foreach (char c in kText)
        {
            newtext[index] = c;
            index++;
            killText.GetComponent<Text>().text = string.Concat(newtext);
            yield return new WaitForSeconds(0.15f);
        }
        newtext = new char[dText.Length];
        index = 0;
        deathText.SetActive(true);
        foreach (char c in dText)
        {
            newtext[index] = c;
            index++;
            deathText.GetComponent<Text>().text = string.Concat(newtext);
            yield return new WaitForSeconds(0.15f);
        }
        newtext = new char[eText.Length];
        index = 0;
        timeElapsedText.SetActive(true);
        foreach (char c in eText)
        {
            newtext[index] = c;
            index++;
            timeElapsedText.GetComponent<Text>().text = string.Concat(newtext);
            yield return new WaitForSeconds(0.15f);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //detects collision with player
        if(collision.gameObject.name == "Player")
        {
            EndGame(true);
        }
    }
    IEnumerator time()
    {
        while (true)
        {
            timeElapsed += 1;
            if (end == true)
            {
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }
}

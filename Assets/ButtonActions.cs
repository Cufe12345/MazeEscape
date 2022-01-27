using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    GameObject playSelect;
    GameObject tutorialSelect;
    GameObject exitSelect;
    GameObject playButton;
    GameObject exitButton;
    GameObject tutorialButton;
    
    // Start is called before the first frame update
    void Start()
    {
        playSelect = GameObject.Find("PlaySelect");
        tutorialSelect = GameObject.Find("TutorialSelect");
        exitSelect = GameObject.Find("ExitSelect");
        playSelect.SetActive(false);
        tutorialSelect.SetActive(false);
        exitSelect.SetActive(false);
        playButton = GameObject.Find("PlayButton");
        tutorialButton = GameObject.Find("TutorialButton");
        exitButton = GameObject.Find("ExitButton");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogError("Scene count: " + SceneManager.sceneCount);
    }
    public void Highlighted(int n)
    {
        if(n == 0)
        {
            playSelect.SetActive(true);
        }
        else if(n == 1)
        {
            tutorialSelect.SetActive(true);
        }
        else
        {
            exitSelect.SetActive(true);
        }
    }
    public void UnHighlighted(int n)
    {
        if (n == 0)
        {
            playSelect.SetActive(false);
        }
        else if (n == 1)
        {
            tutorialSelect.SetActive(false);
        }
        else
        {
            exitSelect.SetActive(false);
        }
    }
    //runs when play button pressed
    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    //runs when exit button pressed
    public void Exit()
    {
        Application.Quit();
    }
}

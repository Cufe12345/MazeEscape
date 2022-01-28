using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    GameObject player;
    GameObject loadingPanel;
    GameObject camera;
    GameObject loadingText;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        loadingPanel = GameObject.Find("LoadingPanel");
        camera = GameObject.Find("Main Camera");
        loadingText = GameObject.Find("LoadingText");
        StartCoroutine("Text");
    }

    // Update is called once per frame
    void Update()
    {
        //turns loading panel off when maze is generated
        if(camera.GetComponent<MazeGeneration>().complete == true)
        {
            loadingPanel.SetActive(false);
        }
    }
    IEnumerator Text()
    {
        while (true)
        {
            loadingText.GetComponent<Text>().text = "Generating Maze.";
            yield return new WaitForSeconds(0.4f);
            loadingText.GetComponent<Text>().text = "Generating Maze..";
            yield return new WaitForSeconds(0.4f);
            loadingText.GetComponent<Text>().text = "Generating Maze...";
            yield return new WaitForSeconds(0.4f);
            loadingText.GetComponent<Text>().text = "Generating Maze";
            yield return new WaitForSeconds(0.4f);
        }
    }
}

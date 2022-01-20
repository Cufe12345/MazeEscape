using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    bool player;
    public float health;
    float regenTime = 10;
    float initialHealth;
    int i;
    int j;
    GameObject camera;
    MazeGeneration mazeGenerationScript;
    GameObject healthPanel;
    // Start is called before the first frame update
    void Start()
    {
        //sets initial health
        health = 100;
        if (transform.name.Contains("Player"))
        {
            //gets the health bar object
            player = true;
            healthPanel = GameObject.Find("PanelHealth");
        }
        else
        {
            player = false;
        }
        camera = GameObject.Find("Main Camera");
        //Gets the maze generation script
        mazeGenerationScript = camera.GetComponent<MazeGeneration>();
        //stores the centre of the maze
        i = (mazeGenerationScript.mapSize / 2) - 1;
        j = i;
        //starts the health regeneration method
        StartCoroutine(healthRegeneration());
    }

    // Update is called once per frame
    void Update()
    {
        //checks if health goes below or equal to zero
        if (health <= 0)
        {
            if(player == true)
            {
                //teleports player back to centre of the maze
                transform.position = new Vector3(i * 50, 50, j * 50);
                health = 100;
            }
            else
            {
                //destorys the enemy
                Destroy(this.gameObject);
            }
        }
        if(player == true)
        {
            //this code is used to scale the health bar depending on how much health the user has
            RectTransform rt = healthPanel.transform.GetComponent<RectTransform>();
            //scale of the health bar
            float scale = (health / 100) * 0.29f;
            Vector3 temp = rt.localScale;
            temp.x = scale;
            rt.localScale = temp;
            //formula calculates how far right the health bar should be depending on the value of the scale
            float rightVal = (-2962.9629629629626f * (scale * scale)) + (-1911.1111111111122f * scale) + 819.4074074074075f;
            rt.offsetMax = new Vector2(-rightVal, rt.offsetMax.y);
        }
    }
    IEnumerator healthRegeneration()
    {
        while (true)
        {
            initialHealth = health;
            yield return new WaitForSeconds(regenTime);
            //regenerates health till the object is damaged again or they are at full health
            while(health == initialHealth && health <= 99)
            {
                health += 1;
                initialHealth = health;
                yield return new WaitForSeconds(2/10f);
            }
        }
    }
}

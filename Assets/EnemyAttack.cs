using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    GameObject player;
    GameObject eye;
    // Start is called before the first frame update
    void Start()
    {
        //Initialises the player variable so it can be referenced later on
        player = GameObject.Find("Player");
        //finds the eye gameobject so the ray can start from the eye
        for(int i = 0; i< transform.childCount; i++)
        {
            if(transform.GetChild(i).transform.name == "EyeRight")
            {
                eye = transform.GetChild(i).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dist = 100f;
        LayerMask layerMask = LayerMask.GetMask("Targets");
        RaycastHit ray;
        //Creates ray and detets what its collided with
        if (Physics.Raycast(eye.transform.position, (player.transform.position-transform.position) * dist, out ray, Mathf.Infinity, layerMask))
        {
            Vector3 pos = ray.point;
            Vector3 targetDirection = pos - transform.position;
            float step = 1000000 * Time.deltaTime;
            Debug.DrawLine(transform.position, pos);
            //if ray collides with player it makes the enemy face towards the player
            if (ray.collider.gameObject == player)
            {

                
                Vector3 temp = transform.forward;

                Vector3 newR = Vector3.RotateTowards(temp, targetDirection, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newR);
                float dist2 = Vector3.Distance(pos, transform.position);
                //if the distance is less than 100 it fires the weapon
                if(dist2 < 100)
                {
                
                    StartCoroutine(Fire());
                }
            }
        }
    }
    IEnumerator Fire()
    {
        int number = 0;
        //For every gun in the scene it checks if the parents of it are equal to the enemy itself
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Gun"))
        {
            number++;
            GameObject parent = g.transform.parent.gameObject;
            while(parent != null)
            {
            //when it finds the gun whos parent is this enemies it fires the gun
                if(parent.name == transform.gameObject.name)
                {
             
                    g.GetComponent<GenerateBullet>().FireWeapon();
                    break;
                }
                else
                {
                    try
                    {
                        parent = parent.transform.parent.gameObject;
                    }catch(NullReferenceException e)
                    {
                        parent = null;
                        break;
                    }
                }
                
            }
            


        }
        yield return new WaitForSeconds(0.01f);
        Debug.LogWarning("Number is: " + number);
    }
}

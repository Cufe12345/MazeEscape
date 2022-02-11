using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fired : MonoBehaviour
{
    Rigidbody rigi;
    GameObject player;
    public GameObject origin;
    Movement move;
    public float speed;
    public GameObject gun;
    public GameObject bulletSpawner;
    public bool start = false;
    public float damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        //sets bullet speed
        speed = 2000;
        player = GameObject.Find("Player");
        move = player.transform.GetComponent<Movement>();
        rigi = transform.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            start = false;
            //sets the layer mask to target meaning only objects with the layer mask of target can collide with ray
            LayerMask layerMask = LayerMask.GetMask("Targets");
            float dist = 100f;
            RaycastHit ray;
          //Creates ray and detets what its collided with
            if (Physics.Raycast(bulletSpawner.transform.position, bulletSpawner.transform.forward * dist, out ray, Mathf.Infinity, layerMask))
            {
                //gets postion where ray collided
                Vector3 pos = ray.point;
                Vector3 targetDirection = pos - transform.position;
                float step = 1000000 * Time.deltaTime;

                Debug.DrawLine(bulletSpawner.transform.position, pos);
                //makes bullet face direction it needs to travel in
                Vector3 temp = transform.forward;

                Vector3 newR = Vector3.RotateTowards(temp, targetDirection, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newR);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                //adds force to make bullet fire
                transform.GetComponentInChildren<MeshRenderer>().enabled = true;
                rigi.AddForce(transform.forward*speed*Time.deltaTime);


            }
        }
        //destroys bullet if it goes below the map as minimum y value is 0
        if(transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
        //destroys bullet when it is stationary
        if(rigi.velocity.x == 0 && rigi.velocity.y == 0 && rigi.velocity.z == 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLIDED WITH " + collision.gameObject.name);
        //destroys bullet if it has collided with anything other than the gun or bullet
        if (collision.gameObject != gun && !collision.gameObject.name.Contains("Shotgun Shell"))
        {
            //if the gameobject that created the bullet is an enemy it makes sure it isnt damaging an enemy
            if (origin.transform.name.Contains("Enemies"))
            {


                if (!collision.gameObject.transform.root.gameObject.transform.name.Contains("Enemies"))
                {
                    Damage(collision);
                }


            }
            //if the gameobject that created the bullet is an player it makes sure it isnt damaging the player
            else
            {
                if (!collision.gameObject.transform.root.gameObject.transform.name.Contains("Player"))
                {
                    Damage(collision);
                }
            }
            Destroy(this.gameObject);
        }
    }
    //Deals the damage
    public void Damage(Collision collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            collision.gameObject.GetComponent<Health>().health -= damage;
        }
        else if (collision.gameObject.name.Contains("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().health -= damage * 4;
        }
    }
}

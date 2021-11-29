using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fired : MonoBehaviour
{
    Rigidbody rigi;
    GameObject player;
    Movement move;
    public float speed = 50000;
    public GameObject gun;
    public GameObject bulletSpawner;
    public bool start = false;

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
     
       //destroys bullet if it has collided with anything other than the gun
        if (collision.gameObject != gun)
        {

            Destroy(this.gameObject);
        }
    }
}

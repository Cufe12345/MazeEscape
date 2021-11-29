using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBullet : MonoBehaviour
{
    GameObject bullet;
    GameObject bulletSpawner;
    GameObject muzzleFlash;
    float timer = 0.1f;
    bool fired = false;
    // Start is called before the first frame update
    void Start()
    {
        //Finds the bullet spawner child object
        bullet = GameObject.Find("Shotgun Shell");
        for(int i =0; i<transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name.Equals("BulletSpawner"))
            {
                bulletSpawner = transform.GetChild(i).gameObject;
                
            }
            else if (transform.GetChild(i).gameObject.name.Equals("MuzzleFlash"))
            {
                muzzleFlash = transform.GetChild(i).gameObject;
                muzzleFlash.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //runs when left mouse button clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Enables muzzle Flash
            muzzleFlash.GetComponent<ParticleSystem>().Play();
            //duplicates bullet
            GameObject temp = Instantiate(bullet);
            temp.GetComponentInChildren<MeshRenderer>().enabled = false;
            //sets new bullets position and rotation
            temp.transform.position = bulletSpawner.transform.position;
            float y = bulletSpawner.transform.eulerAngles.y - 10;
            float x = bulletSpawner.transform.eulerAngles.x-90;
            temp.transform.rotation = Quaternion.Euler(x,y,bulletSpawner.transform.rotation.z);
            //enables fired script and passes relevant variables needed by the fired script
            temp.GetComponent<Fired>().gun = this.gameObject;
            temp.GetComponent<Fired>().bulletSpawner = bulletSpawner;
            temp.GetComponent<Fired>().enabled = true;
            temp.GetComponent<Fired>().start = true;
            fired = true;
            timer = 0.1f;

        }
        if(timer <= 0)
        {
            //Disables muzzle flash when set time is elapsed
            muzzleFlash.GetComponent<ParticleSystem>().Stop();
            fired = false;
        }
        else if(fired == true)
        {
            timer -= Time.deltaTime;
        }
    }
}

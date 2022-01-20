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
    bool enemy;
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
        enemy = false;
        if (transform.parent.root.name.Equals("Enemy") || transform.parent.root.name.Equals("Enemies"))
        {
            enemy = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //runs when left mouse button clicked
        if (enemy == false)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                int tempBullet = transform.GetComponent<Reload>().bulletCount;
                bool reloading = transform.GetComponent<Reload>().reloading;
                if (reloading == false)
                {
                    if (tempBullet > 0)
                    {
                        transform.GetComponent<Reload>().bulletCount--;

                        for (int i = 0; i < 3; i++)
                        {
                            FireWeapon(0);
                        }
                        FireWeapon(1);
                        FireWeapon(-1);



                    }
                    else
                    {
                        transform.GetComponent<Reload>().ReloadGun();
                    }
                }

            }
        }
        if (timer <= 0)
        {
            //Disables muzzle flash when set time is elapsed
            muzzleFlash.GetComponent<ParticleSystem>().Stop();
            fired = false;
        }
        else if (fired == true)
        {
            timer -= Time.deltaTime;
        }

    }
    public void FireWeapon(float up)
    {
        transform.GetComponent<AudioSource>().Play();
        
        //Enables muzzle Flash
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        //duplicates bullet
        GameObject temp = Instantiate(bullet);
        temp.GetComponentInChildren<MeshRenderer>().enabled = false;
        //sets new bullets position and rotation
        temp.transform.position = bulletSpawner.transform.position;
        temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + up, temp.transform.position.z);
        float y = bulletSpawner.transform.eulerAngles.y - 10;
        float x = bulletSpawner.transform.eulerAngles.x - 90;
        temp.transform.rotation = Quaternion.Euler(x, y, bulletSpawner.transform.rotation.z);
        //enables fired script and passes relevant variables needed by the fired script
        temp.GetComponent<Fired>().origin = this.gameObject.transform.parent.root.gameObject;
        temp.GetComponent<Fired>().gun = this.gameObject;
        temp.GetComponent<Fired>().bulletSpawner = bulletSpawner;
        temp.GetComponent<Fired>().enabled = true;
        temp.GetComponent<Fired>().start = true;
        fired = true;
        timer = 0.1f;
    }
}

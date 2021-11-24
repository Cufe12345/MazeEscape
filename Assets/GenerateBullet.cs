using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBullet : MonoBehaviour
{
    GameObject bullet;
    GameObject bulletSpawner;
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
                break;
            }
        }
        Debug.LogWarning(bulletSpawner);
    }

    // Update is called once per frame
    void Update()
    {
        //runs when left mouse button clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //duplicates bullet
            GameObject temp = Instantiate(bullet);
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
            

        }
    }
}

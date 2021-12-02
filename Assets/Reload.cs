using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    public int bulletCount;
    GameObject bulletText;
    int totalBullet;
    Animator reloadAnimation;
    public bool reloading;
    // Start is called before the first frame update
    void Start()
    {
        bulletText = GameObject.Find("BulletText");
        bulletCount = 6;
        totalBullet = 6;
        reloadAnimation = GetComponent<Animator>();
        reloadAnimation.enabled = false;
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.GetComponent<Text>().text = bulletCount.ToString() + "/"+totalBullet.ToString();
        if (Input.GetKeyDown(KeyCode.R) && reloading == false)
        {
            ReloadGun();
        }
    }
    public void ReloadGun()
    {
        StartCoroutine(bulletAni(totalBullet));
    }
    IEnumerator bulletAni(int bullets)
    {
        while (bullets > 0 && bulletCount<totalBullet)
        {
            reloading = true;
            reloadAnimation.enabled = true;
            yield return new WaitForSeconds(2.5f/reloadAnimation.parameters[0].defaultFloat);
            reloadAnimation.enabled = false;
            bullets--;
            bulletCount++;
        }
        reloading = false;
        
        
    }
}

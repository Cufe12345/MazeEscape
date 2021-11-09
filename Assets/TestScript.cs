using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    GameObject[] m;

    // Start is called before the first frame update
    void Start()
    {
        m = GameObject.FindGameObjectsWithTag("T");
    }

    // Update is called once per frame
    void Update()
    {
        m[0].transform.position = new Vector3(m[0].transform.position.x, 0, m[0].transform.position.z);
        transform.position = m[0].transform.position;
    }
}

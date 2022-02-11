using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos;
    Vector3 startRotation;
    Vector3 targetRotation;
    float speed;
    public bool aiming;
    // Start is called before the first frame update
    void Start()
    {
        aiming = false;
        //Sets all the values for the start position and rotation and the aim down sight(ADS) position and rotation
        startPos = transform.localPosition;
        startRotation.x = 28.59129f;
        startRotation.y = 294.0886f;
        startRotation.z = 1.993935f;  
        targetPos.x = -0.189f;
        targetPos.y = -0.033f;
        targetPos.z = 0.14f;
        targetRotation.x = 30.28744f;
        targetRotation.y = 293.9562f;
        targetRotation.z = 3.408987f;
        //ADS speed
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1)){
            //Moves the gun to the ADS position over time
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed*Time.deltaTime);
           //Sets the guns rotation to the ADS rotation
            transform.localRotation = Quaternion.Euler(targetRotation);
            aiming = true;
        }                         
        else if(transform.localPosition!= startPos)
        {
            //Moves the gun to the start position over time
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, speed * Time.deltaTime);
            //Sets the guns rotation to the start rotation
            transform.localRotation = Quaternion.Euler(startRotation);
            aiming = false;
        }
    }
}

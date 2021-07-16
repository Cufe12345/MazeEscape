using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{  
    float speed = 500;
    float horizontalSpeed = 90;
    float verticalSpeed = 90;
    Rigidbody rb;
    GameObject player;
    GameObject camera;
    List<int> keysPressed = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody>();
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        //Makes sure player is on the ground 
        if(transform.position.y < 9)
        {
            transform.position = new Vector3(transform.position.x, 50, transform.position.z);
        }
        //rotates player horizontally when mouse is moved
        transform.Rotate(0,Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime,0);
        //rotates player vertically when mouse is moved
        float rotation = -Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime;
        rotation += camera.transform.eulerAngles.x;
        //Changes the camera rotation by the amount moved by mouse
        camera.transform.localRotation= Quaternion.Euler(rotation, 0, 0);



      //stops the camera being rotated vertically too far
        if (camera.transform.localEulerAngles.x > 70 && camera.transform.localEulerAngles.x < 178)
        {
            Debug.LogError("Local Euler angle " + camera.transform.localEulerAngles.x);
            camera.transform.localRotation= Quaternion.Euler(70, 0, 0);
        }
        if (camera.transform.localEulerAngles.x <= 290 && camera.transform.localEulerAngles.x >178)
        {
            Debug.LogError("Local Euler angle " + camera.transform.localEulerAngles.x);
            camera.transform.localRotation = Quaternion.Euler(290, 0, 0);
        }

    }
    void FixedUpdate()
    {
        //Resets the list of keys pressed
        keysPressed = new List<int>();
        //Adds the key to the key pressed list when the key is pressed
        if (Input.GetKey(KeyCode.A))
        {
            keysPressed.Add(1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            keysPressed.Add(2);
        }
        if (Input.GetKey(KeyCode.W))
        {
            keysPressed.Add(3);
        }
        if (Input.GetKey(KeyCode.S))
        {
            keysPressed.Add(4);
        }
        
        //speeds up and slows down player
        if (Input.GetKey(KeyCode.N))
        {
            speed -= 10;
        }
        if (Input.GetKey(KeyCode.M))
        {
            speed += 10;
        }
        //passes the keysPressed array to the Final move function
        FinalMove(keysPressed);

    }
    public void FinalMove(List<int> theValues)
    {
        //for each key in the, theValues array it passes it to the Move function
        float x = 0;
        float z = 0;
        foreach(int i in theValues)
        {
            float[] temp = Move(i);
            //adds the x and z value returned from the move function to the x and z floats
            x += temp[0];
            z += temp[1];
        }
        //x and z is now the resultant velocity from the keys pressed so sets the velocity of the player to make them
        //move
        rb.velocity = new Vector3(x * speed * Time.deltaTime, rb.velocity.y, z * speed * Time.deltaTime);
    }
    public float[] Move(int direction)
    {
        float[] components2 = new float[2];
        //Checks which direction the player wants to move in depending on the direction provided
        if (direction == 1) {
            //Gets players y rotation value
            float rotation = player.transform.eulerAngles.y;
            //passes its y rotation to the CalculateDirection function and it returns a float array which is stored
            //in components
            float[] components = CalculateDirection(rotation);
            //sets the first 2 values in the components2 array to components recieved from the calculateDirection
            //function
            components2[0] = -components[1];
            components2[1] = components[0];
        }
        else if(direction == 2)
        {
            //Same except sign changes due to direction
            float rotation = player.transform.eulerAngles.y;
            float[] components = CalculateDirection(rotation);
            components2[0] = components[1];
            components2[1] = -components[0];
        }
        else if (direction == 3)
        {
            //Same except sign changes due to direction
            float rotation = player.transform.eulerAngles.y;
            float[] components = CalculateDirection(rotation);
            components2[0] = components[0];
            components2[1] = components[1];
        }
        else if (direction == 4)
        {
            //Same except sign changes due to direction
            float rotation = player.transform.eulerAngles.y;
            float[] components = CalculateDirection(rotation);
            components2[0] = -components[0];
            components2[1] = -components[1];
        }
        return components2;
    }
    public float[] CalculateDirection(float rotation)
    {
        //This function calculates the magnitude of x to z depending on the direction the player is facing
        //eg if they are rotated by 0 degrees x=1,z = 0 if they are 45 degrees x=0.5 z = 0.5 etc
        float x = 0;
        float z = 0;
        if (rotation >= 0 && rotation < 90)
        {
            if (rotation == 0)
            {
                z = 1;
                x = 0;
            }
            else
            {
                rotation = rotation / 90;
                x = rotation;
                rotation = 1 - rotation;
                z = rotation;

            }
        }
        else if (rotation >= 90 && rotation < 180)
        {
            rotation -= 90;
            if (rotation == 0)
            {
                z = 0;
                x = 1;
            }
            else
            {
                rotation = rotation / 90;
                z = -rotation;
                rotation = 1 - rotation;
                x = rotation;

            }


        }
        else if (rotation >= 270 && rotation < 360)
        {
            rotation -= 270;
            if (rotation == 0)
            {
                x = -1;
                z = 0;
            }
            else
            {


                rotation = rotation / 90;
                z = rotation;
                rotation = 1 - rotation;
                x = -rotation;
            }


        }
        else if (rotation >= 180 && rotation < 270)
        {
            rotation -= 180;
            if (rotation == 0)
            {
                z = -1;
                x = 0;
            }
            else
            {
                rotation = rotation / 90;
                x = -rotation;
                rotation = 1 - rotation;
                z = -rotation;

            }


        }
        //returns the values of x and z by storing in an array;
        float[] temp = new float[2];
        temp[0] = x;
        temp[1] = z;
        return temp;
    }
}

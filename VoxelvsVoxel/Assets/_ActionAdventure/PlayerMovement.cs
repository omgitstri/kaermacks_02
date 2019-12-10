using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    // Create public variables for player speed.
    public float walkSpeed;

    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    //private Rigidbody rb;


    // At the start of the game..
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        //rb = GetComponent<Rigidbody>();

    }

    // Each physics step..
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey("w"))
        {
            transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * walkSpeed * 3f;
        }
        else if (Input.GetKey("w") & !Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * walkSpeed;
        }
        else if (Input.GetKey("s"))
        {
            transform.position -= transform.TransformDirection(Vector3.forward) * Time.deltaTime * walkSpeed;
        }
        if (Input.GetKey("a") && !Input.GetKey("d"))
        {
            transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * walkSpeed;
        }
        else if (Input.GetKey("d") && !Input.GetKey("a")){
            transform.position -= transform.TransformDirection(Vector3.left) * Time.deltaTime * walkSpeed;
        }
        // Set some local float variables equal to the value of our Horizontal and Vertical Inputs
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
        //Vector3 movement = new Vector3(0, 0.0f, 0);

        // Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
        // multiplying it by 'speed' - our public player speed that appears in the inspector
        //rb.AddForce(movement * speed);
        /*
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(0,0,1) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(0, 0, -1) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(-1, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(1, 0, 0) * Time.deltaTime;
        }*/

    }
}
	

		
	

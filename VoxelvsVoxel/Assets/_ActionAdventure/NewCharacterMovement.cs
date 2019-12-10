using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacterMovement : MonoBehaviour
{
    //BASIC VARIABLES
    [SerializeField]
    private float speed = 5f; //Base movement speed.

    public Transform myTransform; //This object's transform component (used for optimization).
    private CharacterController myController; //Reference to Character Controller component on GO.
    Rigidbody m_RigidBody;

    public Collider playerCollider; //Collider on Character Controller.
    public Transform mainCameraTransform;

    public Camera mainCamera; //Main Camera.

    private Animator myAnimator; //Reference to Animator component on GO.
    Animator m_Animator;
    private AnimatorStateInfo myAnimatorStateInfo;// ???
    private float myAnimatorNormalizedTime = 0.0f;// ???

    //STATE MACHINE
    private string currentState = "Idle";

    //MOVEMENT/ANIMATIOM VARIABLES
    private Vector3 moveDirection = Vector3.zero;//Vector to move towards?
    private float myYVelocity = 0.0f; //Current Y speed of the avatar.
    public Vector3 m_Movement;
    
    public float turnSpeed = 20f;

    Quaternion m_Rotation = Quaternion.identity;

    //JUMP VARIABLES

    //Gravity
    private float gravity = -9.81f;
    private float terminalYVelocity = -9f; //Meters per seconds.
    private float myDefaultYVelocity = -1.0f; //Default Y speed when the character is grounded.

    //Jump
    public float jumpYVelocity = 3f; //Initial Jump speed.
    public float jumpCancelYVelocity = 6f; //Speed to  which the jump can be cancelled.

    //INPUT VARIABLES
    private float mouseXInput = 0.0f; //To know if the player is aiming left or right with the mouse.
    private float mouseYInput = 0.0f; //To know if the player is aiming up or down with the mouse.
    private float verticalInput = 0.0f; //To know if the player is pressing up (w) or down (s).
    private float horizontalInput = 0.0f; //To know if the player is pressing left or right (a or d).
    private float zAxisInput = 0.0f; // To know if the player is moving towards or away from the screen (r or f).

    private float mouseSensitivity = 3.0f; //Mouse speed.

    private bool runInput = false; // To know if the player is pressing Run key.
    private bool attackInput = false; // To know if the player is pressing Attack key.

    //--- STATE VARIABLES
    private bool isTurning = false;

    public void Awake()
    {
        mainCamera = Camera.main;
        myAnimator = GetComponent<Animator>();
        m_Animator = GetComponent<Animator>();
        myController = GetComponent<CharacterController>();
        
    }

    void FixedUpdate()
    {
        ApplyGravity();
        MoveCharacter();
        RunStates();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetButton("Fire3"))
        {
            speed = 6f;
        }
        else if (!Input.GetButton("Fire3"))
        {
            speed = 3f;
        }

        m_Movement.Set(horizontal, zAxisInput, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool haszAxisInput = !Mathf.Approximately(zAxisInput, 0f);

        bool isWalking = hasHorizontalInput || haszAxisInput;
        bool isIdle = !hasHorizontalInput && !hasVerticalInput && !haszAxisInput;
        bool isRunning = hasHorizontalInput || haszAxisInput && speed > 5f;
        bool isJumping = !myController.isGrounded;

        if (speed > 5f && hasHorizontalInput == true)
        {
            isWalking = false;
            isRunning = true;
        }
        if (speed < 6f && hasHorizontalInput == true)
        {
            isWalking = true;
            isRunning = false;
        }
        if (isWalking == true)
        {
            speed = 3f;
        }

        m_Animator.SetBool("Walking", isWalking);
        m_Animator.SetBool("Idle", isIdle);
        m_Animator.SetBool("Running", isRunning);
        m_Animator.SetBool("Jumping", isJumping);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        m_RigidBody.MovePosition(m_RigidBody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_RigidBody.MoveRotation(m_Rotation);
    }
    void LateUpdate()
    {
        moveDirection = transform.TransformDirection(new Vector3(0, myYVelocity, 0)); //Set direction.
        myController.Move(moveDirection * Time.deltaTime); //Apply movement.
    }
    void ApplyGravity()
    {
        if (myYVelocity > terminalYVelocity)
        {
            myYVelocity += gravity * Time.deltaTime;
        }
        else if (myYVelocity != terminalYVelocity)
        {
            myYVelocity = terminalYVelocity;
        }
        else
        {
            myYVelocity = myDefaultYVelocity;
        }
    }

    //RUN STATES

    void RunStates()
    {
        //Jump

        if (Input.GetButton("Jump"))
        {
            if (myController.isGrounded)
            {
                StartJump();
            }

        }

        // Walk

        if (horizontalInput != 0 || zAxisInput != 0)
        {
            if (currentState != "Walking")
            {
                StartWalk();
            }
        }

        //Run

        if (horizontalInput >= 0 && (Input.GetButton("Fire3")))                              //&& currentState != "isGettingHit") //&& myAnimator.GetFloat("Speed") > 5f))
        {
            StartRun();            //RUNNING SOUND!
        }

        //Idle

        else if (currentState == "none")
        {
            StartIdle();
        }

        //OVERLAPPING STATES

        StartTurn();
        {
            isTurning = true;
            Turn();
        }

        void Turn()
        {
            if (mouseXInput != 0) { myTransform.Rotate(new Vector3(0, 1, 0) * mouseXInput * mouseSensitivity, Space.Self); }
            else { StopTurn(); }
        }

        void StopTurn()
        {
            isTurning = false;
        }

        //Turn
        if (mouseXInput != 0)
        {
            StartTurn();
        }

        //ONGOING STATES
        else if (currentState == "Running") Run();
        else if (currentState == "Walking") Walk();
        else if (currentState == "Idle") Idle();
        else if (currentState == "Jumping") Jump();

        //Animator State Info
        myAnimatorStateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);
        myAnimatorNormalizedTime = myAnimatorStateInfo.normalizedTime;

    }
    void ResetStates()
    {
        StopRun();
        StopWalk();
        StopIdle();
        StopJump();
    }

    //STATE FUNCTIONS

    //JUMPING
    void StartJump()
    {
        ResetStates();
        currentState = "Jumping";
        myAnimator.SetBool("Jumping", true);
        myYVelocity = jumpYVelocity;
    }

    void Jump()
    {
        currentState = "Jumping";
        myAnimator.SetBool("Jumping", true);

        if (!Input.GetButtonDown("Jump")) //Cancel the jump momentum if the jump button is released.
        {
            myYVelocity = jumpCancelYVelocity;
        }
        if (myYVelocity >= jumpCancelYVelocity) //Cancel the jump momentum if the Player's Yspeed exceeds YCancel velocity.
        {
            StopJump();
        }
    }
    void StopJump()
    {
        currentState = "none";
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetFloat("walkInputPosZ", 0);
        myAnimator.SetBool("Jumping", false);
    }

    //WALKING

    void StartWalk()
    {
        ResetStates();
        currentState = "Walking";
        myAnimatorNormalizedTime = 0f;
        myAnimator.SetBool("Walking", true);
        myAnimator.SetFloat("Speed", 3f);
        speed = 3f;
    }

    void Walk()
    {
        myAnimator.SetFloat("walkInputPosY", verticalInput);         //Update animator parameters.
        myAnimator.SetFloat("walkInputPosX", horizontalInput);
        myAnimator.SetFloat("walkInputPosZ", zAxisInput);
        myAnimator.SetFloat("Speed", 3f);

        //EXIT WALK
        if (zAxisInput == 0f || horizontalInput == 0f)
        {
            StopWalk();
        }
    }

    void StopWalk()
    {
        currentState = "none";
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetFloat("walkInputPosZ", 0);
        myAnimator.SetBool("Walking", false);
    }

    //RUNNING

    void StartRun()
    {
        ResetStates();
        currentState = "Running";
        myAnimator.SetBool("Running", true);
        myAnimator.SetFloat("Speed", 6f);
        speed = 6f;
    }

    void Run()
    {
        ResetStates();
        currentState = "Running";
        myAnimator.SetFloat("Speed", 6f);
        myAnimator.SetBool("Running", true);
        if (verticalInput == 0 && horizontalInput == 0)
        {
            StopRun();
        }
    }

    void StopRun()
    {
        currentState = "none";
        myAnimator.SetFloat("runTurnInput", 0);
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetFloat("walkInputPosZ", 0);
        myAnimator.SetBool("Running", false);
    }

    //IDLE

    void StartIdle()
    {
        ResetStates();
        myAnimatorNormalizedTime = 0f;
        currentState = "Idle";
        myAnimator.SetBool("Idle", true);
    }

    void Idle()
    {
        currentState = "Idle";
        myAnimator.SetBool("Idle", true);
    }

    void StopIdle()
    {
        currentState = "none";
        myAnimator.SetBool("Idle", false);
    }

    //TURN

    void StartTurn()
    {
        isTurning = true;
    }

    void Turn()
    {
        if (mouseXInput != 0) { myTransform.Rotate(new Vector3(0, 1, 0) * mouseXInput * mouseSensitivity, Space.Self); }
        else { StopTurn(); }
    }

    void StopTurn()
    {
        isTurning = false;
    }

    //Character Rotation
    public void RotateCharacter()
    {
        Quaternion desiredRotation = Quaternion.LookRotation(new Vector3((transform.position.x - mainCameraTransform.position.x), 0, (transform.position.z - mainCameraTransform.position.z)), Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 5f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 1f * Time.deltaTime);
    }

    //Character Movement
    public void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float zAxis = Input.GetAxis("ZAxis");

        verticalInput = vertical;
        horizontalInput = horizontal;
        zAxisInput = zAxis;

        Vector3 moveDirection = new Vector3(horizontal, 0f, zAxis);
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.Self);
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}

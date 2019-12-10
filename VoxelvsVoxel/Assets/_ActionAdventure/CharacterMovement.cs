using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    //BASIC VARIABLES:

    [SerializeField]
    private float speed = 5f;
    private float gravity = -9.81f;

    private Transform myTransform; //This object's transform component (used of optimization)

    private Transform mainCamera;
    private Rigidbody rb;
    private AttackMovement attackMovement;
    private Animator myAnimator;
    private AnimatorStateInfo myAnimatorStateInfo;
    private float myAnimatorNormalizedTime = 0.0f;

    private int attackCombo = 0;
    public bool move;

    [SerializeField] private float dashTime = 0.5f;
    private float dashF;

    //INPUT VARIABLES
    private float mouseXInput = 0.0f; //To know if the player is aiming left or right with the mouse.
    private float mouseYInput = 0.0f; //To know if the player is aiming up or down with the mouse.
    private float verticalInput = 0.0f; //To know if the player is pressing up or down (w or s)
    private float horizontalInput = 0.0f; //To know if the player is pressing left or right (a or d)
    private float mouseSensitivity = 3.0f; //how fast is the mouse.

    private bool runInput = false; // To know if the player is pressing "run"
    private bool attackInput = false; // To know if the player is pressing "attack"

    private bool dblTapInput1 = false; //To make the run also a double tap of the W key
    private bool dblTapInput2 = false;

    //--- LINK VARIABLES
    public GameObject myWeapon; //This is the weapon that is carried

    //--- STATE VARIABLES
    private string currentState = "none";
    private bool isTurning = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
        myAnimator = GetComponent<Animator>();
        attackMovement = GetComponent<AttackMovement>();
    }

    void EnableMove()
    {
        move = true;
    }
    void DisableMove()
    {
        move = false;
    }


    void Update()
    {

        //ApplyGravity();
        //RotateCharacter();
        MoveCharacter();

        if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Idle"))
        {
            if (move)
            {
                attackMovement.AttackForward();
            }
        }

        dashF -= Time.deltaTime;

        if (dashF > 0)
        {
            attackMovement.Dash();
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (dashF < -(dashTime * 2))
            {
                dashF = dashTime;
            }
        }

        RunStates();
    }

    void RunStates()
    {
        //Start new state using priority system

        //isAttacking

        if (Input.GetButtonDown("Fire1") && currentState != "isAttacking" && currentState != "isAttacking 0" && currentState != "isAttacking1")
        {
            StartAttack();
        }
        //else if (Input.GetButtonDown("Fire1"))
        //{
        //    ComboAttack();
        //    AttackSound(); //??????????????????????????????????
        //}

        //isAttacking 0
        if (Input.GetButtonDown("Fire1") && currentState != "isAttacking" && currentState != "isAttacking 0" && currentState != "isAttacking1")
        {
            StartAttack();
        }
        //isAttacking 1



        //Walk

        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (currentState != "isGettingHit" && currentState != "Walking")
            {
                StartWalk();
            }

            if (Input.GetButtonDown("Jump") && currentState != "isGettingHit" && currentState != "Dodge")
            {
                StartDodge();
            }

            if (Input.GetButtonDown("Jump") && Input.GetButtonDown("Left") && currentState != "isGettingHit" && currentState != "Dodge")
            {
                StartDodgeLeft();
            }
            if (Input.GetButtonDown("Jump") && Input.GetButtonDown("Right") && currentState != "isGettingHit" && currentState != "Dodge")
            {
                StartDodgeRight();
            }

            if (Input.GetButtonDown("Fire1") && currentState != "isGettingHit" && currentState != "isAttacking 0" && currentState != "isAttacking 1" )
            {
                StartAttack();
            }

            if (Input.GetButtonDown("Fire2")  && currentState != "isGettingHit" && currentState != "isAttacking 1")
            {
                StartAttack1();
            }

            if (Input.GetButtonDown("Fire3")  && currentState != "isGettingHit" && currentState != "isAttacking 0")
            {
                StartAttack0();
            }
        }

        ////Dodge Right
        //if (Input.GetAxis("Horizontal") > 0 && Input.GetButton("Jump") && currentState != "isGettingHit")
        //{
        //    StartDodgeRight();
        //    print("dodged right");
        //}

        ////Dodge Left
        //if (Input.GetAxis("Horizontal") < 0 && Input.GetButton("Jump") && currentState != "isGettingHit")
        //{
        //    StartDodgeLeft();
        //    print("dodged right");
        //}

        //Run

        if (Input.GetAxis("Vertical") > 0 && Input.GetButtonDown("Jump") && currentState != "Running" && currentState != "isGettingHit") //&& myAnimator.GetFloat("Speed") > 5f))
        {
            Run();
            //RUNNING SOUND!
        }

        //Idle
        else if (currentState == "none")
        {
            StartIdle();
        }

        //Get Hit

        //Death

        //Start overlapping states
        /*StartTurn();
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
        //Turn
        if (mouseXInput != 0)
        { StartTurn(); }*/

        //Run ongoing states
        if (currentState == "isGettingHit") GetHit();
        else if (currentState == "isAttacking") Attacking();
        else if (currentState == "Running") Run();
        else if (currentState == "Walking") Walk();
        else if (currentState == "Idle") Idle();
        else if (currentState == "Dodge") Dodge();
        else if (currentState == "Death") Death();

        //Animator State Info
        myAnimatorStateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);
        myAnimatorNormalizedTime = myAnimatorStateInfo.normalizedTime;
    }

    void ResetStates()
    {
        StopRun();
        StopWalk();
        StopIdle();
        StopAttack();
        StopGettingHit();
        StopDodge();
        StopDodgeLeft();
        StopDodgeRight();
        StopDeath();
        myAnimator.SetBool("ComboAttack", false);
    }

    //STATE FUNCTIONS

    //WALKING

    void StartWalk()
    {
        ResetStates();
        currentState = "Walking";
        myAnimatorNormalizedTime = 0f;
        myAnimator.SetBool("Walking", true);
    }

    void Walk()
    {
        //update animator parameters
        myAnimator.SetFloat("walkInputPosY", verticalInput);
        myAnimator.SetFloat("walkInputPosX", horizontalInput);

        //exit
        if (verticalInput == 0 && horizontalInput == 0)
        {
            StopWalk();
        }
    }

    void StopWalk()
    {
        currentState = "none";
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetBool("Walking", false);

    }

    //Dodge
    void StartDodge()
    {
        ResetStates();
        currentState = "Dodge";
        myAnimatorNormalizedTime = 0f;
        myAnimator.SetBool("Dodge", true);
    }

    void Dodge()
    {
        myAnimator.SetFloat("walkInputPosY", verticalInput);
        myAnimator.SetFloat("walkInputPosX", horizontalInput);

        if (myAnimatorNormalizedTime >= 1)
        {
            StopDodge();
        }
    }

    void StopDodge()
    {
        currentState = "none";
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetBool("Dodge", false);
    }

    //Dodge Right
    void StartDodgeRight()
    {
        currentState = "DodgeRight";
        myAnimatorNormalizedTime = 0f;
        myAnimator.SetBool("DodgeRight", true);
    }
    void DodgeRight()
    {
        myAnimator.SetFloat("walkInputPosY", verticalInput);
        myAnimator.SetFloat("walkInputPosX", horizontalInput);

        if (myAnimatorNormalizedTime >= 2)
        {
            StopDodgeRight();
        }
    }
    void StopDodgeRight()
    {
        currentState = "none";
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetBool("DodgeRight", false);
    }
    //Dodge Left
    void StartDodgeLeft()
    {
        currentState = "DodgeLeft";
        myAnimatorNormalizedTime = 0f;
        myAnimator.SetBool("DodgeLeft", true);
    }
    void DodgeLeft()
    {
        myAnimator.SetFloat("walkInputPosY", verticalInput);
        myAnimator.SetFloat("walkInputPosX", horizontalInput);

        if (myAnimatorNormalizedTime >= 2)
        {
            StopDodgeLeft();
        }
    }
    void StopDodgeLeft()
    {
        currentState = "none";
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetBool("DodgeLeft", false);
    }

    //RUNNING

    void StartRun()
    {
        ResetStates();
        currentState = "Running";
        myAnimator.SetBool("Running", true);
        myAnimator.SetFloat("Speed", 5f);
    }

    void Run()
    {
        ResetStates();
        currentState = "Running";
        myAnimator.SetFloat("Speed", 5f);
        myAnimator.SetBool("Running", true);
    }

    void StopRun()
    {
        currentState = "none";
        //myAnimator.SetFloat("runTurnInput", 0);
        myAnimator.SetFloat("walkInputPosY", 0);
        myAnimator.SetFloat("walkInputPosX", 0);
        myAnimator.SetBool("Running", false);
    }

    //IDLE
    void StartIdle()
    {
        DisableWeapon();
        ResetStates();
        myAnimatorNormalizedTime = 0f;
        currentState = "Idle";
        myAnimator.SetBool("Idle", true);
    }

    void Idle()
    {
        //nothing so far
    }

    void StopIdle()
    {
        currentState = "none";
        myAnimator.SetBool("Idle", false);

    }
    //ATTACKING
    void StartAttack()
    {
        EnableWeapon();
        ResetStates();
        myAnimatorNormalizedTime = 0;
        myAnimator.SetBool("isAttacking", true);
        currentState = "isAttacking";
    }

    void Attacking()
    {
        if (myAnimatorNormalizedTime >= 1f)
        {
            StopAttack();
        }
    }

    void StopAttack()
    {
        myAnimator.SetBool("isAttacking", false);
        currentState = "none";
        //myWeapon.SendMessage("DeactivateWeaponCollider");
    }

    //ATTACKING 1
    void StartAttack1()
    {
        EnableWeapon();
        ResetStates();
        myAnimatorNormalizedTime = 0;
        myAnimator.SetBool("isAttacking 1", true);
        currentState = "isAttacking 1";
    }

    void Attacking1()
    {
        if (myAnimatorNormalizedTime >= 1f)
        {
            StopAttack1();
        }
    }

    void StopAttack1()
    {
        myAnimator.SetBool("isAttacking 1", false);
        currentState = "none";
        //myWeapon.SendMessage("DeactivateWeaponCollider");
    }

    //ATTACKING 0
    void StartAttack0()
    {
        EnableWeapon();
        ResetStates();
        myAnimatorNormalizedTime = 0;
        myAnimator.SetBool("isAttacking 0", true);
        currentState = "isAttacking 0";
    }

    void Attacking0()
    {
        if (myAnimatorNormalizedTime >= 1f)
        {
            StopAttack0();
        }
    }

    void StopAttack0()
    {
        myAnimator.SetBool("isAttacking 0", false);
        currentState = "none";
        //myWeapon.SendMessage("DeactivateWeaponCollider");
    }

    //GETTING HIT
    public void StartGetHit()
    {
        ResetStates();
        currentState = "isGettingHit";
        myAnimatorNormalizedTime = 0;
        //myAnimator.SetBool("isGettingHit", true);
        myAnimator.SetTrigger("GetHit");
        Tester.health -= 1;        
    }

    void GetHit()
    {
        if (myAnimatorNormalizedTime >= 1)
        {
            StopGettingHit();
        }

    }

    void StopGettingHit()
    {
        currentState = "none";
        //myAnimator.SetBool("isGettingHit", false);
    }

    //DEATH
    void StartDeath()
    {
      //  StopGettingHit();
    }
    void Death()
    {
      //  StopGettingHit();
    }
    void StopDeath()
    {

    }

    private void ComboAttack()
    {
        myAnimator.SetFloat("RandAttack", Random.value);
        myAnimator.SetTrigger("ComboAttack");
        myAnimator.SetBool("ComboAttack", true);
        attackMovement.AttackForward();

        //audio for combo attack
    }

    //--- OVERLAPPING STATE FUNCTIONS
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
    private void RotateCharacter()
    {
        Quaternion desiredRotation = Quaternion.LookRotation(new Vector3((transform.position.x - mainCamera.position.x), 0, (transform.position.z - mainCamera.position.z)), Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 5f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 1f);
    }

    //Character Movement
    private void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        verticalInput = vertical;
        horizontalInput = horizontal;


        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.Self);
        //transform.Translate(moveDirection * speed * Time.deltaTime, target);
    }

    private void ApplyGravity()
    {
        rb.AddForce(Vector3.up * gravity);
    }

    void EnableWeapon()
    {
        myWeapon.GetComponent<Needle>()?.EnableWeapon();
        myWeapon.GetComponent<Blade>()?.EnableWeapon();
    }

    void DisableWeapon()
    {
        myWeapon.GetComponent<Needle>()?.DisableWeapon();
        myWeapon.GetComponent<Blade>()?.DisableWeapon();
    }
}

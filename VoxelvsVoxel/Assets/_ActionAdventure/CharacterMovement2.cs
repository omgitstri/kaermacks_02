using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2 : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    private float gravity = -9.81f;

    private Transform mainCamera;
    private Rigidbody rb;
    private AttackMovement attackMovement;
    private Animator myAnimator;
    private int attackCombo = 0;
    public bool move;

    [SerializeField] private float dashTime = 0.5f;
    private float dashF;

    //audio stuff
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioSource audiosource1;
    public AudioSource audiosource2;

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
            //            if(myAnimator.GetCurrentAnimatorStateInfo(0).length > 15f/60f & myAnimator.GetCurrentAnimatorStateInfo(0).length < 25f / 60f)
            if(move)
            {
                attackMovement.AttackForward();
            }
        }

        dashF -= Time.deltaTime;

        if (dashF > 0)
        {
            attackMovement.Dash();
        }

        if(Input.GetButtonDown("Jump"))
        {
            if (dashF < -(dashTime * 2))
            {
                dashF = dashTime;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                attackCombo = 0;
                attackCombo++;
                Attack();
            }
            else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Idle"))
            {
                attackCombo = 0;
                attackCombo++;
                Attack();
            }
            else
            {
                switch (attackCombo)
                {
                    case 1:
                        attackCombo++;
                        ComboAttack();
                        break;
                    default:
                        attackCombo = 0;
                        break;
                }
            }
        }
    }

    private void ComboAttack()
    {
        myAnimator.SetFloat("RandAttack", Random.value);
        myAnimator.SetTrigger("ComboAttack");
        //attackMovement.AttackForward();

        //audio for combo attack
        audiosource2.clip = attack2;
        audiosource2.Play();
    }

    private void Attack()
    {
        myAnimator.SetFloat("RandAttack", Random.value);
        myAnimator.SetTrigger("AnyAttack");
        //attackMovement.AttackForward();

        //audio for normal attack
        audiosource1.clip = attack1;
        audiosource1.Play();
    }

    private void RotateCharacter()
    {
        Quaternion desiredRotation = Quaternion.LookRotation(new Vector3((transform.position.x - mainCamera.position.x), 0, (transform.position.z - mainCamera.position.z)), Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 5f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 1f);
    }


    private void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.Self);
        //transform.Translate(moveDirection * speed * Time.deltaTime, target);
    }

    private void ApplyGravity()
    {
        rb.AddForce(Vector3.up * gravity);
    }
}

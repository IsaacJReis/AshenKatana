using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovimentPlayer : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;
    [SerializeField] private int speed = 5;
    [SerializeField] private Transform GroundChecker;
    [SerializeField] private LayerMask GroundLayer;
    private bool facingRight = true;



    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private int movemntingHash = Animator.StringToHash("movementing");
    private int jumpingHash = Animator.StringToHash("jumping");
    private int attackingHash = Animator.StringToHash("attacking");


    [SerializeField] private AudioSource stepsAudioSource;
    [SerializeField] private AudioClip[] stepsAudioClip;
    [SerializeField] private AudioClip[] JumpAudioClip;
    [SerializeField] private AudioSource JumpAudioSource;
    [SerializeField] private AudioSource AttackAudioSource;

    public Transform currentTarget;
    public float followSpeed = 5f;
    public float stopDistance = 1.5f;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {

        Attacking();

        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * 350);
            Jump();
        }
        isGrounded = Physics2D.OverlapCircle(GroundChecker.position, 0.2f, GroundLayer);


        animator.SetBool(movemntingHash, horizontalInput != 0);
        animator.SetBool(jumpingHash, !isGrounded);

        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        if (currentTarget != null) {
            float distance = Vector2.Distance(transform.position, currentTarget.position);

            if (distance > stopDistance) {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    currentTarget.position,
                    followSpeed * Time.deltaTime
                );
            }
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }


    private void Attacking()
    {
        if (Input.GetKey(KeyCode.J))
        {
            animator.SetTrigger(attackingHash);
            
        }
    }


    private void Steps()
    {
        stepsAudioSource.PlayOneShot(stepsAudioClip[Random.Range(0, stepsAudioClip.Length)]);
    }

    private void Jump()
    {

        JumpAudioSource.PlayOneShot(JumpAudioClip[Random.Range(0, JumpAudioClip.Length)]);
    }
    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
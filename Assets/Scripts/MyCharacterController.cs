using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;
    BoxCollider2D bc;

    public float speed;
    public float jumpForce;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    bool isPointingRight = true;
    bool isGrounded = false;
    public Transform frontCollider;
    public Transform groundCollider;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float rememberGroundedFor;

    float lastTimeGrounded;

    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    [HideInInspector]
    public int defaultAdditionalJumps = 1;
    [HideInInspector]
    public int additionalJumps;

    [HideInInspector]
    public bool isDead = false;

    private GameManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        StartPlayer();
        defaultAdditionalJumps = gm.playerAditionalJumps;
        additionalJumps = defaultAdditionalJumps;
    }

    void Update()
    {
        CheckIfIsGrounded();
        if (!isDead)
        {
            Move();
            if (ShouldJump())
            {
                Jump();
            }
            SmoothenJump();
        }
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (IsColidingFront() && ((x == 1 && isPointingRight) || (x == -1 && !isPointingRight))  ) return;

        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);

        if (this.isGrounded)
        {
            anim.SetTrigger(IsWalking(x) ? "isWalking" : "isIdle");
        }
        if (ShouldTurn(x))
        {
            Flip();
        }
    }

    void Flip()
    {
        isPointingRight = !isPointingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void Jump(float forcex, float forceY)
    {
        anim.SetTrigger("isJumping");
        rb.velocity = new Vector2(rb.velocity.x, forceY);
        additionalJumps--;
    }

    void Jump()
    {
        audioSource.PlayOneShot(jumpSound);
        Jump(rb.velocity.x, jumpForce);
    }

    public void Jump(float forceY)
    {
        Jump(rb.velocity.x, forceY);
    }

    public void JumpBack(float forceY)
    {
        Jump((rb.velocity.x * -1), forceY);
    }

    void SmoothenJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckIfIsGrounded()
    {
        Collider2D colliders = Physics2D.OverlapBox(groundCollider.position, new Vector2(bc.size.x - 0.15f, 0.1f), 0, groundLayer);
        
        if (colliders != null)
        {
            isGrounded = true;
            additionalJumps = defaultAdditionalJumps;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
            IsFallingToDeath();
        }
    }

    bool IsColidingFront()
    {
        return Physics2D.OverlapBox(frontCollider.position, new Vector2(0.015f, bc.size.y - 0.05f), 0, groundLayer);
    }

    void IsFallingToDeath()
    {
        if (isDead) return;
        if (gm.cameraHandler.yMin > transform.position.y)
        {
            gm.DealDamage(gm.playerMaxHealth);
            isDead = true;
        }
    }

    void StartPlayer()
    {
        gm = GameManager.GetInstance();
        gm.characterController = this;
        gm.playerPos = transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        if (collision.gameObject.CompareTag("MortalDanger"))
        {
            gm.DealDamage(gm.playerMaxHealth);
        }
    }

    bool ShouldJump()
    {
        return Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0);
    }

    bool IsWalking(float translationX)
    {
        return translationX != 0 && this.isGrounded;
    }

    bool ShouldTurn(float translationX)
    {
        return ((translationX > 0 && !this.isPointingRight) || (translationX < 0 && this.isPointingRight));
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(frontCollider.position, new Vector2(0.015f, bc.size.y));
        Gizmos.DrawWireCube(groundCollider.position, new Vector2(bc.size.x - 0.15f, 0.1f));
    }
    */
}
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
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

    public int defaultAdditionalJumps = 1;
    int additionalJumps;

    [HideInInspector]
    public bool isDead = false;

    private GameManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();

        additionalJumps = defaultAdditionalJumps;

        StartPlayer();
    }

    void Update()
    {
        CheckIfGrounded();
        if (!isDead)
        {
            Move();
            if (shouldJump())
            {
                Jump();
            }
            BetterJump();
        }
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (CheckIfColidingFront() && ((x == 1 && isPointingRight) || (x == -1 && !isPointingRight))  ) return;

        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);

        if (this.isGrounded)
        {
            anim.SetTrigger(isWalking(x) ? "isWalking" : "isIdle");
        }

        if (shouldTurn(x))
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
        Jump(rb.velocity.x, jumpForce);
    }

    void BetterJump()
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

    public void Jump(float forceY)
    {
        Jump(rb.velocity.x, forceY);
        anim.SetTrigger("isJumping");
        rb.velocity = new Vector2(rb.velocity.x, forceY);
        additionalJumps--;
    }

    public void JumpBack(float forceY)
    {
        Jump((rb.velocity.x * -1), forceY);
    }

    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(groundCollider.position, checkGroundRadius, groundLayer);

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
            CheckIfFellToDeath();
        }
    }

    bool CheckIfColidingFront()
    {
        return Physics2D.OverlapBox(frontCollider.position, new Vector2(1, bc.size.y), 0, groundLayer);
    }

    void CheckIfFellToDeath()
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

    bool shouldJump()
    {
        return Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0);
    }

    bool isWalking(float translationX)
    {
        return translationX != 0 && this.isGrounded;
    }

    bool shouldTurn(float translationX)
    {
        return ((translationX > 0 && !this.isPointingRight) || (translationX < 0 && this.isPointingRight));
    }
}
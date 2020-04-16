using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public float respawnDelay = 0f;
    public Transform respawnLocation;
    public bool isPreventingFall = false;
    public bool isPointingRight = false;

    public Transform sight;
    public Transform fallDetector;
    public float checkSightRadius = 0.5f;
    public LayerMask trackLayer;

    private GameManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        int x = isPointingRight ? 1 : -1;
        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        if (isPreventingFall && WillFall())
        {
            Rotate();
            return;
        }
        CheckIfShouldDie();
    }

    private bool WillFall()
    {
        return Physics2D.OverlapCircle(fallDetector.position, 0.01f, trackLayer) == null;
    }

    private void CheckIfShouldDie()
    {
        if (gm.cameraHandler.yMin > transform.position.y)
        {
            if (respawnLocation)
            {
                StartCoroutine(Respawn());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Rotate()
    {
        isPointingRight = !isPointingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        rb.velocity = Vector2.zero;
        transform.position = respawnLocation.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.gameObject.Equals(gameObject) || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("NoTouch")))
        {
            Rotate();
        }
    }
}

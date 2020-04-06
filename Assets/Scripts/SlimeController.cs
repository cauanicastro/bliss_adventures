using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public float respawnDelay = 0f;
    public Transform respawnLocation;
    public bool isPointingRight = false;

    public Transform sight;
    public float checkSightRadius = 0.1f;
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
        if (CheckIfColidingFront())
        {
            isPointingRight = !isPointingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        CheckIfShouldDie();
    }
    private bool CheckIfColidingFront()
    {
        return Physics2D.OverlapCircle(sight.position, checkSightRadius, trackLayer);
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

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        rb.velocity = Vector2.zero;
        transform.position = respawnLocation.position;
    }
}

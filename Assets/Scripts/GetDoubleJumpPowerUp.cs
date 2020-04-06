using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDoubleJumpPowerUp : MonoBehaviour
{
    GameManager gm;
    public float floatAmplitude = 0.1f;
    public float floatFrequency = 1f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    void Start()
    {
        gm = GameManager.GetInstance();
        this.posOffset = transform.position;
    }

    private void Update()
    {
        this.tempPos = this.posOffset;
        this.tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * this.floatFrequency) * this.floatAmplitude;

        transform.position = tempPos;

        transform.Rotate(new Vector3(0, 0, 1), 30 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gm.ActivateDoubleJumpPowerUp();
        }
    }
}

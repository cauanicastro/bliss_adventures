using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    private GameManager gm;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        gm = GameManager.GetInstance();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("isActive");
            gm.characterController.Jump(10);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetTrigger("isIdle");
    }
}

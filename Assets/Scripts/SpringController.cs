using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    private GameManager gm;
    private Animator anim;
    private AudioSource audioSource;

    public AudioClip springSound;
    public float jumpForce = 10;

    void Start()
    {
        anim = GetComponent<Animator>();
        gm = GameManager.GetInstance();
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(springSound);
            anim.SetTrigger("isActive");
            gm.characterController.Jump(jumpForce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetTrigger("isIdle");
    }
}

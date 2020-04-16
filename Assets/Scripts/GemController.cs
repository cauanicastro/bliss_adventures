using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager gm = GameManager.GetInstance();
            gm.AddGem();
            if (collectSound)
            {
                gm.characterController.audioSource.PlayOneShot(collectSound);
            }
            Destroy(gameObject);
        }
    }
}

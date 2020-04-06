using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Math;

[System.Serializable]
public class GameManager
{
    [Header("Camera")]
    public Camera mainCamera;
    public CameraFollow cameraHandler;

    [Header("HUD")]
    public HudController hudController;

    [Header("Player props")]
    public MyCharacterController characterController;
    public Transform playerPos;
    public float playerHealth = 100;
    public float playerMaxHealth = 100;
    public float playerLives = 3;
    public float playerGems = 0;
    public int playerAditionalJumps = 0;
    public List<KeyScOb> playerKeys = new List<KeyScOb>();

    [Header("Stage")]
    public StageManagerController stageManager;


    private static GameManager instance;

    private GameManager() {}

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }

    public void KeyFind(KeyScOb key)
    {
        playerKeys.Add(hudController.AddKey(key));
    }

    public void DealDamage(float amount)
    {
        playerHealth = Max(playerHealth - amount, 0);
        if (playerHealth == 0)
        {
            LifeDecrease();
        }
    }

    public void DealRecover(float amount)
    {
        playerHealth = Min(playerHealth + amount, playerMaxHealth);
    }

    public void LifeIncrease()
    {
        ++playerLives;
    }

    private void LifeDecrease()
    {
        --playerLives;
        if (playerLives < 0)
        {
            characterController.StartCoroutine(GameOver());
            return;
        }
        characterController.StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        if (characterController.isDead) yield break;

        characterController.isDead = true;
        characterController.GetComponent<Animator>().SetTrigger("isDead");
        yield return new WaitForSeconds(1.5f);
        characterController.StartCoroutine(DeactivatePowerups(0));

        stageManager.RestartStage();
        hudController.RemoveLife();

        playerHealth = playerMaxHealth;
        playerGems = 0;
        playerKeys = new List<KeyScOb>();
    }

    private IEnumerator GameOver()
    {
        characterController.isDead = true;
        characterController.GetComponent<Animator>().SetTrigger("isDead");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        instance = new GameManager();
    }

    public void ActivateDoubleJumpPowerUp()
    {
        characterController.StopCoroutine("DeactivatePowerups");
        characterController.additionalJumps = characterController.defaultAdditionalJumps = 2;
        characterController.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        characterController.StartCoroutine(DeactivatePowerups(10));
    }

    public IEnumerator DeactivatePowerups(float delay)
    {
        yield return new WaitForSeconds(delay);
        characterController.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        characterController.defaultAdditionalJumps = characterController.additionalJumps = playerAditionalJumps;
    }
}

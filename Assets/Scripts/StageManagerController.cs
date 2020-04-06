using System.Collections.Generic;
using UnityEngine;

public class StageManagerController : MonoBehaviour
{
    GameManager gm;

    public float currentStage = 1;

    //Terrible implementation, but time was really short on my side here...
    [Header("Stage 1")]
    public BoxCollider2D stageLimits1;
    public Transform stageStart1;
    [Header("Stage 2")]
    public BoxCollider2D stageLimits2;
    public Transform stageStart2;
    [Header("Stage 3")]
    public BoxCollider2D stageLimits3;
    public Transform stageStart3;

    void Start()
    {
        gm = GameManager.GetInstance();
        gm.stageManager = this;
    }

    public void RestartUser(Transform teleportLocation)
    {
        GameObject player = gm.characterController.gameObject;
        gm.characterController.rb.velocity = Vector2.zero;
        gm.characterController.isDead = false;
        gm.characterController.anim.Rebind();
        player.transform.position = teleportLocation.position;
    }

    public void RestartStage()
    {
        Transform location;
        switch (currentStage)
        {
            case 1:
                location = stageStart1;
                break;
            case 2:
                location = stageStart2;
                break;
            case 3:
                location = stageStart3;
                break;
            default:
                throw new MissingReferenceException();
        }
        RestartUser(location);
    }

    public void GoToNextStage()
    {
        currentStage++;
        switch (currentStage)
        {
            case 1:
                gm.cameraHandler.mapBounds = stageLimits1;
                gm.characterController.gameObject.transform.position = stageStart1.position;
                break;
            case 2:
                gm.cameraHandler.mapBounds = stageLimits2;
                gm.characterController.gameObject.transform.position = stageStart2.position;
                break;
            case 3:
                gm.cameraHandler.mapBounds = stageLimits3;
                gm.characterController.gameObject.transform.position = stageStart3.position;
                break;
            default:
                break;
        }
        StartCoroutine(gm.DeactivatePowerups(0));
        gm.cameraHandler.UpdateCameraBounds();
    }
}
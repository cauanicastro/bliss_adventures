using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/Stage", order = 2)]
public class StageScOb : ScriptableObject
{
    public BoxCollider2D stageLimits;
    public Transform startPlace;
    public Sprite backGroundImage;
}
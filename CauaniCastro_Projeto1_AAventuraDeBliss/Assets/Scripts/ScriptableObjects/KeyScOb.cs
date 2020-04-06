using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Key", order = 1)]
public class KeyScOb : ScriptableObject
{
    public string id;
    public GameObject objReference;
    public GameObject objImage;
    public string colour;
}

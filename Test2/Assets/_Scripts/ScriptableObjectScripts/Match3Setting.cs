using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "setting/game")]
public class Match3Setting : ScriptableObject
{
    public int size;
    public float animationSpeed;
}

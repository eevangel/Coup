using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PolicyCardTheming", order = 1)]
public class PolicyCardTheming : ScriptableObject
{

    [Header("FASCIST DISCARD")]
    public ColorBlock _fascistDiscard;
    [Header("FASCIST Keep")]
    public ColorBlock _fascistKeep;
    [Header("LIB  DISCARD")]
    public ColorBlock _liberalDiscard;
    [Header("LIB Keep")]
    public ColorBlock _liberalKeep;
}

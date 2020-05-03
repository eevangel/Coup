using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RoleCardLibrary", order = 1)]
public class RoleCardDefinition : ScriptableObject
{
    public List<Sprite> _liberals = new List<Sprite>();
    public List<Sprite> _fascists = new List<Sprite>();
    public Sprite _hitler;

    public Sprite GetRandomLib()
    {
        int rand = Random.Range(0, _liberals.Count);
        return _liberals[rand];
    }

    public Sprite GetRandomFas()
    {
        int rand = Random.Range(0, _fascists.Count);
        return _fascists[rand];
    }
}

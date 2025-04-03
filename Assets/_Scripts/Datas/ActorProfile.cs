using UnityEngine;
using CustomInspector;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/ActorProfile")]
public class ActorProfile : ScriptableObject
{
    [HorizontalLine("PROPERTIES"),HideField] public bool _h0;
    
    public string Alias;
    [Preview(Size.medium)] public Sprite portrait;
    [Preview(Size.medium)] public GameObject model;
    [Preview(Size.medium)] public Avatar avatar;

    [Space(10), HorizontalLine("ABILITIES"),HideField] public bool _h1;
    public List<AbilityData> abilities;
}

using System.Collections.Generic;
using UnityEngine;
using CustomInspector;



public enum ActorType { NONE = 0, PLAYER, NPC, ENEMY, BOSS, ITEM }


[CreateAssetMenu(menuName = "Datas/ActorProfile")]
public class ActorProfile : ScriptableObject
{

    [HorizontalLine("PROPERTIES"),HideField] public bool _h0;

    public ActorType type;
    public string alias;
    [Preview(Size.medium)] public Sprite portrait;
    [Preview(Size.medium)] public GameObject model;
    [Preview(Size.medium)] public Avatar avatar;



    [HorizontalLine("ABILITIES"),HideField] public bool _h1;
    public List<AbilityData> abilities;

}

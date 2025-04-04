using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using DungeonArchitect.Flow.Exec;



public enum ActorType { NONE = 0, PLAYER, NPC, ENEMY, BOSS, ITEM }


[CreateAssetMenu(menuName = "Datas/ActorProfile")]
public class ActorProfile : ScriptableObject
{

    [HorizontalLine("PREFABS"),HideField] public bool _h0;

    public ActorType type;
    public string alias;
    [Preview(Size.medium)] public Sprite portrait;
    [Preview(Size.medium)] public GameObject model;
    [Preview(Size.medium)] public Avatar avatar;

    [HorizontalLine("ATTRIBUTES"),HideField] public bool _h1;

    [Tooltip("체력")] public int health;
    [Tooltip("이동 속도(per sec)")] public float movespeed;
    [Tooltip("초당 회전 속도")]public float rotatespeed;
    [Tooltip("점프 파워")]public float jumpforce;
    [Tooltip("점프 체공 시간")]public float jumpduration;



    [HorizontalLine("ABILITIES"),HideField] public bool _h2;
    public List<AbilityData> abilities;

}

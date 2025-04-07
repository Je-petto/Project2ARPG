using CustomInspector;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [Space(20)]
    //외부에는 안 보이지만 public으로 보이게 할 수 있도록 protected 추가
    [SerializeField] protected Transform spawnpoint;    
    [SerializeField, Foldout] protected ActorProfile actorProfile;



}

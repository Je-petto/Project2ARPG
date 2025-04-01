using System.Collections;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    
    public EventPlayerSpawnBefore eventPlayerSpawnBefore;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        //플레이어 스폰 전 동작
        eventPlayerSpawnBefore?.Raise();
    }


}

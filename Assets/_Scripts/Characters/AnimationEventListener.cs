using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    private CharacterControl cc;

    public Transform Lfoot, Rfoot, Root;
    public PoolableParticle smoke1, smoke2;
    
    void Awake()
    {
        if (TryGetComponent(out cc) == false)
            Debug.LogWarning("AnimationEventListener ] CharacterControl 없음");
    }

    public void Footstep(string s)
    {
        if (cc.isArrived == true) return;

        PoolManager.I.Spawn(smoke1, s == "L" ? Lfoot.position : Rfoot.position, Quaternion.identity, null);

    }

    public void Footstop(string s)
    {
        PoolManager.I.Spawn(smoke1, s == "L" ? Lfoot.position : Rfoot.position, Quaternion.identity, null);
    }

    public void Jumpdown()
    {
        Vector3 offset = Vector3.up * 0.1f + Random.insideUnitSphere * 0.2f;
        PoolManager.I.Spawn(smoke2, Root.position + offset, Quaternion.identity, null);
    } 
}

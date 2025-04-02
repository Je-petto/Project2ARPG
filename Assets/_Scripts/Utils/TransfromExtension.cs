using UnityEngine;

public static class TransfromExtension
{
    public static Transform FindSlot(this Transform root, string slotname)
    {
        Transform[] children = root.GetComponentsInChildren<Transform>();
        
        foreach( Transform t in children )
            if(t.name.ToLower().Contains(slotname))
                return t;
        
        Debug.Log($"못 찾음 : {slotname}");
        return null;
    }
}

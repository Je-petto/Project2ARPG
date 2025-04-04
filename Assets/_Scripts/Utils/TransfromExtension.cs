using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransfromExtension
{
    public static Transform FindSlot(this Transform root, string slotname)
    {
        List<Transform> children = root.GetComponentsInChildren<Transform>().ToList();        

        foreach( Transform t in children )
            if(t.name.ToLower().Contains(slotname))
                return t;
        
        Debug.Log($"못 찾음 : {slotname}");
        return null;
    }
}

using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CursorSelectable : MonoBehaviour
{
    public CursorType cursorType;
    public List<Renderer> meshrenders = new List<Renderer>();

    [Tooltip("아웃라인 Material")]
    public Material selectableMaterial;
    [Tooltip("아웃라인 Material 두깨")]
    public float selectableThickness = 0.05f;


    public void SetupRenderer()
    {
        meshrenders.Clear();

        var skinnedmeshes = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        var meshes = GetComponentsInChildren<MeshRenderer>().ToList();

        meshrenders.AddRange(skinnedmeshes);
        meshrenders.AddRange(meshes);
    }
    
    public void Select(bool on)
    {
        if (meshrenders == null || meshrenders.Count <= 0) return;
        
        foreach( var r in meshrenders)
        {
            string layername = on ? "Outline" : "Default";
            r.gameObject.layer = LayerMask.NameToLayer(layername);

        }

        if (selectableMaterial != null)
            selectableMaterial?.SetFloat("_Thickness", selectableThickness);

    }

}

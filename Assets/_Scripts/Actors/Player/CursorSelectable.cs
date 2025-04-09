using UnityEngine;

public class CursorSelectable : MonoBehaviour
{
    public CursorType cursorType;
    public Renderer meshrender;

    [Tooltip("아웃라인 Material")]
    public Material selectableMaterial;
    [Tooltip("아웃라인 Material 두깨")]
    public float selectableThickness;

    public void SetupRenderer()
    {
        if (meshrender != null) return;

        meshrender = GetComponentInChildren<SkinnedMeshRenderer>();

        if(meshrender == null)
            meshrender = GetComponentInChildren<MeshRenderer>();
    }
    
    public void Select(bool on)
    {
        if (meshrender == null) return;
        
        string layername = on ? "Outline" : "Default";
        meshrender.gameObject.layer = LayerMask.NameToLayer(layername);

        if (selectableMaterial != null)
            selectableMaterial?.SetFloat("_Thickness", selectableThickness);

    }

}

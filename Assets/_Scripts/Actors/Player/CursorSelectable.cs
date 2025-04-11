using UnityEngine;

public class CursorSelectable : MonoBehaviour
{
    public CursorType cursorType;
    public Renderer[] meshrenders;

    [Tooltip("아웃라인 Material")]
    public Material selectableMaterial;
    [Tooltip("아웃라인 Material 두깨")]
    public float selectableThickness = 0.05f;


    public void SetupRenderer()
    {
        if (meshrenders.Length > 0) return;

        meshrenders = GetComponentsInChildren<SkinnedMeshRenderer>();

        if(meshrenders.Length <= 0)
            meshrenders = GetComponentsInChildren<MeshRenderer>();
    }
    
    public void Select(bool on)
    {
        if (meshrenders == null || meshrenders.Length <= 0) return;
        
        foreach( var r in meshrenders)
        {
            string layername = on ? "Outline" : "Default";
            r.gameObject.layer = LayerMask.NameToLayer(layername);

        }

        if (selectableMaterial != null)
            selectableMaterial?.SetFloat("_Thickness", selectableThickness);

    }

}

using CustomInspector;
using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{

    [ReadOnly] public Transform uiroot;
    [SerializeField] TextMeshPro textmesh;

    void Start()
    {
        uiroot = transform.Find("_UI_");
        if (uiroot == null)
            Debug.LogWarning("UIControl ] _UI_ 없음");
        
        Show(false);
    }

    public void Show(bool on)
    {
        if (uiroot == null) return;
            uiroot.localScale = Vector3.zero;

        uiroot.gameObject.SetActive(on);
    }
    
    public void Display(string info)
    {
        if(textmesh == null) return;

        textmesh.text = info;

    }
}

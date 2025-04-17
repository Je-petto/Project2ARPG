using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomInspector;

public class UiControl : MonoBehaviour
{
    
    [ReadOnly] public Transform uiroot;


//TEMPCODE
    [HorizontalLine("직접연결 (없으면 안씀)"),HideField] public bool _h0;
    [SerializeField] TextMeshPro textmesh;
    [SerializeField] Slider sliderHealth;
//TEMPCODE


    void Start()
    {
        uiroot = transform.Find("_UI_");
        if (uiroot == null)
            Debug.LogWarning("UiControl ] _UI_ 없음");

        Show(false);
    }


    public void Show(bool on)
    {
        if (uiroot == null) return;

        if (on == false)
            uiroot.localScale = Vector3.zero;

        uiroot.gameObject.SetActive(on);
    }

    public void Display(string info)
    {
        if (textmesh == null) return;

        textmesh.text = info;
    }

    public void SetHealth(int current , int max)
    {
        if (sliderHealth == null)
            return;

        float val = (float)current / max;
            
        sliderHealth.value = Mathf.Clamp01(val);
    }
}

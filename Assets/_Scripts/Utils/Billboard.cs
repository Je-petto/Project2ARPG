using System.Collections;
using UnityEngine;

public class Billboard : MonoBehaviour
{    
    private Transform maincam;
    [SerializeField] Transform offset;
    [SerializeField] float offsetForce = 1f;

    IEnumerator Start()
    {
        yield return new WaitUntil(()=> Camera.main != null);
        maincam = Camera.main.transform;        
    }
    
    void Update()
    {
        if (maincam == null) return;

        // A, B (B에서 A를 바라보기) => B - A
        Vector3 direction = (maincam.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-direction, maincam.up);
        
        if (offset != null)
            offset.position = transform.position - maincam.forward * offsetForce;
    }

}

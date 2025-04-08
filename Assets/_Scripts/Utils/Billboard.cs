using UnityEngine;

public class Billboard : MonoBehaviour
{

    private Transform maincam;
    void Start()
    {
        maincam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // A, B/ B에서 A를 바라보기 => B - A

        Vector3 direction = (maincam.position - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(-direction, maincam.up);

    }
}

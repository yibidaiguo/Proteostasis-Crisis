using UnityEngine;


public class Movetest : MonoBehaviour
{
    void Update()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }
}

using UnityEngine;
using System.Collections;

public class DragRotate : MonoBehaviour
{
    public float gain = 1f;
    Vector3 lastMousePos;

    // Use this for initialization
    void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.Rotate(Vector3.forward, delta.x * gain);
        }

        lastMousePos = Input.mousePosition;
    }
}

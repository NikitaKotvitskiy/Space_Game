using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject target;
    private float startXOffset;
    private float startZOffset;
    void Start()
    {
        startXOffset = transform.position.x;
        startZOffset = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + startXOffset, transform.position.y, target.transform.position.z + startZOffset);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;
    public float minAngVel;
    public float maxAngVel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float xAngVel = Random.Range(minAngVel, maxAngVel);
        float yAngVel = Random.Range(minAngVel, maxAngVel);
        float zAngVel = Random.Range(minAngVel, maxAngVel);
        rb.angularVelocity = new Vector3(xAngVel, yAngVel, zAngVel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AveronController : MonoBehaviour
{
    private Rigidbody shipRb;
    public float maxSpeed;
    public float engineForce;
    public float brakeForce;
    public float maxTorque;
    public float torqueForce;
    public float torqueBrakeForce;

    public KeyCode mainEngineKey = KeyCode.W;
    public KeyCode rotateRight = KeyCode.D;
    public KeyCode rotateLeft = KeyCode.A;

    public float inertiaCorrect;

    void Start()
    {
        shipRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MainEnginesControl();
        RotationEnginesControl();
        DirectionCorrect();
    }

    void MainEnginesControl()
    {
        Vector3 force;
        if (Input.GetKey(mainEngineKey))
            force = Vector3.forward * engineForce;
        else if (shipRb.velocity.magnitude != 0)
            force = -shipRb.velocity.normalized * brakeForce;
        else
            force = Vector3.zero;

        force = transform.TransformDirection(force);
        shipRb.AddForce(force, ForceMode.Force);

        if (shipRb.velocity.magnitude > maxSpeed)
            shipRb.velocity = shipRb.velocity.normalized * maxSpeed;
    }

    void RotationEnginesControl()
    {
        Vector3 force;
        if (Input.GetKey(rotateRight) && !Input.GetKey(rotateLeft))
            force = Vector3.up * torqueForce;
        else if (!Input.GetKey(rotateRight) && Input.GetKey(rotateLeft))
            force = Vector3.down * torqueForce;
        else if (shipRb.angularVelocity.magnitude != 0)
            force = -shipRb.angularVelocity.normalized * torqueBrakeForce;
        else
            force = Vector3.zero;

        shipRb.AddTorque(force, ForceMode.Force);
    }


    void DirectionCorrect()
    {
        Vector3 directionsDifference = transform.forward - shipRb.velocity;
        Vector3 newDirection = (shipRb.velocity + directionsDifference * inertiaCorrect).normalized;
        transform.LookAt(transform.position + newDirection);
    }
}

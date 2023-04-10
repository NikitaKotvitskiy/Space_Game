using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody shipRb;
    public GameObject shipBody;

    public float maxSpeed;
    public float accelerationTime;
    public float brakeCoef;
    private float enginesTimer;

    public float maxRotation;
    public float rotAccelTime;
    public float rotBrakeCoef;
    public float brakeRightTimer;
    public float brakeLeftTimer;

    public KeyCode mainEngineKey = KeyCode.W;
    public KeyCode rotateRight = KeyCode.D;
    public KeyCode rotateLeft = KeyCode.A;

    public Vector3 oldDirection;
    public float inertiaCoef;

    public float maxVisualRotation;

    public ParticleSystem leftMainEngine;   public Light lmeLight;
    public ParticleSystem middleMainEngine; public Light mmeLight;
    public ParticleSystem rightMainEngine;  public Light rmeLight;
    public ParticleSystem leftBackEngine;   public Light lbeLight;
    public ParticleSystem leftTopEngine;    public Light lteLight;
    public ParticleSystem rightBackEngine;  public Light rbeLight;
    public ParticleSystem rightTopEngine;   public Light rteLight;

    void Start()
    {
        shipRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float mainEngineSpeed = enginesController(mainEngineKey, maxSpeed, brakeCoef, accelerationTime, ref enginesTimer);
        float rotRightSpeed = enginesController(rotateRight, maxRotation, rotBrakeCoef, rotAccelTime, ref brakeRightTimer);
        float rotLeftSpeed = enginesController(rotateLeft, maxRotation, rotBrakeCoef, rotAccelTime, ref brakeLeftTimer);
        float rotSpeed = rotRightSpeed - rotLeftSpeed;

        shipRb.velocity = mainEngineSpeed * GetMovementDirection(mainEngineSpeed);
        shipRb.angularVelocity = rotSpeed * Vector3.up;
        shipBody.transform.localRotation = GetSlant(rotRightSpeed, rotLeftSpeed);

        ParticleController();
    }

    private float enginesController(KeyCode key, float maxSpeed, float brakeCoef, float accelerationTime, ref float enginesTimer)
    {
        if (Input.GetKey(key))
        {
            enginesTimer += Time.deltaTime;
            if (enginesTimer > accelerationTime)
                enginesTimer = accelerationTime;
        }
        else if (enginesTimer > 0)
            enginesTimer -= Time.deltaTime * brakeCoef;
        else
            enginesTimer = 0;
        return Mathf.Lerp(0, maxSpeed, enginesTimer / accelerationTime);
    }

    private Vector3 GetNormalizedDirection()
    {
        float yRot = transform.eulerAngles.y * Mathf.Deg2Rad;
        float zRot = Mathf.Sin(yRot);
        float xRot = -Mathf.Cos(yRot);
        return new Vector3(xRot, 0, zRot);
    }

    private Vector3 GetMovementDirection(float speed)
    {
        Vector3 newDirection = GetNormalizedDirection();
        Vector3 difference = newDirection - oldDirection * speed;
        oldDirection = (oldDirection * speed + difference * inertiaCoef).normalized;
        return oldDirection;
    }

    private Quaternion GetSlant(float rightRotSpeed, float leftRotSpeed)
    {
        float rightRot = Mathf.Lerp(0, maxVisualRotation, rightRotSpeed / maxRotation);
        float leftRot = Mathf.Lerp(0, maxVisualRotation, leftRotSpeed / maxRotation);
        float finalRot = rightRot - leftRot;
        if (finalRot < 0)
            return Quaternion.Euler(finalRot + 360, 0, 0);
        return Quaternion.Euler(finalRot, 0, 0);
    }

    private Quaternion GetPhysicRotation()
    {
        return Quaternion.Euler(270, transform.eulerAngles.y, 0);
    }

    private void ParticleController()
    {
        if (Input.GetKeyDown(mainEngineKey))
        {
            leftMainEngine.Play();      lmeLight.enabled = true;
            middleMainEngine.Play();    mmeLight.enabled = true;
            rightMainEngine.Play();     rmeLight.enabled = true;
        }
        if (Input.GetKeyUp(mainEngineKey))
        {
            leftMainEngine.Stop();      lmeLight.enabled = false;
            middleMainEngine.Stop();    mmeLight.enabled = false;
            rightMainEngine.Stop();     rmeLight.enabled = false;
        }

        if (Input.GetKeyDown(rotateRight))
        {
            leftTopEngine.Play();       lteLight.enabled = true;
            rightBackEngine.Play();     rbeLight.enabled = true;
        }
        if (Input.GetKeyUp(rotateRight))
        {
            leftTopEngine.Stop();       lteLight.enabled = false;
            rightBackEngine.Stop();     rbeLight.enabled = false;
        }

        if (Input.GetKeyDown(rotateLeft))
        {
            rightTopEngine.Play();      rteLight.enabled = true;
            leftBackEngine.Play();      lbeLight.enabled = true;
        }
        if (Input.GetKeyUp(rotateLeft))
        {
            rightTopEngine.Stop();      rteLight.enabled = false;
            leftBackEngine.Stop();      lbeLight.enabled = false;
        }
    }
}
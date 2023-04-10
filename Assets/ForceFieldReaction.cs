using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldReaction : MonoBehaviour
{
    public float fieldForce;
    public string physicObjectTag;
    public GameObject parts;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == physicObjectTag)
        {
            Vector3 collisionPos = other.ClosestPointOnBounds(transform.position);
            Instantiate(parts, collisionPos, transform.rotation, gameObject.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == physicObjectTag)
        {
            Vector3 collisionPos = other.ClosestPointOnBounds(transform.position);

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 objPos = other.transform.position;
            Vector3 objDir = (objPos - transform.position).normalized;
            rb.AddForceAtPosition(objDir * fieldForce, collisionPos, ForceMode.Impulse);
        }
    }
}

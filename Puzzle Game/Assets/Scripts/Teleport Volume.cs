using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportVolume : MonoBehaviour
{
    public float scaleFactor = 1.0f;
    public GameObject relativeDestination;
    private Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponents<Collider>();
    }

    private void OnTriggerEnter(Collider other) => ShouldTeleport(other);

    private void OnTriggerStay(Collider other) => ShouldTeleport(other);

    private void ShouldTeleport(Collider other)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        foreach (var collider in colliders)
        {
            if (collider.bounds.Contains(other.gameObject.transform.position))
            {
                var relativePosition = transform.InverseTransformPoint(other.transform.position);
                Quaternion relativeRotation = Quaternion.FromToRotation(transform.forward, relativeDestination.transform.forward);

                //var destScale = relativeDestination.transform.lossyScale;
                //var localScale = transform.lossyScale;
                //var scaleFactor = new Vector3(destScale.x / localScale.x, destScale.y / localScale.y, destScale.z / localScale.z);

                //var otherScale = other.transform.lossyScale;
                //var desiredLossy = new Vector3(otherScale.x * scaleFactor.x, otherScale.y * scaleFactor.y, otherScale.z * scaleFactor.z);
                var desiredLossy = scaleFactor * other.transform.lossyScale;

                other.transform.localScale = Vector3.one;
                var otherScale = other.transform.lossyScale;

                other.transform.localScale = new Vector3(otherScale.x * desiredLossy.x, otherScale.y * desiredLossy.y, otherScale.z * desiredLossy.z);

                var velocity = rigidbody.velocity * scaleFactor;
                var angularVelocity = rigidbody.angularVelocity;

                rigidbody.isKinematic = true;
                other.transform.position = relativeDestination.transform.TransformPoint(relativePosition);
                other.transform.rotation = relativeRotation * other.transform.rotation;
                rigidbody.isKinematic = false;
                rigidbody.velocity = relativeRotation * velocity;
                rigidbody.angularVelocity = relativeRotation * angularVelocity;
                
                return;
            }
        }
    }
}

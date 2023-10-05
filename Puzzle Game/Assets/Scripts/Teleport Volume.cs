using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportVolume : MonoBehaviour
{
    [Tooltip("used to scale duplicated objects to avoid use of lossyScale.")]
    public float globalScale = 1.0f;
    
    [Tooltip("The position the player will be teleported relative to based of their relative position to the center of the volume.")]
    public GameObject relativeDestination;

    [Tooltip("If the destination is inside a duplication volume then this field forces the teleported object to be duplicated this frame.")]
    public DuplicationVolume duplicationVolume;
    
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
        foreach (var collider in colliders)
        {
            if (collider.bounds.Contains(other.gameObject.transform.position))
            {
                Rigidbody rigidbody = other.GetComponent<Rigidbody>();

                var relativePosition = transform.InverseTransformPoint(other.transform.position);
                Quaternion relativeRotation = Quaternion.FromToRotation(transform.forward, relativeDestination.transform.forward);

                var desiredLossy = globalScale * other.transform.lossyScale;

                other.transform.localScale = Vector3.one;
                var otherScale = other.transform.lossyScale;

                other.transform.localScale = new Vector3(otherScale.x * desiredLossy.x, otherScale.y * desiredLossy.y, otherScale.z * desiredLossy.z);

                var velocity = rigidbody.velocity * globalScale;
                var angularVelocity = rigidbody.angularVelocity;

                rigidbody.isKinematic = true;
                other.transform.position = relativeDestination.transform.TransformPoint(relativePosition);
                other.transform.rotation = relativeRotation * other.transform.rotation;
                rigidbody.isKinematic = false;
                rigidbody.velocity = relativeRotation * velocity;
                rigidbody.angularVelocity = relativeRotation * angularVelocity;

                if (duplicationVolume) duplicationVolume.AddDuplicate(other.gameObject);
                //Debug.Break();
                return;
            }
        }
    }
}

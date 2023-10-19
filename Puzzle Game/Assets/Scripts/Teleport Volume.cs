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
        if (other.gameObject.layer == LayerMask.NameToLayer("Duplicates")) return;

        foreach (var collider in colliders)
        {
            if (collider.bounds.Contains(other.gameObject.transform.position))
            {
                CharacterController controller = other.GetComponent<CharacterController>();
                Player player = other.GetComponent<Player>();

                var relativePosition = transform.InverseTransformPoint(other.transform.position);
                //Quaternion relativeRotation = Quaternion.FromToRotation(transform.forward, relativeDestination.transform.forward);
                Quaternion relativeRotation = relativeDestination.transform.rotation * Quaternion.Inverse(transform.rotation);

                var desiredLossy = globalScale * other.transform.lossyScale;

                if (controller) controller.enabled = false;

                Vector3 velocity = new();
                Vector3 angularVelocity = new();
                if (other.attachedRigidbody)
                {
                    velocity = other.attachedRigidbody.velocity;
                    angularVelocity = other.attachedRigidbody.angularVelocity;
                }

                Transform heldObjectParent = null;
                if (player && player.heldObject)
				{
                    heldObjectParent = player.heldObject.transform.parent;
                    player.heldObject.transform.parent = player.transform;
				}

                other.transform.localScale = Vector3.one;
                var otherScale = other.transform.lossyScale;

                other.transform.localScale = new Vector3(otherScale.x * desiredLossy.x, otherScale.y * desiredLossy.y, otherScale.z * desiredLossy.z);
                other.transform.position = relativeDestination.transform.TransformPoint(relativePosition);
                other.transform.rotation = relativeRotation * other.transform.rotation;

                if (controller) controller.enabled = true;

                if (player && player.heldObject)
				{
                    player.heldObject.transform.parent = heldObjectParent;
                }

    //            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
    //            if (rigidbody)
				//{
    //                velocity = rigidbody.velocity * globalScale;
    //                angularVelocity = rigidbody.angularVelocity;

    //                rigidbody.isKinematic = true;
    //                rigidbody.velocity = relativeRotation * velocity;
    //                rigidbody.angularVelocity = relativeRotation * angularVelocity;
    //                rigidbody.isKinematic = false;
				//}

                if (other.attachedRigidbody)
                {
                    other.attachedRigidbody.velocity = relativeRotation * velocity;
                    other.attachedRigidbody.angularVelocity = relativeRotation * angularVelocity;
                }

                if (!controller && duplicationVolume) duplicationVolume.AddDuplicate(other.gameObject);
                //Debug.Break();

                return;
            }
        }
    }
}

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
                Quaternion relativeRotation = relativeDestination.transform.rotation * Quaternion.Inverse(transform.rotation);

                var desiredLossyScale = globalScale * other.transform.lossyScale;

                // Disable character control to prevent strange behavior
                if (controller) controller.enabled = false;

                // Save velocities if rigidbody is attached.
                Vector3 velocity = new();
                Vector3 angularVelocity = new();
                Rigidbody rigidbody = other.GetComponent<Rigidbody>();
                if (rigidbody)
                {
                    velocity = rigidbody.velocity;
                    angularVelocity = rigidbody.angularVelocity;
                }

                // Set held objects parent to player so that it teleports with the player
                Transform heldObjectParent = null;
                if (player && player.heldObject)
				{
                    heldObjectParent = player.heldObject.transform.parent;
                    player.heldObject.transform.parent = player.transform;
				}

                // Teleport object
                other.transform.localScale = Vector3.one;
                var otherScale = other.transform.lossyScale;

                other.transform.localScale = new Vector3(otherScale.x * desiredLossyScale.x, otherScale.y * desiredLossyScale.y, otherScale.z * desiredLossyScale.z);
                other.transform.position = relativeDestination.transform.TransformPoint(relativePosition);
                other.transform.rotation = relativeRotation * other.transform.rotation;

                // Enable character control
                if (controller) controller.enabled = true;

                // Reset parent for held object
                if (player && player.heldObject) player.heldObject.transform.parent = heldObjectParent;

                // Apply rotation to rigidbody's velocity
                if (rigidbody)
                {
                    rigidbody.isKinematic = true;
                    rigidbody.velocity = relativeRotation * (velocity * globalScale);
                    rigidbody.angularVelocity = relativeRotation * (angularVelocity * globalScale);
                    rigidbody.isKinematic = false;
                }

                // If destination has a duplication volume create a duplicate this frame
                if (!controller && duplicationVolume) duplicationVolume.AddDuplicate(other.gameObject);
                
                return;
            }
        }
    }
}

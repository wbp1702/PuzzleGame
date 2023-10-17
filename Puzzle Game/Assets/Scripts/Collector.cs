using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Manager.ActivateColor(System.Enum.Parse<NamedColor>(other.tag));
	}

	private void OnTriggerStay(Collider other)
	{
		// Make sure orb does not bounce out of collector
		other.attachedRigidbody.velocity /= 2f;
		other.attachedRigidbody.angularVelocity /= 2f;
	}

	private void OnTriggerExit(Collider other)
	{
		Manager.DeactivateColor(System.Enum.Parse<NamedColor>(other.tag));
	}
}

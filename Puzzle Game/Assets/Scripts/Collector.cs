using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
	public NamedColor usableColorMask;

	private void OnTriggerEnter(Collider other)
	{
		NamedColor color = System.Enum.Parse<NamedColor>(other.tag);
		if ((color & usableColorMask) != NamedColor.None) Manager.ActivateColor(color);
	}

	private void OnTriggerStay(Collider other)
	{
		// Make sure orb does not bounce out of collector
		other.attachedRigidbody.velocity /= 2f;
		other.attachedRigidbody.angularVelocity /= 2f;
	}

	private void OnTriggerExit(Collider other)
	{
		NamedColor color = System.Enum.Parse<NamedColor>(other.tag);
		if ((color & usableColorMask) != NamedColor.None) Manager.DeactivateColor(color);
	}
}

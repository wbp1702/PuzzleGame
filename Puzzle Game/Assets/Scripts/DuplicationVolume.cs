using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicationVolume : MonoBehaviour
{
	public DuplicationVolume twin;
	private Dictionary<GameObject, GameObject> duplicates = new();

	void Update()
    {
		List<GameObject> toRemove = new();

		foreach (var pair in duplicates)
		{
			GameObject original = pair.Key;
			GameObject duplicate = pair.Value;

			if (!original.activeInHierarchy || original.layer != LayerMask.NameToLayer("Duplicatable"))
			{
				toRemove.Add(original);
				continue;
			}

			var relativePosition = transform.InverseTransformPoint(original.transform.position);
			var relativeRotation = Quaternion.FromToRotation(transform.forward, twin.transform.forward);

			duplicate.transform.position = twin.transform.TransformPoint(relativePosition);
			duplicate.transform.rotation = relativeRotation * original.transform.rotation;

			//duplicate.transform.localScale = Vector3.one;
			//duplicate.transform.localScale = new Vector3(original.transform.lossyScale.x / duplicate.transform.lossyScale.x,
			//											 original.transform.lossyScale.y / duplicate.transform.lossyScale.y,
			//											 original.transform.lossyScale.z / duplicate.transform.lossyScale.z);
		}

		foreach (var gameObject in toRemove) if (duplicates.Remove(gameObject, out GameObject duplicate)) Destroy(duplicate);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Duplicatable")) return;

		var relativePosition = transform.InverseTransformPoint(other.transform.position);
		var relativeRotation = Quaternion.FromToRotation(transform.forward, twin.transform.forward);

		var duplicate = Instantiate(other.gameObject, twin.transform.TransformPoint(relativePosition), relativeRotation * twin.transform.rotation);
		duplicate.layer = LayerMask.NameToLayer("TransparentFX");
		
		duplicates.Add(other.gameObject, duplicate);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Duplicatable")) return;

		if (duplicates.Remove(other.gameObject, out GameObject duplicate)) Destroy(duplicate);
	}
}

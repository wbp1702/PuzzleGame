using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class DuplicationVolume : MonoBehaviour
{
	[Tooltip("The global scale of the volume, used to scale duplicated objects to avoid use of lossyScale.")]
	public float globalScale = 1.0f;

	[Tooltip("The duplication volume that this volume will duplicate to and vise versa.")]
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

			duplicate.transform.localScale = Vector3.one;
			duplicate.transform.localScale = new Vector3(original.transform.lossyScale.x / duplicate.transform.lossyScale.x * globalScale / twin.globalScale,
														 original.transform.lossyScale.y / duplicate.transform.lossyScale.y * globalScale / twin.globalScale,
														 original.transform.lossyScale.z / duplicate.transform.lossyScale.z * globalScale / twin.globalScale);
		}

        foreach (var gameObject in toRemove) if (duplicates.Remove(gameObject, out GameObject duplicate)) Destroy(duplicate);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Duplicatable") && !duplicates.ContainsKey(other.gameObject)) AddDuplicate(other.gameObject);
    }

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Duplicatable")) return;

		if (duplicates.Remove(other.gameObject, out GameObject duplicate)) Destroy(duplicate);
	}

	public void AddDuplicate(GameObject other)
	{
        var relativePosition = transform.InverseTransformPoint(other.transform.position);
        var relativeRotation = Quaternion.FromToRotation(transform.forward, twin.transform.forward);

        var duplicate = Instantiate(other.gameObject);
        duplicate.layer = LayerMask.NameToLayer("TransparentFX");
        duplicate.transform.position = twin.transform.TransformPoint(relativePosition);
        duplicate.transform.rotation = relativeRotation * other.transform.rotation;

        duplicate.transform.localScale = Vector3.one;
        duplicate.transform.localScale = new Vector3(other.transform.lossyScale.x / duplicate.transform.lossyScale.x * globalScale / twin.globalScale,
                                                     other.transform.lossyScale.y / duplicate.transform.lossyScale.y * globalScale / twin.globalScale,
                                                     other.transform.lossyScale.z / duplicate.transform.lossyScale.z * globalScale / twin.globalScale);

        duplicates.Add(other.gameObject, duplicate);
    }
}

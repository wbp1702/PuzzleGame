using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [System.Serializable]
    public class SubPart
	{
        public GameObject gameObject;
        public Vector3 openPosition;
        public Vector3 closedPosition;
        public float movementSpeed;
	}

    public SubPart[] subParts;
    public bool open;

    // Update is called once per frame
    void Update()
    {
		foreach (var part in subParts)
		{
            if (open) {
                Vector3 diff = part.openPosition - part.gameObject.transform.localPosition;
                part.gameObject.transform.localPosition += Vector3.ClampMagnitude(diff.normalized * part.movementSpeed, diff.magnitude);
            }
            else {
                Vector3 diff = part.closedPosition - part.gameObject.transform.localPosition;
                part.gameObject.transform.localPosition += Vector3.ClampMagnitude(diff.normalized * part.movementSpeed, diff.magnitude);
            }
        }
    }
}

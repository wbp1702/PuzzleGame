using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncVolume : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The volume to synchrnoize with.")]
    private SyncVolume target;

    [SerializeField]
    [Tooltip("The global scale factor of the volume, used to scale duplicated objects to avoid use of lossyScale.")]
    private float scaleFactor = 1.0f;

    private Dictionary<GameObject, GameObject> duplicates = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

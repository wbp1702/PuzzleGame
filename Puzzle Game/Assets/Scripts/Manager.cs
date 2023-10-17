using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Manager : MonoBehaviour
{
    [System.Serializable]
    public class ColorPair
	{
        public NamedColor color;
        public UnityEvent<bool> events;
	}

    [SerializeField]
    private ColorPair[] colorPairs;
    private NamedColor colorMask;
    private static Manager manager;

    void Start()
    {
        if (manager)
        {
            Debug.LogError("More than one manager");
            Destroy(this);
        }
        else manager = this;
    }

    public static void ActivateColor(NamedColor color)
	{
        if (color != NamedColor.None)
		{
            manager.colorMask |= color;
            foreach (var pair in manager.colorPairs) pair.events.Invoke((pair.color & manager.colorMask) == pair.color);
		}
	}

    public static void DeactivateColor(NamedColor color)
	{
        if (color != NamedColor.None)
		{
            manager.colorMask ^= color;
            foreach (var pair in manager.colorPairs) pair.events.Invoke((pair.color & manager.colorMask) == pair.color);
		}
    }
}

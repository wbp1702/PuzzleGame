using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [System.Serializable]
    public class ColorPair
	{
        public NamedColor includeColors;
        public NamedColor excludeColors;
        public UnityEvent activationEvents;
        public UnityEvent deactivationEvents;

    }

    [SerializeField]
    private ColorPair[] colorPairs;
    [SerializeField]
    private NamedColor colorMask;

    private static Manager manager;
    private Stopwatch stopwatch;

    void Start()
    {
        if (manager)
        {
            Debug.LogError("More than one manager");
            Destroy(this);
        }
        else manager = this;
        stopwatch = GetComponent<Stopwatch>();
    }

    public static void ActivateColor(NamedColor color)
	{
        if (color != NamedColor.None)
		{
            manager.colorMask |= color;
            foreach (var pair in manager.colorPairs)
			{
                // Used because the Unity Editor sets all bits if Everything is selected which causes checks to fail.
                // Ex: include = (0b11111111 & 0b11100000) == (0b11111111); returns false, actual includeColors should be 0b11100000.
                if (((ulong)pair.includeColors & (ulong)NamedColor.Everything) == (ulong)NamedColor.Everything) pair.includeColors = NamedColor.Everything;

                bool included = ((ulong)pair.includeColors & (ulong)manager.colorMask) == (ulong)pair.includeColors;
                bool excluded = ((ulong)pair.excludeColors & (ulong)manager.colorMask) != (ulong)NamedColor.None;
                if (included && !excluded) pair.activationEvents.Invoke();
                else pair.deactivationEvents.Invoke();
            }
		}
	}

    public static void DeactivateColor(NamedColor color)
	{
        if (color != NamedColor.None)
		{
            manager.colorMask ^= color;
            foreach (var pair in manager.colorPairs)
			{
                bool included = (pair.includeColors & manager.colorMask) == pair.includeColors;
                bool excluded = (pair.excludeColors & manager.colorMask) != NamedColor.None;
                if (included && !excluded) pair.activationEvents.Invoke();
                else pair.deactivationEvents.Invoke();
            }
		}
    }

    public static void LoadMainMenu(bool levelCompleted)
	{
        if (levelCompleted)
		{
            var name = SceneManager.GetActiveScene().name + "HighScore";
            var fastestTime = PlayerPrefs.GetFloat(name, float.PositiveInfinity);
            if (manager.stopwatch.CurrentTime() < fastestTime) PlayerPrefs.SetFloat(name, manager.stopwatch.CurrentTime());
        }

        SceneManager.LoadScene("MainMenu");
	}
}

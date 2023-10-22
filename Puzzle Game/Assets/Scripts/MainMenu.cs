using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelPrefab;
    public Transform levelSelectContent;

    public int levelCount;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

		for (int i = 1; i <= levelCount; i++)
		{
            var gameObject = Instantiate(levelPrefab, levelSelectContent);
            var bestTimeText = gameObject.transform.Find("Best Time");
            Text text = bestTimeText.GetComponent<Text>();

            float bestTime = PlayerPrefs.GetFloat("Level " + i + "HighScore", float.PositiveInfinity);
            if (bestTime == float.PositiveInfinity) bestTimeText.gameObject.SetActive(false);
            else text.text = string.Format("BestTime: {0:00}:{1:00}", (int)(bestTime / 60), (int)(bestTime % 60));

            var loadButton = gameObject.transform.Find("Load Button");
            Button button = loadButton.GetComponent<Button>();
            var index = i;
            button.onClick.AddListener(() => { SceneManager.LoadScene("Level " + index); Time.timeScale = 1f; });

            var loadText = loadButton.GetChild(0);
            loadText.GetComponent<Text>().text = "Load Level " + i;
        }
    }

    public void ExitGame()
	{
        Application.Quit();
	}
}

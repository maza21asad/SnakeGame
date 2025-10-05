//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameOverWindow : MonoBehaviour
//{
//    private static GameOverWindow instance;
//    private void Awake()
//    {
//        instance = this;

//        transform.Find("restartButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
//        {
//            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
//        });

//        Hide();
//    }

//    private void Show()
//    {
//        gameObject.SetActive(true);
//    }

//    private void Hide()
//    {
//        gameObject.SetActive(false);
//    }

//    public static void ShowStatic()
//    {
//        instance.Show();
//    }
//}


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        // Make sure the panel is hidden at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Hook up button events
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(RestartGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
}


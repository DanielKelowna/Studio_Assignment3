using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 Instance { get; private set; }
    private int score = 0;

    [SerializeField] private TextMeshProUGUI scoreText; // Drag a UI Text to this in the Inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementScore()
    {
        score += 1;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        Debug.Log("Score: " + score);
    }
}

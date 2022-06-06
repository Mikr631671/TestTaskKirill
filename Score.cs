using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [Header("Main Component")]
    private TextMeshProUGUI scoreText;

    [Header("Variables")]
    private float score = 0;


    private void Awake()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        SetScore(0);
    }

    public void SetScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
}



using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.Score;
    }
}

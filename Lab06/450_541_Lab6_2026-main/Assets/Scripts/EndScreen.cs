using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text FinalScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FinalScoreText.text = "Final Score: " + ScoreManager.Instance.Score;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEbdTrigger : MonoBehaviour
{
    [SerializeField] private string endSceneName = "EndScene";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
            ScoreManager.Instance.SaveFinalScore();
            SceneManager.LoadScene(endSceneName);
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

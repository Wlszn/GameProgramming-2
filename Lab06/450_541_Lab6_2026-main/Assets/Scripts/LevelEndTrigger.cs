using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private string endSceneName = "EndScene";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
            Debug.Log("Level completed!");
            ScoreManager.Instance.SaveFinalScore();
            SceneManager.LoadScene(endSceneName);
        
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 2f;
    public GameObject crystalPrefab;
    public GameObject robotPrefab;

    public TMP_Text scoreText;
    private Rigidbody rb;
    private int score = 0; 
    private float horizontalInput; 
    private float verticalInput;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(SpawnCrystal),2.0f,2.0f); 
        InvokeRepeating(nameof(SpawnRobot),2.0f,3.0f); 
    }
    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
    }

    public void SpawnCrystal()
    {
     
        Vector3 spawnPosition = transform.position + 
        new Vector3(Random.Range(-7,7), 3.0f, 0.0f);
        Instantiate(crystalPrefab,spawnPosition,Quaternion.identity);

    }

    public void SpawnRobot()
    {
     
        Vector3 spawnPosition = transform.position + 
        new Vector3(Random.Range(-7,7), 5.0f, 0.0f);
        Instantiate(robotPrefab,spawnPosition,Quaternion.identity);
    }


 void FixedUpdate()
 {

    rb.linearVelocity = new Vector3(horizontalInput *
    movementSpeed,rb.linearVelocity.y,verticalInput*movementSpeed);

 }

 void OnCollisionEnter(Collision other)
 {
    if (other.gameObject.tag == "Collectible")
        {
            score += 50;
            scoreText.text = "Score : "+ score;
            Destroy(other.gameObject);

        } else if(other.gameObject.tag == "Enemy")
        {
            score -= 100;
            if (score < 0)
            {
                RestartScene();
            }
            scoreText.text = "Score : "+ score;
            Destroy(other.gameObject);
        }
}

 void RestartScene()
 {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
 }

}

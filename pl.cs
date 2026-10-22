using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerComponent : MonoBehaviour
{
 public float movementSpeed = 2f; // Multiplier for the movement speed
 public float jumpStrength = 2f; // Multiplier for the jump strenght.
 public GameObject starPrefab; // Use to spawn the star collectibles

 public TMP_Text scoreText; // Used to read and write Text UI.
 private int score = 0; // Keeps the value of the current score
 private float horizontalInput; // Keeps the last horizontal Input entered

 private float verticalInput; // Keeps the last vertical input entered

 private Vector2 moveInput; // New Input system , store the value of both
vertical and horizontal input
 private Rigidbody rb; // Use to spawn the star collectibles
 private bool jumpRequested = false; //
 // Start is called once before the first execution of Update after the
MonoBehaviour is created
 void Start()
 {
 rb = GetComponent<Rigidbody>();// Assigning the rigidBody
 InvokeRepeating(nameof(SpawnStars),3.0f,3.0f); // Calling the function
Spawn Stars Every 3s after an initial 3s delay
 }
 // New input system method
 void OnMove(InputValue value)
 {
 moveInput = value.Get<Vector2>();// Reading the input with the new Input
system.
 //
 horizontalInput = moveInput.x;
 verticalInput = moveInput.y;
 }
 // Update is called once per frame
 void Update()
 {
 // Reading the input with the Old input system
 horizontalInput = Input.GetAxis("Horizontal");
 verticalInput = Input.GetAxis("Vertical");
 // Reading the Jump input
 if (Input.GetButtonDown("Jump"))
 {
 jumpRequested = true;
 }
 // Restarting the scene if you fall of the floor. When the height of the
player is lower than -10.
 if (transform.position.y < -10)
 {
 RestartScene();
 }
 }
 // Spawn the Stars prefab at a random position in X of -10 to 10 units around
the player position
 void SpawnStars()
 {
 // We find a random spawn position at a Range of between -10 and 10 of
the position of the player in X with a height in Y of 5 and a depth of 0.0f in Z
 Vector3 spawnPosition = transform.position + new
Vector3(Random.Range(-10,10), 5.0f, 0.0f);
 Instantiate(starPrefab,spawnPosition,Quaternion.identity);
 }
 // Move the player with physics
 void FixedUpdate()
 {
 // Assign the linear velocity in X so that the horizontal velocity moves
the player in X.
 rb.linearVelocity = new Vector3(horizontalInput
*movementSpeed,rb.linearVelocity.y,rb.linearVelocity.z);

 //if we wanted to move the player in X and Z this would be the statement
to use.Rather than the previous line.
 //rb.linearVelocity = new Vector3(horizontalInput
*movementSpeed,rb.linearVelocity.y,verticalInput*movementSpeed);
 // If the jump is requested we add an inpulse up to make the player jump
up
 if (jumpRequested)
 {
 rb.AddForce(Vector3.up *jumpStrength, ForceMode.Impulse );
 jumpRequested = false;
 }
 }
 // Reading Collision information to maniuplate the score.
 void OnCollisionEnter(Collision other)
 {
 // If the other object we collide with is a Collectible
 if (other.gameObject.tag == "Collectible")
 {
 // We increment the score by 100
 score += 100;
 // If the score is > 1000 we Restart the scene
 if (score >= 1000)
 {
 RestartScene();
 }
 // We then Change the Score UI
 scoreText.text = "Score : "+ score;
 // And destroy the game object we collided with.
 Destroy(other.gameObject);
 }
 }
 // Method to restart the scene.
 void RestartScene()
 {
 // We get the active scene to get the scene currently loaded and restart
it.
 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
 }
}
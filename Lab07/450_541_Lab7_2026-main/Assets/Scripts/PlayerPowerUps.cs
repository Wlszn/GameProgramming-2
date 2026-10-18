using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{

    // The speed multiplier for the player's boost power-up
    [SerializeField] private float boostMultiplier = 1.5f;

    // The duration of the boost power-up in seconds
    public float SpeedTimeLeft { get; private set; }

    private PlayerMovement movement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        
    }

    //Called by pickup when the player collects a speed boost power-up
    public void ActivateSpeedBoost(float duration)
    {
        SpeedTimeLeft = duration;
        movement.SpeedMultiplier = boostMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if(SpeedTimeLeft <= 0f) return; //no boost active

        SpeedTimeLeft -= Time.deltaTime;

        if(SpeedTimeLeft <= 0f)
        {
            movement.SpeedMultiplier = 1f; //reset speed multiplier to normal
        }        
    }
}

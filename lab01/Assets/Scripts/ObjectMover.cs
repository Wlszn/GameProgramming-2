using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    public float timer = 0f;
    public float directionChangeDelay = 2f;
    public Vector3 movement;

    void Start()
    {
     
    }

    void Update(){
        timer += Time.deltaTime;
        if (timer >= directionChangeDelay)
        {
            movement *= -1f;
            timer = 0f;
        }
        transform.position += movement * Time.deltaTime;
    }
    
}

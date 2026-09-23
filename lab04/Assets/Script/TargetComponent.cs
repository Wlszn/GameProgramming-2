using UnityEngine;

public class TargetComponent : MonoBehaviour
{

    private Renderer targetRenderer;
    private Color originalColor;
    public Color hitColor = Color.green;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
        }
    }

    private void onCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GameManager.Instance.IncrementScore();

            if (targetRenderer != null)
            {
                targetRenderer.material.color = hitColor;
            }

            Invoke("ResetColor", 5f);
        }
    }

    private void ResetColor()
    {
        if (targetRenderer != null)
        {
            targetRenderer.material.color = originalColor;
        }
    }
}

using UnityEngine;

public class GunComponent : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletMaxImpulse = 100.0f;
    public float maxChargeTime = 3.0f;
    private float chargeTime = 0.0f;
    private bool isCharging = false;

    void Update()
{
    if (Input.GetButtonDown("Fire1"))
    {
        isCharging = true;
        chargeTime = 0.0f;
    }

    // While the button is held charge goes up
    if (Input.GetButton("Fire1"))
    {
        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Clamp(chargeTime, 0.0f, maxChargeTime);
    }

    // When button is released fire the shot
    if (Input.GetButtonUp("Fire1"))
    {
        ShootBullet();
        isCharging = false;
    }
}

    void ShootBullet()
{
    GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    Rigidbody rb = bullet.GetComponent<Rigidbody>();

    // Calculate the charge ratio and apply force to the bullet
    float chargeRatio = chargeTime / maxChargeTime;       // 0.0 to 1.0
    float bulletImpulse = bulletMaxImpulse * chargeRatio; 

    rb.AddForce(bulletSpawnPoint.forward * bulletImpulse, ForceMode.Impulse);

}
}

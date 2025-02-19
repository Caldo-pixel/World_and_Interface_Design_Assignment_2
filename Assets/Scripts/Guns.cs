using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum WeaponType
{
    Automatic,
    Burst,
    COUNT
}
public abstract class Weapon
{
    public abstract void Shoot(Vector3 direction, float speed);
    public GameObject weaponPrefab;
    public GameObject shooter;
}
public class Automatic : Weapon
{
    public override void Shoot(Vector3 direction, float speed)
    {
        GameObject bullet = GameObject.Instantiate(weaponPrefab);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.position = shooter.transform.position + direction * 0.75f;
        bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        bullet.GetComponent<SpriteRenderer>().color = Color.red;
        GameObject.Destroy(bullet, 1.0f);
    }
}
public class Burst : Weapon
{
    public override void Shoot(Vector3 direction, float speed)
    {
        GameObject bullet = GameObject.Instantiate(weaponPrefab);
        GameObject bulletLeft = GameObject.Instantiate(weaponPrefab);
        GameObject bulletRight = GameObject.Instantiate(weaponPrefab);
        Vector3 directionLeft = Quaternion.Euler(0.0f, 0.0f, 30.0f) * direction;
        Vector3 directionRight = Quaternion.Euler(0.0f, 0.0f, -30.0f) * direction;
        bullet.transform.position = shooter.transform.position + direction * 0.75f;
        bulletLeft.transform.position = shooter.transform.position + directionLeft * 0.75f;
        bulletRight.transform.position = shooter.transform.position + directionRight * 0.75f;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        bulletLeft.GetComponent<Rigidbody2D>().velocity = directionLeft * speed;
        bulletRight.GetComponent<Rigidbody2D>().velocity = directionRight * speed;
        bullet.GetComponent<SpriteRenderer>().color = Color.green;
        bulletLeft.GetComponent<SpriteRenderer>().color = Color.green;
        bulletRight.GetComponent<SpriteRenderer>().color = Color.green;
        GameObject.Destroy(bullet, 1.0f);
        GameObject.Destroy(bulletLeft, 1.0f);
        GameObject.Destroy(bulletRight, 1.0f);
    }
}

public class Guns : MonoBehaviour
{

    [SerializeField]
    GameObject bulletPrefab;

    float bulletSpeed = 10.0f;
    float moveSpeed = 5.0f;
    float enemyShootDelay = 0.1f;
    float enemyBulletSpeed = 5.0f;

    bool isShooting;

    WeaponType weaponType = WeaponType.Automatic;

    Weapon Automatic = new Automatic();
    Weapon Burst = new Burst();

    void Start()
    {
        Automatic.weaponPrefab = bulletPrefab;
        Automatic.shooter = gameObject;

        Burst.weaponPrefab = bulletPrefab;
        Burst.shooter = gameObject;

        isShooting = false;
    }

    void Update()

    {
        

        // Aiming with mouse 
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0.0f;
        Vector3 mouseDirection = (mouse - transform.position).normalized;
        Debug.DrawLine(transform.position, transform.position + mouseDirection * 5.0f);

        if (Input.GetKeyDown(KeyCode.Mouse0) && gameObject.tag == "Player")
        {
                switch (weaponType)
                {
                    case WeaponType.Automatic:

                        Automatic.Shoot(mouseDirection, bulletSpeed);
                        break;

                    case WeaponType.Burst:
                        Burst.Shoot(mouseDirection, bulletSpeed);
                        break;
                }
        }

        // Cycle weapon with left-shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            int weaponNumber = (int)++weaponType;
            weaponNumber %= (int)WeaponType.COUNT;
            weaponType = (WeaponType)weaponNumber;
            Debug.Log("Selected weapon: " + weaponType);
        }

        if (isShooting)
        {
            return;
        }

        if (gameObject.tag == "Enemy")
        {
            StartCoroutine(EnemyShoot());
        }

    }

    public IEnumerator EnemyShoot()
    {
        isShooting = true;
        Automatic.Shoot(Vector2.down, enemyBulletSpeed);
        yield return new WaitForSeconds(enemyShootDelay);
        isShooting = false;
    }

}


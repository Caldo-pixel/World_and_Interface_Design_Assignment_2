using System.Collections;
using UnityEngine;

public class WorldHazards : MonoBehaviour
{

    public GameObject bullet;

    Rigidbody2D rb;

    Vector3 spawnPosition1;

    float speed = 0.75f;
    float waitTime = 0.5f;

    bool isCooling;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPosition1 = new Vector3(3.0f, 3.0f, 0f);
        rb = bullet.GetComponent<Rigidbody2D>();
        isCooling = false;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.down * speed;

        if (isCooling)
        {
            return;
        }

        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        isCooling = true;
        Instantiate(bullet, spawnPosition1, Quaternion.identity);
        
        yield return new WaitForSeconds(waitTime);
        isCooling = false;
    }

}

using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    SpriteRenderer sr;

    float speed = 100.0f;
    //float dodgeForce = 100.0f;
    float dodgeTimer = 0.5f;

    bool isDodging;

    Vector2 movement;
    Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        sr.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isDodging)
        {
            return;
        }

        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }

        direction = direction.normalized;
        movement = direction * speed * Time.deltaTime;

        rb.velocity = movement;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dodge());
        }
    }

    public IEnumerator Dodge()
    {
        isDodging = true;
        sr.color = Color.green;
        rb.velocity *= 2.0f;
        yield return new WaitForSeconds(dodgeTimer);
        isDodging = false;
        sr.color = Color.white;
    }

}

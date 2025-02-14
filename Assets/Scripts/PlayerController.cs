using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    SpriteRenderer sr;

    float speed = 200.0f;
    //float dodgeForce = 100.0f;
    //float dodgeTimer1 = 0.5f;
    float dodgeTimer = 0.25f;

    bool isDodging;

    Vector2 movement;
    Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        sr.color = Color.white;

        isDodging = false;
    }

    // Update is called once per frame
    void Update()
    {
        DodgeHandle();
    }

    private void FixedUpdate()
    {
        if (isDodging)
        {
            return;
        }

        MovementHandle();
    }

    void MovementHandle()
    {
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
    }

    void DodgeHandle()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isDodging)
        {
            StartCoroutine(Dodge());
            //Debug.Log("test 1");
        }
    }

    public IEnumerator Dodge()
    {
        isDodging = true;
        sr.color = Color.green;
        sr.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
        rb.velocity *= 2.0f;
        yield return new WaitForSeconds(dodgeTimer);
        rb.velocity *= 0.25f;
        yield return new WaitForSeconds(dodgeTimer);
        isDodging = false;
        sr.color = Color.white;
        sr.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isDodging)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isDodging)
        {
            Destroy(gameObject);
        }
    }

}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;


public class PL_Move : MonoBehaviour
{
    public float speed;
    public float jump;
    public GameObject rayOrigin;
    public float rayCheckDistance;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Jump") > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.transform.position, Vector2.down, rayCheckDistance);
            if (hit.collider != null)
            {
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            }
        }
        rb.linearVelocity = new Vector3(x * speed, rb.linearVelocity.y, 0);
        
    }
}

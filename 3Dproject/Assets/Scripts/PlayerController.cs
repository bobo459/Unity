using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float inputThreshold = 0.1f;

    Rigidbody rb;
    Vector3 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical);

        if(moveDirection.sqrMagnitude>= inputThreshold * inputThreshold)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                 targetRotation,
                                 rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
    private void FixedUpdate()
    {
        if(moveDirection.sqrMagnitude >= inputThreshold * inputThreshold)
        {
            Vector3 move = moveDirection.normalized * moveSpeed;
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

}

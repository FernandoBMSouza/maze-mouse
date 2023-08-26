using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public float moveSpeed = 20f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    public int Points { get; set; }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }

        if (isMoving)
        {
            MoveToTarget();
        }
    }

    private void SetTargetPosition()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;
        isMoving = true;

        // Rotate the character towards the target position
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(targetPosition.x < this.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
        else{
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }
    }

    private void MoveToTarget()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMoving && collision.gameObject.CompareTag("Wall"))
        {
            isMoving = false;
        }

        if(collision.gameObject.CompareTag("Cheese"))
        {
            Points++;
            Destroy(collision.gameObject);
        }
    }
}

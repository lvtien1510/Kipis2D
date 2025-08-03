using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform postA, postB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 1f;

    private Vector2 targetPos;
    private bool isWaiting = false;

    private void Start()
    {
        targetPos = postB.position;
    }

    private void FixedUpdate()
    {
        if (isWaiting) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            StartCoroutine(WaitBeforeMoving());
            targetPos = targetPos == (Vector2)postA.position ? postB.position : postA.position;
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

    // Gán Player làm con khi đứng lên platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        if (postA != null && postB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(postA.position, postB.position);
        }
    }
}

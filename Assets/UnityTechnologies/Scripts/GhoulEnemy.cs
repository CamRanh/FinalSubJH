using System.Collections;
using UnityEngine;

public class GhoulEnemy : MonoBehaviour
{
    public GameEnding gameEnding;
    public float dashDistance = 3f;
    public float dashCooldown = 2f;
    public float attackCooldown = 1f;
    public float attackDuration = 0.5f;

    private bool isAttacking = false;

    void Start()
    {
        StartCoroutine(PhantomDash());
    }

    IEnumerator PhantomDash()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(dashCooldown, dashCooldown + 1f));

            if (!isAttacking)  
            {
                DashAndAttack();
            }
            else
            {
                
            }

            yield return new WaitForSeconds(attackCooldown);
        }
    }

    void DashAndAttack()
    {
        isAttacking = true;
        StartCoroutine(Dash()); 
    }

    IEnumerator Dash()
    {
        Vector3 targetPosition = GetRandomDashPosition();

        
        if (!IsObstacleInPath(transform.position, targetPosition))
        {
            float startTime = Time.time;
            Vector3 startPosition = transform.position;

            while (Time.time - startTime < attackDuration)
            {
                float percentage = (Time.time - startTime) / attackDuration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, percentage);
                yield return null;
            }

            

            isAttacking = false;
        }
        else
        {
            
            Debug.Log("Obstacle detected. Choosing a different action.");
            
        }
    }

    bool IsObstacleInPath(Vector3 start, Vector3 end)
    {
       
        RaycastHit hit;
        if (Physics.Raycast(start, end - start, out hit, Vector3.Distance(start, end)))
        {
            if (hit.collider.CompareTag("Wall")) 
            {
                return true; 
            }
        }

        return false; 
    }

    Vector3 GetRandomDashPosition()
    {
        
        float randomX = Random.Range(-dashDistance, dashDistance);
        float randomZ = Random.Range(-dashDistance, dashDistance);

       
        float yPosition = transform.position.y;

        return new Vector3(transform.position.x + randomX, yPosition, transform.position.z + randomZ);
    }

   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking)
        {
         
            gameEnding.CaughtPlayer();
        }
    }
}
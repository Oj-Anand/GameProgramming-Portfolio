using UnityEngine;
using System.Collections;

public class KickoutHole : MonoBehaviour
{
    public float kickForce = 10f;
    public float delay = 1f;
    public int points = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                StartCoroutine(HoldAndKick(rb));

                // Add score
                GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
                gameState.Score += points;

                // Give player 1 extra ball!
                gameState.BallsRemaining++;
            }
        }
    }

    private IEnumerator HoldAndKick(Rigidbody2D rb)
    {
        // Move ball to exact hole center
        rb.position = transform.position;
        
        // Freeze the ball
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; //freeze it physics wise 

        // Wait a moment
        yield return new WaitForSeconds(delay);

        // Kick it upward
        rb.bodyType = RigidbodyType2D.Dynamic; //back to regular pinball
        Vector2 launchDirection = new Vector2(0.5f, 1f).normalized; //because launching it right up causes an infinite loop
        rb.AddForce(launchDirection * kickForce, ForceMode2D.Impulse);
    }
}

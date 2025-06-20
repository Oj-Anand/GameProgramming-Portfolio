using UnityEngine;
using System.Collections;
public class Bumper : MonoBehaviour
{
    public float force;
    //color flash variables
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    private SpriteRenderer _renderer;
    private Color _originalColor;
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.color;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D theRBThatHitMe = collision.gameObject.GetComponent<Rigidbody2D>();
        if (theRBThatHitMe != null)
        {
            // Using collision normal for pinball-style "springy" bounce
            Vector2 bounceDirection = collision.contacts[0].normal * -force;
            theRBThatHitMe.AddForce(bounceDirection, ForceMode2D.Impulse);
        }

        if (_renderer != null)
        {
            StartCoroutine(FlashColor());
        }
    }
        IEnumerator FlashColor()
    {
        _renderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        _renderer.color = _originalColor;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;


public class Plunger : MonoBehaviour
{
    public Transform plungerLowestPoint;
    public Transform plungerStop;
    public float force;
    private InputAction spacebarAction;
    void Start()
    {
        spacebarAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        //have we reached the lowest point yet? are they pressing the spacebar?
        if(transform.position.y >= plungerLowestPoint.position.y 
            && spacebarAction.ReadValue<float>() != 0)
        {
            transform.Translate(0, spacebarAction.ReadValue<float>() * Time.deltaTime * -2, 0);
        }

        if(spacebarAction.WasReleasedThisFrame())
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(new Vector2(0, force * (plungerStop.position.y - transform.position.y)), ForceMode2D.Impulse);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    public float movementSpeed;
    private InputAction _moveAction;

    private bool _maxLeftReached = false, _maxRightReached = false;
    public GameObject projectilePrefab;  
    public float fireRate = 0.5f;        // Time between shots

    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
    }
    void Update()
    {

        //Debug.Log("Player is active and running Update.");

        Vector2 translationValue = _moveAction.ReadValue<Vector2>();
        translationValue.y = 0;
        translationValue *= Time.deltaTime * movementSpeed;


        if(translationValue.x < 0 && !_maxLeftReached)
        {
            transform.Translate(translationValue);
        }

        if(translationValue.x > 0 && !_maxRightReached)
        {
            transform.Translate(translationValue);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

        
    }


    void OnTriggerEnter2D(Collider2D collider2D)
    {
        
        if(collider2D.CompareTag("WallLeft"))
        {
            _maxLeftReached = true;
        }
        if(collider2D.CompareTag("WallRight")) 
        {
            _maxRightReached = true;
        }

    }
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("WallLeft"))
        {
            _maxLeftReached = false;
        }
        if(collider2D.CompareTag("WallRight")) 
        {
            _maxRightReached = false;
        }
    }
    
    private void Fire()
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.direction = Vector2.up;   // Move upward
            projectileScript.speed = 7f;              // Faster than enemy bullets
            projectileScript.shooterTag = "Player";   // Set shooter as Player
        }

        Debug.Log("Player fired a projectile!");
    }
    
    // void OnTriggerStay2D(Collider2D collider2D)
    // {
    //     //Debug.Log("#################OnTriggerStay2D#################");
    // }
}

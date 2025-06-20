using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public float maxSpeed, jumpForce, moveForce;//5, 100, 200
    public Transform groundCheck;
    private Rigidbody2D _myRigidBody;
    private Animator _myAnimator;
    private InputAction _jump, _move;
    private bool _grounded, _jumpInitiated, _facingRight;


    void Awake()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();

        var playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.actions.FindActionMap("Player");

        _jump = actionMap.FindAction("Jump");
        _move = actionMap.FindAction("Move");

        _jump.Enable();
        _move.Enable();

        _jumpInitiated = false;
        _facingRight = true;
    }

    void Update()
    {
        //_grounded will be true if the line passes through an object set to have the layer "Ground"
        _grounded = 
            Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if(_jump.WasPressedThisFrame() && _grounded)
        {
            _jumpInitiated = true;
        }    
    }

    void FixedUpdate()
    {
        float horizontalMovement = _move.ReadValue<Vector2>().x;

        //Have we reached _maxSpeed? If not, add force.
        if(horizontalMovement * _myRigidBody.linearVelocityX < maxSpeed)
        {
            _myRigidBody.AddForce(Vector2.right * horizontalMovement * moveForce);
        }
        //clamp to max speed
        //Mathf.Sign returns -1 or 1, and allows us to keep the direction in world space
        if(Mathf.Abs(_myRigidBody.linearVelocityX) > maxSpeed)
        {
            _myRigidBody.linearVelocityX = Mathf.Sign(_myRigidBody.linearVelocityX) * maxSpeed;
        }
        if(_jumpInitiated)
        {
            _myRigidBody.AddForce(new Vector2(0, jumpForce));
            _myAnimator.SetTrigger("Jump");
            _jumpInitiated = false;
        }
        if(horizontalMovement > 0 && !_facingRight || horizontalMovement < 0 && _facingRight)
        {
            Flip();
        }
    }
    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

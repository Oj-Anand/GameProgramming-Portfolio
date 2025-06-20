using UnityEngine;
using UnityEngine.InputSystem;

public class Flipper : MonoBehaviour
{
    private HingeJoint2D _joint2D;
    private InputAction _flipRight;
    private InputAction _flipLeft;
    
    public enum FlipperType { Right, Left}
    public FlipperType _flipperType;

    void Start()
    {
        _joint2D = GetComponent<HingeJoint2D>();

        PlayerInput playerInput = FindAnyObjectByType<PlayerInput>(); //get the player input into the scene
        if (playerInput != null)
        {
            _flipRight = playerInput.actions.FindAction("FlipperRight");
            _flipLeft = playerInput.actions.FindAction("FlipperLeft");
        }
        else
        {
            Debug.LogError("No PlayerInput found in scene!");
        }
    }
    void Update()
    {
        if (_flipRight == null || _flipLeft == null)
            return;
        switch(_flipperType)
        {
            case FlipperType.Right:
                _joint2D.useMotor = _flipRight.IsPressed();
                break;
            case FlipperType.Left:
                _joint2D.useMotor = _flipLeft.IsPressed();
                break;
        }
        
    }
}

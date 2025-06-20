using UnityEngine;
using System.Collections;

public class PlatformMoving : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical }
    public MoveDirection direction = MoveDirection.Horizontal;
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Vector3 _targetPoint;
    public float pauseDuration = 1f;

    void Start()
    {
        _startPoint = transform.position; //start at your urrent position 
        if(direction == MoveDirection.Horizontal)
        {
            _endPoint = _startPoint + Vector3.right * moveDistance; //set end position based on its start
        }
        else
        {
            _endPoint = _startPoint + Vector3.up * moveDistance; 
        }
        
        _targetPoint = _endPoint;
        StartCoroutine(MovePlatform());

    }

    IEnumerator MovePlatform()
    {
        while(true)//loops forever
        {
            while (Vector3.Distance(transform.position, _targetPoint) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPoint, moveSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

            yield return new WaitForSeconds(pauseDuration);

            if(_targetPoint == _startPoint)
            {
                _targetPoint = _endPoint;
            }
            else
            {
                _targetPoint = _startPoint;
            }
        }
    }

}

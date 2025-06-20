using UnityEngine;

public class PlungerStop : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        //collision.gameObject is the gameobject that we collided into
        if(collision.gameObject.name == "Plunger")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = 
                RigidbodyType2D.Kinematic;
        }
    }
}

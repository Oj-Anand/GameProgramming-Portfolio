using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform spawnPointTransform;
    public GameObject ballPrefab;
    void Start()
    {
        Instantiate(ballPrefab).transform.position = spawnPointTransform.position;
        // GameObject newBall = Instantiate(ballPrefab);
        // newBall.transform.position = spawnPointTransform.position;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(collider.gameObject);

        StartCoroutine(WaitThenSpawnBall()); 
    }

    IEnumerator WaitThenSpawnBall()
    {
        yield return new WaitForSeconds(5);//yield control of this process for 5 seconds
        GameObject newBall = Instantiate(ballPrefab);
        newBall.transform.position = spawnPointTransform.position;
    }
}

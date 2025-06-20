using UnityEngine;

public class KillerAlien : Alien
{
    public GameObject killerProjectilePrefab; // Assign in Inspector
    public Transform firePoint; // Set a point from where it fires

    public override void Fire()
    {
        if (killerProjectilePrefab)
        {
            Instantiate(killerProjectilePrefab, transform.position, Quaternion.identity);
            Debug.Log("Killer Alien fired an instant-kill projectile!");
        }
    }
}

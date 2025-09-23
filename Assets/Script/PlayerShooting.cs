using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectilePrefabs_2;
    public Transform firePoint;
    public GameObject currentBullet;
    private int clickCount = 0;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        currentBullet = projectilePrefab;
    }
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentBullet = projectilePrefabs_2;
            clickCount++;
            Debug.Log(clickCount);
            if (clickCount % 2 != 0)
            {
                currentBullet = projectilePrefabs_2;
            }
            else
            {
                currentBullet = projectilePrefab;
            }
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            Shoot(currentBullet);
        }
        
        
    }

    void Shoot(GameObject bullet)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject proj = Instantiate(bullet, firePoint.position, Quaternion.LookRotation(direction));
    }
}

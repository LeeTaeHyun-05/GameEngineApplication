using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask hitMask = ~0;
    public int toolDamage = 1;
    public float hitCooldown = 0.15f;
    public float _nextHitTime;
    private Camera _cam;
    public Inventory inventory;
    void Awake()
    {
        _cam = Camera.main;
        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
            Debug.Log("test");
            Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out var hit, rayDistance, hitMask))
            {
                Debug.Log("test2");
                var block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    Debug.Log("test3");
                    block.Hit(toolDamage, inventory);
                }
            }
        }
    }
}

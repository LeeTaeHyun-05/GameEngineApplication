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
    InventoryUI invenUI;
    public GameObject selectedBlock;
    void Awake()
    {
        _cam = Camera.main;
        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();
        invenUI = FindObjectOfType<InventoryUI>();  
    }
    
    // Update is called once per frame
    void Update()
    {
        if (invenUI.selectedIndex < 0)
        {
            selectedBlock.transform.localScale = Vector3.zero;
            if (Input.GetMouseButtonDown(0))
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
        else
        {
            Ray rayDebug = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast (rayDebug, out var hitDebug, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                Vector3Int placePos = AdjacentCellOnHitFace(hitDebug);
                selectedBlock.transform.localScale = Vector3.one;
                selectedBlock.transform.position = placePos;
                selectedBlock.transform.rotation = Quaternion.identity;
            }
            else
            {
                selectedBlock.transform.localScale = Vector3.zero;  
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3Int placePos = AdjacentCellOnHitFace(hit);

                    BlockType selected = invenUI.GetInventorySlot();
                    if (inventory.Consume(selected, 1))
                    {
                        FindObjectOfType<NoiseVoxelMap>().PlaceTile(placePos, selected);
                    }
                }
            }
        }
    }
    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position;
        Vector3 adjCenter = baseCenter + hit.normal;
        return Vector3Int.RoundToInt(adjCenter);
    }
}

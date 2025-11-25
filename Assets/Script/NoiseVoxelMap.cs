using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
   
    public GameObject dirtPrefab;
    public GameObject waterPrefab;
    public GameObject grassPrefab;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;  //Y
    public int waterLevel = 4;
    [SerializeField] float noiseScale = 20f; // 값이 높을수록 평평한 지형
    // Start is called before the first frame update
    void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offSetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offSetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);

                int h = Mathf.FloorToInt(noise * maxHeight);

                if (h <= 0) h = 1;
                for (int y = 0; y <= h; y++)
                {
                    if ( y ==  h )
                    {
                        PlaceGrass(x, y, z);
                    }
                    else
                    {
                        PlaceDirt(x, y, z);
                    }
                }
                for (int y = h +1; y <= waterLevel; y ++)
                {
                    PlaceWater(x, y, z);
                }
            }
        }
        
    }

    private void PlaceDirt (int x, int y, int z)
    {
        var go = Instantiate(dirtPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Dirt_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Dirt;
        b.maxHP = 3;
        b.dropcount = 1;
        b.mineable = true;
       
    }
    private void PlaceGrass(int x, int y, int z)
    {
        var go = Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Grass_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Grass;
        b.maxHP = 3;
        b.dropcount = 1;
        b.mineable = true;


    }
    private void PlaceWater(int x, int y, int z)
    {
        var go = Instantiate(waterPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Water_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Water;
        b.maxHP = 3;
        b.dropcount = 1;
        b.mineable = false;

    }
    public void PlaceTile(Vector3Int pos, BlockType type)
    {
        switch (type)
        {
            case BlockType.Dirt:
                PlaceDirt(pos.x, pos.y, pos.z);
                break;
            case BlockType.Grass:
                PlaceGrass(pos.x, pos.y, pos.z);
                break;
            case BlockType.Water:
                PlaceWater(pos.x, pos.y, pos.z);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

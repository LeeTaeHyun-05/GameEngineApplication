using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    class TileInfo
    {
        public int tileX;
        public int tileY;
        public int tileZ;
        public string tileType;
    }
    public GameObject dirtPrefab;
    public GameObject waterPrefab;
    public GameObject grassPrefab;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;  //Y
    [SerializeField] float noiseScale = 20f; // 값이 높을수록 평평한 지형
    List<TileInfo> tileInfos = new List<TileInfo>();
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

                if (h <= 0) continue;

                for (int y = 0; y <= h; y++)
                {
                    if ( y ==  h )
                    {
                        PlaceGrass(x, y, z);
                    }
                    else
                    {
                        Place(x, y, z);
                    }
                }
            }
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y <= 4; y++)
                {
                    if (tileInfos.Find(tile => tile.tileX == x && tile.tileZ == z && tile.tileY == y) == null)
                    {
                        PlaceWater(x, y, z);
                    }
                }
            }
        }
    }

    private void Place (int x, int y, int z)
    {
        var go = Instantiate(dirtPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
        tileInfos.Add(new TileInfo
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Dirt"
        });
        
    }
    private void PlaceGrass(int x, int y, int z)
    {
        var go = Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
        tileInfos.Add(new TileInfo
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Grass"
        });
    }private void PlaceWater(int x, int y, int z)
    {
        var go = Instantiate(waterPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
        tileInfos.Add(new TileInfo
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Water"
        });
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

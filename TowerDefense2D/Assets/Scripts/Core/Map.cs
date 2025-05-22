using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    private Vector2 offset;

    // Grootte van de map
    public int width = 50;
    public int height = 50;

    // Prefabs voor path en tower tiles
    [SerializeField] private GameObject pathTilePrefab;  // Pad prefab (bruin)
    [SerializeField] private GameObject mapTilePrefab; // Tower prefab (groen)

    // Lijst van coördinaten voor het pad
    private List<Vector2> path = new List<Vector2>();

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    private void Awake()
    {
        offset = new Vector2(width / 2f, height / 2f);

        CreatePath();

    }

    void Start()
    {

        // Genereer de map met de juiste tiles
        GenerateMap();
    }

    void CreatePath()
    {
        // Handmatig opgegeven routepunten
        List<Vector2Int> rawPoints = new List<Vector2Int>
    {
        new Vector2Int(0, 10),
        new Vector2Int(5, 10),
        new Vector2Int(5, 8),
        new Vector2Int(10, 8),
        new Vector2Int(15, 3),
        new Vector2Int(20, 8),
        new Vector2Int(20, 12),
        new Vector2Int(25, 12),
        new Vector2Int(30, 15),
        new Vector2Int(40, 20),
        new Vector2Int(49, 25),
    };

        path.Clear();

        for (int i = 0; i < rawPoints.Count - 1; i++)
        {
            Vector2Int start = rawPoints[i];
            Vector2Int end = rawPoints[i + 1];

            // Eerst horizontaal
            int xStep = start.x < end.x ? 1 : (start.x > end.x ? -1 : 0);
            int yStep = start.y < end.y ? 1 : (start.y > end.y ? -1 : 0);

            Vector2Int current = start;
            path.Add((Vector2)current);

            // Loop totdat we bij eindpunt zijn
            while (current != end)
            {
                if (current.x != end.x)
                    current.x += xStep;
                else if (current.y != end.y)
                    current.y += yStep;

                path.Add((Vector2)current);
            }
        }

        // Offset toepassen om map te centreren
        for (int i = 0; i < path.Count; i++)
        {
            path[i] -= offset;
        }
    }




    void GenerateMap()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new Vector2(x, y) - offset;
                Vector2 localCoord = new Vector2(x, y) - offset;

                if (path.Contains(position))
                {
                    Instantiate(pathTilePrefab, position, Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(mapTilePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }



    public List<Vector2> GetPath()
    {
        return path;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    private Vector2 _offset;

    // Grootte van de map
    [SerializeField] private int _width = 50;
    [SerializeField] private int _height = 30;

    // Prefabs voor path en tower tiles
    [SerializeField] private GameObject _pathTilePrefab;  // Pad prefab (bruin)
    [SerializeField] private GameObject _mapTilePrefab; // Tower prefab (groen)

    // Lijst van coördinaten voor het pad
    private List<Vector2> path = new List<Vector2>();

    public int Width { get { return _width; } }
    public int Height { get { return _height; } }

    private void Awake()
    {
        _offset = new Vector2(_width / 2f, _height / 2f);

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
            path[i] -= _offset;
        }
    }




    void GenerateMap()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Vector2 position = new Vector2(x, y) - _offset;
                Vector2 localCoord = new Vector2(x, y) - _offset;

                if (path.Contains(position))
                {
                    Instantiate(_pathTilePrefab, position, Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(_mapTilePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }



    public List<Vector2> GetPath()
    {
        return path;
    }
}

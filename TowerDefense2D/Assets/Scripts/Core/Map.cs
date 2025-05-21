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


    void Start()
    {
        offset = new Vector2(width / 2f, height / 2f);

        // Maak een eenvoudig pad dat van links naar rechts slingert
        CreatePath();

        // Genereer de map met de juiste tiles
        GenerateMap();
    }

    void CreatePath()
    {
        // Startpunt aan de linkerkant
        path.Add(new Vector2(0, 0));

        for (int x = 1; x < width - 1; x++)
        {
            if (x % 2 == 0)
                path.Add(new Vector2(x, 0));
            else
                path.Add(new Vector2(x, 1));
        }

        path.Add(new Vector2(width - 1, 1));

        // Pas alle punten aan zodat ze gecentreerd liggen
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

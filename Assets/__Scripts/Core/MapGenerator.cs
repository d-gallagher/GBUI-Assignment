using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    #region Public Variables
    public GameMap[] maps;
    public int mapIndex;
    public Transform tilePrefab;
    public Transform obstaclePrefab;


    public Transform mapFloor;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefab;
    public Vector2 mapSize;
    public Vector2 maxMapSize;
    [Range(0, 1)]
    public float outlinePercent;
    [Range(0, 1)]
    public float obstaclePercent;
    public int seed = 10;
    public float tileSize;
    #endregion

    #region Private Variables
    private List<Coord> _allTileCoords;
    private Queue<Coord> _shuffledTileCoords;
    private Queue<Coord> _shuffledOpenTileCoords;

    private GameMap _currentMap;
    private Transform[,] _tileMap;
    #endregion

    #region Unity Methods
    private void Awake() => FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
    #endregion

    private void OnNewWave(int waveNumber)
    {
        mapIndex = waveNumber - 1;
        GenerateMap();
    }

    public void GenerateMap()
    {
        _currentMap = maps[mapIndex];
        _tileMap = new Transform[_currentMap.mapSize.x, _currentMap.mapSize.y];
        System.Random prng = new System.Random(_currentMap.seed);
        //GetComponent<BoxCollider>().size = new Vector3(_currentMap.mapSize.x * tileSize, .05f, _currentMap.mapSize.y * tileSize);

        // Generating coords
        _allTileCoords = new List<Coord>();
        for (int x = 0; x < _currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < _currentMap.mapSize.y; y++)
            {
                _allTileCoords.Add(new Coord(x, y));
            }
        }
        _shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(_allTileCoords.ToArray(), _currentMap.seed));

        // Create map holder object
        string holderName = "Generated Map";
        if (transform.Find(holderName)) DestroyImmediate(transform.Find(holderName).gameObject);

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // Spawning tiles
        for (int x = 0; x < _currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < _currentMap.mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.parent = mapHolder;
                _tileMap[x, y] = newTile;
            }
        }

        // Spawning obstacles
        bool[,] obstacleMap = new bool[(int)_currentMap.mapSize.x, (int)_currentMap.mapSize.y];

        int obstacleCount = (int)(_currentMap.mapSize.x * _currentMap.mapSize.y * _currentMap.obstaclePercent);
        int currentObstacleCount = 0;

        List<Coord> allOpenCoords = new List<Coord>(_allTileCoords);

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != _currentMap.MapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(_currentMap.minObstacleHeight, _currentMap.maxObstacleHeight, (float)prng.NextDouble());
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight / 2, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
                newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);

                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                float colourPercent = randomCoord.y / (float)_currentMap.mapSize.y;
                obstacleMaterial.color = Color.Lerp(_currentMap.foregroundColour, _currentMap.backgroundColour, colourPercent);
                obstacleRenderer.sharedMaterial = obstacleMaterial;

                allOpenCoords.Remove(randomCoord);
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

        _shuffledOpenTileCoords = new Queue<Coord>(Utility.ShuffleArray(allOpenCoords.ToArray(), _currentMap.seed));

        // Creating navmesh mask
        Transform maskLeft = Instantiate(navmeshMaskPrefab, Vector3.left * (_currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.x - _currentMap.mapSize.x) / 2f, 1, _currentMap.mapSize.y) * tileSize;

        Transform maskRight = Instantiate(navmeshMaskPrefab, Vector3.right * (_currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale = new Vector3((maxMapSize.x - _currentMap.mapSize.x) / 2f, 1, _currentMap.mapSize.y) * tileSize;

        Transform maskTop = Instantiate(navmeshMaskPrefab, Vector3.forward * (_currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - _currentMap.mapSize.y) / 2f) * tileSize;

        Transform maskBottom = Instantiate(navmeshMaskPrefab, Vector3.back * (_currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - _currentMap.mapSize.y) / 2f) * tileSize;

        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
        mapFloor.localScale = new Vector3(_currentMap.mapSize.x * tileSize, _currentMap.mapSize.y * tileSize);
    }

    private bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(_currentMap.MapCentre);
        mapFlags[_currentMap.MapCentre.x, _currentMap.MapCentre.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(_currentMap.mapSize.x * _currentMap.mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private Vector3 CoordToPosition(int x, int y) => new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);

    public Transform GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / tileSize + (_currentMap.mapSize.x - 1) / 2f);
        int z = Mathf.RoundToInt(position.z / tileSize + (_currentMap.mapSize.y - 1) / 2f);

        x = Mathf.Clamp(x, 0, _tileMap.GetLength(0) - 1);
        z = Mathf.Clamp(z, 0, _tileMap.GetLength(1) - 1);

        return _tileMap[x, z];
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = _shuffledTileCoords.Dequeue();
        _shuffledTileCoords.Enqueue(randomCoord);

        return randomCoord;
    }

    public Transform GetRandomOpenTile()
    {
        Coord randomCoord = _shuffledOpenTileCoords.Dequeue();
        _shuffledOpenTileCoords.Enqueue(randomCoord);

        return _tileMap[randomCoord.x, randomCoord.y];
    }

    [Serializable]
    public class GameMap
    {
        public Coord mapSize;
        [Range(0, 1)]
        public float obstaclePercent;
        public int seed;
        public float minObstacleHeight;
        public float maxObstacleHeight;
        public Color foregroundColour;
        public Color backgroundColour;

        public Coord MapCentre => new Coord(mapSize.x / 2, mapSize.y / 2);
    }
}

[Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static bool operator ==(Coord c1, Coord c2) => c1.x == c2.x && c1.y == c2.y;

    public static bool operator !=(Coord c1, Coord c2) => !(c1 == c2);

    public override bool Equals(object obj)
    {
        return obj is Coord coord &&
               x == coord.x &&
               y == coord.y;
    }

    public override int GetHashCode()
    {
        var hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }
}
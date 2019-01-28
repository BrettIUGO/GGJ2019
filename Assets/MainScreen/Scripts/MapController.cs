using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject mapTile;
    public Vector2 mapSize;
    public float mapIndent = 1;

    [Range(0, 10)]
    public int minNumElements = 1;

    [Range(5, 20)]
    public int maxNumElements = 10;
    public GameObject[] elements;

    public GameObject mapBorderPrefab;

    private static MapController _instance = null;
    public static MapController Instance { get
        {
            return _instance;
        }
    }

    private Vector2 _minMapExtents;
    public Vector2 minMapExtents
    {
        get
        {
            return _minMapExtents;
        }
    }

    private Vector2 _maxMapExtents;
    public Vector2 maxMapExtents
    {
        get
        {
            return _maxMapExtents;
        }
    }

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance.gameObject);
        _instance = this;

        for(int i = 0; i < mapSize.x; ++i)
        {
            for(int j = 0; j < mapSize.y; ++j)
            {
                GameObject mapTileObj = Instantiate(mapTile, transform);
                mapTileObj.transform.position = new Vector3(i * 10, 0, j * 10);
            }
        }

        _minMapExtents.x = -5.0f;
        _minMapExtents.y = -5.0f;
        _maxMapExtents.x = (mapSize.x * 10) - 5;
        _maxMapExtents.y = (mapSize.y * 10) - 5;

        Camera.main.transform.position = new Vector3(
            (_minMapExtents.x + _maxMapExtents.x) * 0.5f,
            Camera.main.transform.position.y,
            (_minMapExtents.y + _maxMapExtents.y) * 0.5f
        );

        //Make borders
        //North
        GameObject northWall = Instantiate(mapBorderPrefab, transform);
        northWall.name = "NorthWall";
        northWall.transform.position = new Vector3((_minMapExtents.x + _maxMapExtents.x) * 0.5f, 0, _maxMapExtents.y);
        northWall.transform.rotation = Quaternion.Euler(0, -90, 0);
        northWall.transform.localScale = new Vector3(1, 1, mapSize.x * 10);

        //South
        GameObject southWall = Instantiate(mapBorderPrefab, transform);
        southWall.name = "SouthWall";
        southWall.transform.position = new Vector3((_minMapExtents.x + _maxMapExtents.x) * 0.5f, 0, _minMapExtents.y);
        southWall.transform.rotation = Quaternion.Euler(0, 90, 0);
        southWall.transform.localScale = new Vector3(1, 1, mapSize.x * 10);

        //West
        GameObject westWall = Instantiate(mapBorderPrefab, transform);
        westWall.name = "WestWall";
        westWall.transform.position = new Vector3(_maxMapExtents.x, 0, (_minMapExtents.y + _maxMapExtents.y) * 0.5f);
        westWall.transform.rotation = Quaternion.Euler(0, 0, 0);
        westWall.transform.localScale = new Vector3(1, 1, mapSize.x * 10);

        //East
        GameObject eastWall = Instantiate(mapBorderPrefab, transform);
        eastWall.name = "EastWall";
        eastWall.transform.position = new Vector3(_minMapExtents.x, 0, (_minMapExtents.y + _maxMapExtents.y) * 0.5f);
        eastWall.transform.rotation = Quaternion.Euler(0, 180, 0);
        eastWall.transform.localScale = new Vector3(1, 1, mapSize.x * 10);

        //Just to bring players, elements etc. inwards
        _minMapExtents.x += mapIndent;
        _minMapExtents.y += mapIndent;
        _maxMapExtents.x -= mapIndent;
        _maxMapExtents.y -= mapIndent;

        GenerateMapElements();
    }

    public void ResetGame()
    {
        GenerateMapElements();
    }

    private void GenerateMapElements()
    {
        GameObject[] existingElements = GameObject.FindGameObjectsWithTag("MapElement");
        for(int i = 0; i < existingElements.Length; ++i)
        {
            Destroy(existingElements[i]);
        }

        //Auto Generate Map Elements
        int numElements = Random.Range(minNumElements, maxNumElements);
        for (int i = 0; i < numElements; ++i)
        {
            int randomElementIndex = Random.Range(0, elements.Length);
            GameObject element = Instantiate(elements[randomElementIndex], transform);
            element.transform.position = new Vector3(
                    Random.Range(_minMapExtents.x, _maxMapExtents.x),
                    0,
                    Random.Range(_minMapExtents.y, _maxMapExtents.y));
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

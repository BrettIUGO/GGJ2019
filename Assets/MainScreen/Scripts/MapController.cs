using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject mapTile;
    public Vector2 mapSize;

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

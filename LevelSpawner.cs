using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public List<RoadTile> RoadTiles => _roadTiles;

    [Header("Level texture")]
    [SerializeField] private Texture2D _levelTexture;

    [Header("Tiles Prefabs")]
    [SerializeField] private GameObject _wallTile;
    [SerializeField] private GameObject _roadTile;

    [Header("Player Prefab")]
    [SerializeField] private GameObject _player;

    private Color _wallColor = Color.white;
    private Color _roadColor = Color.black;

    private float _unitPerPixel;

    private List<RoadTile> _roadTiles = new List<RoadTile>();

    private void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        _unitPerPixel = _wallTile.transform.localScale.x;
        float halfUnitPerPixel = _unitPerPixel / 2f;

        float width = _levelTexture.width;
        float heigth = _levelTexture.height;

        Vector3 offset = (new Vector3(width / 2f, 0f, heigth / 2f) * _unitPerPixel)
                        - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < heigth; y++)
            {
                Color pixelColor = _levelTexture.GetPixel(x, y);

                Vector3 spawnPosition = ((new Vector3(x, 0f, y) * _unitPerPixel) - offset);

                if (pixelColor == _wallColor)
                    Spawn(_wallTile, spawnPosition);
                else if (pixelColor == _roadColor)
                    Spawn(_roadTile, spawnPosition);
            }
        }

        Spawn(_player, _roadTiles[0].transform.position);
    }

    private void Spawn(GameObject tile,Vector3 position)
    {
        position.y = tile.transform.position.y;

        GameObject spawnedTile = Instantiate(tile, position, Quaternion.identity, transform);

        if (tile == _roadTile)
            _roadTiles.Add(spawnedTile.GetComponent<RoadTile>());
    }
}

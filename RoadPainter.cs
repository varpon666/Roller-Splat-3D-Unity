using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadPainter : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;

    [SerializeField] private RoadTile _changedTile;
    [SerializeField] private Color _paintColor;

    private int _paintedRoadTiles = 0;

    private void Start()
    {
        Paint(_levelSpawner.RoadTiles[0], 0.5f, 0f);

        GameEvents.Instance.OnMoveStarted += OnPlayerMoveStartHandler;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnMoveStarted -= OnPlayerMoveStartHandler;
    }

    private void OnPlayerMoveStartHandler(List<RoadTile> roadTiles, float TotalDuration)
    {
        float stepDuration = TotalDuration / roadTiles.Count;

        for(int i = 0; i < roadTiles.Count; i++)
        {
            RoadTile tile = roadTiles[i];

            if(tile.IsPainted == false)
            {
                float duration = TotalDuration / 2f;
                float delay = i * (stepDuration / 2f);

                Paint(tile, duration, delay);

                if(_paintedRoadTiles == _levelSpawner.RoadTiles.Count)
                {
                    Debug.Log("Level Comleted");
                }
            }
        }
    }

    private void Paint(RoadTile tile, float duration, float delay)
    {
        StartCoroutine(Paint(tile,_changedTile, delay));

        /*tile.GetComponent<MeshRenderer>().material
            .DOColor(_paintColor, duration)
            .SetDelay(delay);*/

        tile.IsPainted = true;
        _paintedRoadTiles++;
    }

    private IEnumerator Paint(RoadTile previousTile, RoadTile nextTile, float delay)
    {
        yield return new WaitForSeconds(delay);

        previousTile.gameObject.SetActive(false);
        RoadTile road = Instantiate(nextTile, previousTile.transform.position, Quaternion.identity);
    }
}

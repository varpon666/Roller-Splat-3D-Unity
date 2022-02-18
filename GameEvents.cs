using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance => _instance;
    private static GameEvents _instance;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public UnityAction<List<RoadTile>, float> OnMoveStarted;
    public UnityAction<RoadTile, float, float> OnPainted;

    public void PlayerMoveStartHandler(List<RoadTile> roadTiles, float duration)
    {
        OnMoveStarted?.Invoke(roadTiles, duration);
    }

    public void Paint(RoadTile tile, float duration, float delay)
    {
        OnPainted?.Invoke(tile, duration, delay);
    }
}

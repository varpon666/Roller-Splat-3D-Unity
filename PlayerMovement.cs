using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using DG.Tweening;
using System.Linq;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SwipeListener _swipeListener;

    [SerializeField] private LayerMask _wallsAndRoadsLayer;

    [SerializeField] private float _stepDuration = 0.1f;

    private const float MAX_RAY_DISTANCE = 100f;

    private Vector3 _moveDirection;
    private Vector3 _targetRotate;

    private bool _canMove = true;

    private void Start()
    {
        _swipeListener.OnSwipe.AddListener(swipe =>
        {
            switch (swipe)
            {
                case "Right":
                    _moveDirection = Vector3.right;
                    _targetRotate = new Vector3(0f, 90f, 0f);
                    break;
                case "Left":
                    _moveDirection = Vector3.left;
                    _targetRotate = new Vector3(0f, -90f, 0f);
                    break;
                case "Up":
                    _moveDirection = Vector3.forward;
                    _targetRotate = new Vector3(0f, 0f, 0f);
                    break;
                case "Down":
                    _moveDirection = Vector3.back;
                    _targetRotate = new Vector3(0f, 180f, 0f);
                    break;
            }

            Move();
        });
    }

    private void Move()
    {
        if(_canMove == true)
        {
            _canMove = false;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, _moveDirection, MAX_RAY_DISTANCE, _wallsAndRoadsLayer.value)
                .OrderBy(hit => hit.distance).ToArray();

            Vector3 targetPosition = transform.position;

            int steps = 0;

            List<RoadTile> pathRoadTiles = new List<RoadTile>();

            for(int i = 0; i < hits.Length; i++)
            {
                if(hits[i].collider.isTrigger == true)
                {
                    pathRoadTiles.Add(hits[i].transform.GetComponent<RoadTile>());
                }
                else
                {
                    if(i == 0)
                    {
                        _canMove = true;
                        return;
                    }

                    steps = i;
                    targetPosition = hits[i - 1].transform.position;
                    break;
                }
            }

            float moveDuration = _stepDuration * steps;

            transform
                .DOMove(targetPosition, moveDuration)
                .SetEase(Ease.OutExpo)
                .OnComplete(() => _canMove = true);

            transform
                .DORotate(_targetRotate, 0.1f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() => _canMove = true);

            GameEvents.Instance.PlayerMoveStartHandler(pathRoadTiles, moveDuration);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    #region Vars
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float duration;
    private float progress;
    #endregion

    #region Inspector
    public Vector3 zoneConstraints = new Vector3(10, 10, 10);
    #endregion

    #region Unity
    private void Awake()
    {
        transform.position = GetRandomPosition();
        SetRandomDestination();
    }

    private void Update()
    {
        if (progress < duration)
        {
            progress += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress / duration);
            return;
        }

        SetRandomDestination();
    }
    #endregion

    #region Internal
    private void SetRandomDestination()
    {
        startPosition = transform.position;
        targetPosition = GetRandomPosition();

        duration = Random.Range(5f, 10f);
        progress = 0;
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-zoneConstraints.x / 2, zoneConstraints.x / 2),
                             Random.Range(-zoneConstraints.y / 2, zoneConstraints.y / 2),
                             Random.Range(-zoneConstraints.z / 2, zoneConstraints.z / 2));
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    #region Inspector
    public int amountToSpawnOnStart;
    #endregion

    #region Vars
    private int amount;
    private List<GameObject> objectsSpawned = new List<GameObject>();
    #endregion

    #region Context menu
    [ContextMenu("Spawn")]
    public void Spawn()
    {
        Spawn(amount);
    }

    [ContextMenu("Despawn")]
    public void Despawn()
    {
        Despawn(amount);
    }
    #endregion

    #region Internal
    private void OnInputChanged(int input)
    {
        amount = input;

    }

    private void Spawn(int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
           objectsSpawned.Add(Pooler.Instance.Get());
        }

        UIOverlay.outputChanged?.Invoke(objectsSpawned.Count);
    }

    private void Despawn(int amountToDespawn)
    {
        if (amountToDespawn > objectsSpawned.Count)
            throw new ArgumentOutOfRangeException();

        for (int i = amountToDespawn - 1; i >= 0; i--)
        {
            Pooler.Instance.Return(objectsSpawned[i]);
            objectsSpawned.RemoveAt(i);
        }

        UIOverlay.outputChanged?.Invoke(objectsSpawned.Count);
    }
    #endregion

    #region Unity
    private void Awake()
    {
        UIOverlay.inputChanged += OnInputChanged;
    }

    private void OnDestroy()
    {
        UIOverlay.inputChanged -= OnInputChanged;
    }

    void Start()
    {
        Spawn(amountToSpawnOnStart);
        UIOverlay.outputChanged?.Invoke(amountToSpawnOnStart);
    }

    #endregion
}

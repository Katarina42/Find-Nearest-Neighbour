using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay: MonoBehaviour
{
    #region Inspector
    public Text output;
    public InputField input;
    #endregion

    #region Events
    public static Action<int> outputChanged;
    public static Action<int> inputChanged;
    #endregion

    #region UI Handlers
    public void OnInputChanged()
    {
        if (int.TryParse(input.text, out var amount))
            inputChanged?.Invoke(amount);
    }

    public void OnOutputChanged(int amount)
    {
        output.text = amount.ToString();
    }
    #endregion

    #region Unity
    private void Awake()
    {
        outputChanged += OnOutputChanged;
    }

    private void OnDestroy()
    {
        outputChanged -= OnOutputChanged;
    }
    #endregion
}

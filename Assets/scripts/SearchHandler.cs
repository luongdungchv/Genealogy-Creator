using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SearchHandler : MonoBehaviour
{
    public static SearchHandler ins;

    public GameObject searchPanel;
    public static event EventHandler OnOpenSearch;
    public static event EventHandler OnCloseSearch;

    private void Start()
    {
        ins = this;
    }
    public void OpenSearch()
    {
        searchPanel.SetActive(true);
        OnOpenSearch?.Invoke(this, EventArgs.Empty);
    }
    public void CloseSearch()
    {
        searchPanel.SetActive(false);
        OnCloseSearch?.Invoke(this, EventArgs.Empty);

    }
}

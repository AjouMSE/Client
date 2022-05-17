using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    #region Private variables

    private const int Width = 6, Height = 5;

    [SerializeField] private GameObject panel;

    private GameObject[,] _panels;

    #endregion

    private void Start()
    {
        Init();
    }

    private void Update()
    {

    }

    public void Init()
    {
        if (_panels == null)
        {
            // int mapSize = Width * Height;
            _panels = new GameObject[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _panels[i, j] = Instantiate(panel, new Vector3(j * 4.2f, 0.2f, i * 3.2f), Quaternion.identity);
                }
            }
        }
    }

    public (int i, int j) GetIdx(Vector3 position)
    {
        int i = (int)Math.Floor(position.z / 3.2f);
        int j = (int)Math.Floor(position.x / 4.2f);

        return (i, j);
    }

    public Vector3 GetPosition(int i, int j)
    {
        return _panels[i, j].transform.position;
    }
}

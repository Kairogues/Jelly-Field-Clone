using System.Collections.Generic;
using UnityEngine;

public class MaterialLoader : MonoBehaviour
{
    public static MaterialLoader Instance;
    [SerializeField] public Dictionary<TileType, Material> materialDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
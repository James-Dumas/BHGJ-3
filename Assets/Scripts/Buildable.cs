using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="New Buildable", menuName="Scriptable Objects/Buildable", order=2)]
public class Buildable : ScriptableObject
{
    public GameObject Object;
    public Tile Tile;
    public int Price;
}

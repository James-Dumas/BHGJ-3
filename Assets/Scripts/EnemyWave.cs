using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Wave", menuName="Scriptable Objects/Enemy Wave", order=1)]
public class EnemyWave : ScriptableObject
{
    public GameObject[] Enemies;
}

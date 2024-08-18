using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
[System.Serializable]
public class Quest : ScriptableObject
{
    public string questTitle;
    public string questDescription;
    public bool isCompleted;
}
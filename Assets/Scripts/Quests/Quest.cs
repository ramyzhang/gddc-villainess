using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Quest : ScriptableObject
{
    public string questTitle;
    public string questDescription;
    public bool isCompleted;
}
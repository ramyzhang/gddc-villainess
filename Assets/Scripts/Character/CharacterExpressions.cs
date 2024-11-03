using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterExpressions : ScriptableObject
{
    [System.Serializable]
    public struct ExpressionSet {
        public Sprite eyebrows;
        public Sprite eyes;
        public Sprite nosemouth;
    }

    public ExpressionSet neutral;
    public ExpressionSet happy;
    public ExpressionSet excited;
    public ExpressionSet surprised;
    public ExpressionSet blushing;
    public ExpressionSet sad;
    public ExpressionSet crying;
    public ExpressionSet worried;
    public ExpressionSet angry;
    public ExpressionSet determined;
    public ExpressionSet exasperated;

    public Dictionary<string, ExpressionSet> expressionLookup => new Dictionary<string, ExpressionSet>()
    {
        { "neutral", neutral },
        { "happy", happy },
        { "excited", excited },
        { "surprised", surprised },
        { "blushing", blushing },
        { "sad", sad },
        { "crying", crying },
        { "worried", worried },
        { "angry", angry },
        { "determined", determined },
        { "exasperated", exasperated },
    };
}

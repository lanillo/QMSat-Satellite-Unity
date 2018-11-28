using System;
using UnityEngine;

[Serializable]
public class PersonScore : IComparable<PersonScore> {

    [SerializeField]
    public string personName;
    [SerializeField]
    public float score;

    public PersonScore(string newName, float newScore)
    {
        personName = newName;
        score = newScore;
    }

    public int CompareTo(PersonScore other)
    {
        if (other == null)
        {
            return 1;
        }

        return (int) (other.score - score);
    }
}

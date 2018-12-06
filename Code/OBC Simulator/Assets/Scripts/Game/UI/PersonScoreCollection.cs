using System;
using UnityEngine;
using System.Collections.Generic;

/* Create PersonScoreCollection */
[Serializable]
class PersonScoreCollection {

    [SerializeField]
    public List<PersonScore> persons = new List<PersonScore>
    {
        new PersonScore("Luis", 1500f),
        new PersonScore("Fred", 0f),
        new PersonScore("Samuel", 300f),
        new PersonScore("Burge", 100f)
    };
}

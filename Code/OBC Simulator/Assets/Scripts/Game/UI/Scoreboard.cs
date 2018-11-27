using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Scoreboard : MonoBehaviour {

    public GameObject playerRowPrefab;
    public GameObject scoreboardPanel;

    [SerializeField]
    List<PersonScore> scoreboard = new List<PersonScore>
    {
        new PersonScore("Luis", 1500f),
        new PersonScore("Fred", 0f),
        new PersonScore("Samuel", 300f),
        new PersonScore("Burge", 100f)
    };

    public void UpdateText()
    {
        foreach (Transform t in scoreboardPanel.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        scoreboard.Sort();

        foreach (PersonScore person in scoreboard)
        {
            GameObject go = (GameObject)Instantiate(playerRowPrefab);
            go.transform.SetParent(scoreboardPanel.transform);

            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = person.personName;
            go.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = person.score.ToString();
        }

        SaveValuesOnJSON();
    }

    public void AddNewScore(string newName, float newScore)
    {
        scoreboard.Add(new PersonScore(newName, newScore));
        scoreboard.Sort();
        UpdateText();
    }

    public void SaveValuesOnJSON()
    {
        string json = "";

        foreach (PersonScore ps in scoreboard)
        {
            Debug.Log(JsonUtility.ToJson(ps));
            json += JsonUtility.ToJson(ps);
        }

        //string json = JsonUtility.ToJson(scoreboard, true);
        Debug.Log(json);
    }
}

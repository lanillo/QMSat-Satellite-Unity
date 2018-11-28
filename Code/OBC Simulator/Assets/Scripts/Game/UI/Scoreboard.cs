using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class Scoreboard : MonoBehaviour {

    public GameObject playerRowPrefab;
    public GameObject scoreboardPanel;

    PersonScoreCollection scoreboard = new PersonScoreCollection();

    public void UpdateText()
    {
        foreach (Transform t in scoreboardPanel.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        ReadValuesFromJSON();

        foreach (PersonScore person in scoreboard.persons)
        {
            GameObject go = (GameObject)Instantiate(playerRowPrefab);
            go.transform.SetParent(scoreboardPanel.transform);

            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = person.personName;
            go.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = person.score.ToString();
        }
    }

    public void AddNewScore(string newName, float newScore)
    {
        scoreboard.persons.Add(new PersonScore(newName, newScore));
        scoreboard.persons.Sort();
        WriteValuesToJSON();
        UpdateText();
    }

    public void WriteValuesToJSON()
    {
        string path = Application.dataPath + "/Ressources/scoreboard.json";
        string str = JsonUtility.ToJson(scoreboard);
        File.WriteAllText(path, str);
    }

    public void ReadValuesFromJSON()
    {
        string path = Application.dataPath + "/Ressources/scoreboard.json";

        if (!File.Exists(path))
        {
            Directory.CreateDirectory(Application.dataPath + "/Ressources");
            File.Create(Application.dataPath + "/Ressources/scoreboard.json");
            File.WriteAllText(Application.dataPath + "/Ressources/scoreboard.json", JsonUtility.ToJson(scoreboard));
        }

        string json = File.ReadAllText(path);
        if (json != null)
            JsonUtility.FromJsonOverwrite(json, scoreboard);
    }
}

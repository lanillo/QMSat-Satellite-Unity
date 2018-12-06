using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Display GameOverUI when loosing condition happen */
public class GameManager : MonoBehaviour {

    [Header("Unity UI Objects")]
    public GameObject gameUI;
    public GameObject gameOverUI;
    public Text reasonText;
    public InputField newName;
    public Button button;

    /* For Script accessing */
    private SatelliteStats satelliteStats;
    private AntennaDeployer antennaDeployer;
    private PayloadDeployer payloadDeployer;
    private Scoreboard scoreboard;

    public static bool gameOver = false;
    public static bool timeOver = false;

    public enum Reasons
    {
        NoBattery,
        PayloadBroke,
        AntennaBroke,
        PayloadOverheated,
        AntennaOverheated,
        BatteryOverheated,
        Radiation,
        MissionOver
    }

    private void Start()
    {
        satelliteStats = GetComponent<SatelliteStats>();
        antennaDeployer = GetComponent<AntennaDeployer>();
        payloadDeployer = GetComponent<PayloadDeployer>();
        scoreboard = GetComponent<Scoreboard>();

        gameOver = false;
        timeOver = false;
        gameOverUI.SetActive(false);
        gameUI.SetActive(true);
        button.interactable = true;
    }

    private void Update()
    {
        if (gameOver)
            return;

        if (satelliteStats.battery <= 0f)
            EndGame(Reasons.NoBattery);

        if (payloadDeployer.payloadBroke)
            EndGame(Reasons.PayloadBroke);

        if (antennaDeployer.antennaBroke)
            EndGame(Reasons.AntennaBroke);

        if (satelliteStats.payloadFailure)
            EndGame(Reasons.PayloadOverheated);

        if (satelliteStats.telecomFailure)
            EndGame(Reasons.AntennaOverheated);

        if (satelliteStats.batteryFailure)
            EndGame(Reasons.BatteryOverheated);

        if (timeOver)
            EndGame(Reasons.MissionOver);
    }

    void EndGame(Reasons reasons)
    {
        //Time.timeScale = 1f;

        scoreboard.UpdateText();

        gameOver = true;
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);

        switch (reasons)
        {
            case Reasons.NoBattery:
                reasonText.text = "Plus d'énergie, le satellite est mort !".Replace('é', 'e');
                break;
            case Reasons.PayloadBroke:
                reasonText.text = "Le magnetometre s'est détachée du satellite !".Replace('é', 'e');
                break;
            case Reasons.AntennaBroke:
                reasonText.text = "Les antennes ne se sont pas deployées !".Replace('é', 'e');
                break;
            case Reasons.PayloadOverheated:
                reasonText.text = "La température a brisé le magnetometre !".Replace('é', 'e');
                break;
            case Reasons.AntennaOverheated:
                reasonText.text = "La température a brisé les antennes !".Replace('é', 'e');
                break;
            case Reasons.Radiation:
                reasonText.text = "La radiation a pertubé le magnetometre !".Replace('é', 'e');
                break;
            case Reasons.BatteryOverheated:
                reasonText.text = "La temperature a brisé les batteries !".Replace('é', 'e');
                break;
            case Reasons.MissionOver:
                reasonText.text = "La mission est finie. Bravo !".Replace('é', 'e');
                break;
            default:
                reasonText.text = "Ce n'est définitivement pas ton jour !".Replace('é', 'e');
                break;
        }
    }

    public void GetInputText()
    {
        scoreboard.AddNewScore(newName.text, SatelliteStats.playerScore);
        button.interactable = false;
    }
}

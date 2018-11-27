using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public enum Reasons
    {
        NoBattery,
        PayloadBroke,
        AntennaBroke,
        PayloadOverheated,
        AntennaOverheated,
        BatteryOverheated,
        Radiation
    }

    private void Start()
    {
        satelliteStats = GetComponent<SatelliteStats>();
        antennaDeployer = GetComponent<AntennaDeployer>();
        payloadDeployer = GetComponent<PayloadDeployer>();
        scoreboard = GetComponent<Scoreboard>();

        gameOver = false;
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

        if (satelliteStats.alimFailure)
            EndGame(Reasons.PayloadOverheated);

        if (satelliteStats.telecomFailure)
            EndGame(Reasons.AntennaOverheated);

        if (satelliteStats.alimFailure)
            EndGame(Reasons.BatteryOverheated);
    }

    void EndGame(Reasons reasons)
    {
        Time.timeScale = 1f;

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
                reasonText.text = "La charge utile s'est détachée du satellite !".Replace('é', 'e');
                break;
            case Reasons.AntennaBroke:
                reasonText.text = "Les antennes ne se sont pas deployées !".Replace('é', 'e');
                break;
            case Reasons.PayloadOverheated:
                reasonText.text = "Les composantes électriques de la charge utile ont brisé dû à la température !".Replace('é', 'e');
                break;
            case Reasons.AntennaOverheated:
                reasonText.text = "Les composantes électriques du circuit de transmission ont brisé dû à la température !".Replace('é', 'e');
                break;
            case Reasons.Radiation:
                reasonText.text = "La radiation a brisé les composantes électriques !".Replace('é', 'e');
                break;
            case Reasons.BatteryOverheated:
                reasonText.text = "Les batteries ont surchauffé et brisé !".Replace('é', 'e');
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

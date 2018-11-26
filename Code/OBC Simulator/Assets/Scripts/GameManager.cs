using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Unity UI Objects")]
    public GameObject gameUI;
    public GameObject gameOverUI;
    public Text reasonText;

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
        gameOver = false;
        gameOverUI.SetActive(false);
        gameUI.SetActive(true);
    }

    private void Update()
    {
        if (SatelliteStats.battery <= 0f)
            EndGame(Reasons.NoBattery);

        if (PayloadDeployer.payloadBroke)
            EndGame(Reasons.PayloadBroke);

        if (AntennaDeployer.antennaBroke)
            EndGame(Reasons.AntennaBroke);

        if (SatelliteStats.alimFailure)
            EndGame(Reasons.PayloadOverheated);

        if (SatelliteStats.telecomFailure)
            EndGame(Reasons.AntennaOverheated);

        if (SatelliteStats.alimFailure)
            EndGame(Reasons.BatteryOverheated);
    }

    void EndGame(Reasons reasons)
    {
        gameOver = true;
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);

        switch (reasons)
        {
            case Reasons.NoBattery:
                reasonText.text = "Plus d'énergie, le satellite est mort !";
                break;
            case Reasons.PayloadBroke:
                reasonText.text = "La charge utile s'est détachée du satellite !";
                break;
            case Reasons.AntennaBroke:
                reasonText.text = "Les antennes ne se sont pas deployées !";
                break;
            case Reasons.PayloadOverheated:
                reasonText.text = "Les composantes électriques de la charge utile ont brisé dû à la chaleur !";
                break;
            case Reasons.AntennaOverheated:
                reasonText.text = "Les composantes électriques du circuit de transmission ont brisé dû à la chaleur !";
                break;
            case Reasons.Radiation:
                reasonText.text = "La radiation a brisé les composantes électriques !";
                break;
            case Reasons.BatteryOverheated:
                reasonText.text = "Les batteries ont surchauffé et brisé !";
                break;
            default:
                reasonText.text = "Ce n'est définitivement pas ton jour !";
                break;
        }

    }
}

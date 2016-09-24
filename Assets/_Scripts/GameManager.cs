using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public Image fuelImage, shieldImage;
    public Text distanceText, currencyText, gameoverScoreText, highscoreText;
    public float distancePerSecond = 2, fuelPerSecond = 2;
    public GameObject pauseThing, gameoverThing;
    float maxFuel = 40, maxShield = 40, fuel = 40, shield = 40, shieldUnit = 10, distance = 0;
    int score = 0, currency = 0, currencyValue = 100;
    bool isPause = false, isGameActive = false;
    UserClass usrDat;
    List<ShipClass> shipList;
    string usrPath, shipPath, shipFirstTime;

    void Awake()
    {
        usrPath = Application.persistentDataPath + "/usr.lzmonster";
    }

	void Start ()
    {
        LoadProgress();
        GameReset();
	}
	
	void Update ()
    {
        if (isGameActive)
        {
            distance += Time.deltaTime * distancePerSecond;
            distanceText.text = distance.ToString("0.0");
            fuel -= Time.deltaTime * fuelPerSecond;
            fuelImage.fillAmount = fuel / maxFuel;
            if (fuel <= 0)
                GameOver();
            if (Input.GetKeyDown(KeyCode.Escape))
                ClickTogglePause();
        }
	}

    void GameOver()
    {
        return;
        isGameActive = false;
        score = (int)distance + currency * currencyValue;
        usrDat.coins += (int)currency;
        if (usrDat.score < score)
            usrDat.score = score;
        SaveProgress();
        gameoverScoreText.text = "Currency : +" + currency.ToString() + "x" + currencyValue.ToString() + "\nDistance : +" + Mathf.RoundToInt(distance).ToString() + "\nScore : " + score.ToString();
        highscoreText.text = "Highscore : " + usrDat.score;
        gameoverThing.SetActive(true);
        Time.timeScale = 0.15f;
        player.gameObject.SetActive(false);
    }

    public void ClickTogglePause()
    {
        if (!isGameActive)
            return;
        if (isPause)
        {
            pauseThing.SetActive(false);
            isPause = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            pauseThing.SetActive(true);
            isPause = true;
            Time.timeScale = 0.0f;
        }
    }

    public void ClickRestart()
    {
        GameReset();
    }

    public void ClickQuit()
    {

    }

    public void HitCurrency()
    {
        currency += 1;
        currencyText.text = currency.ToString("0");
    }

    public void HitObstacle()
    {
        shield -= shieldUnit;
        shieldImage.fillAmount = shield / maxShield;
        if (shield < 0)
            GameOver();
    }

    public void HitFuel()
    {
        fuel = maxFuel;
        fuelImage.fillAmount = 1f;
    }

    public void HitShield()
    {
        shield = (shield < maxShield) ? (shield + shieldUnit) : maxShield;
        shieldImage.fillAmount = shield / maxShield;
    }

    void GameReset()
    {
        shield = maxShield;
        fuel = maxFuel;
        shieldImage.fillAmount = shield / maxShield;
        fuelImage.fillAmount = fuel / maxFuel;
        distance = 0;
        currency = 0;
        currencyText.text = "0";
        player.gameObject.SetActive(false);
        player.Reset();
        player.gameObject.SetActive(true);
        isGameActive = true;
        isPause = false;
        pauseThing.SetActive(false);
        gameoverThing.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void LoadProgress()
    {
        usrDat = new UserClass();
        if (File.Exists(usrPath))
        {
            using (Stream s = File.Open(usrPath, FileMode.Open))
            {
                BinaryFormatter b = new BinaryFormatter();
                usrDat = (UserClass)b.Deserialize(s);
            }
        }
        string raw;
        if (!File.Exists(shipPath))
        {
            raw = File.ReadAllText(shipFirstTime);
            File.WriteAllText(shipPath, raw);
        }
        else
            raw = File.ReadAllText(shipPath);
        var t = new JSONObject(raw);
        var l = t.list;
        if (shipList == null)
            shipList = new List<ShipClass>();
        foreach (var u in l)
            shipList.Add(JsonUtility.FromJson<ShipClass>(u.ToString()));
    }

    void SaveProgress()
    {
        if (File.Exists(usrPath))
            File.Delete(usrPath);
        using (Stream s = File.Open(usrPath, FileMode.Create))
        {
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(s, usrDat);
        }
    }
}

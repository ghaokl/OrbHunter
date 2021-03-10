using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public TextMeshProUGUI OrbText, TimeText, DeathText, GameOverText;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public static void UpdateOrbUI(int orbCount)
    {
        _instance.OrbText.text = orbCount.ToString();
    }

    public static void UpdateDeathUI(int deathCount)
    {
        _instance.DeathText.text = deathCount.ToString();
    }

    public static void UpdateGameTimeUI(float time)
    {
        int minutes = (int)(time / 60);
        float sceonds = time % 60;

        _instance.TimeText.text = minutes.ToString("00") + ":" + sceonds.ToString("00");
    }

    public static void DisplayGameOver()
    {
        _instance.GameOverText.enabled = true;
    }

    public static void HideGameOver()
    {
        _instance.GameOverText.enabled = false;
    }
}

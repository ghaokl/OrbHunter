using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private SceneFader _sceneFader;
    private List<Orb> _orbsList;
    private Door _lockedDoor;

    private float _gameTime;
    private bool _gameIsOver = false;

    private  int _deathNum;

    private DeathPos[] _deathPosList;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
       // Debug.Log(SceneManager.sceneCount);
        _orbsList =new List<Orb>();

        DontDestroyOnLoad(this);
    }

    public static void REgisterDoor(Door door)
    {
        _instance._lockedDoor = door;
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        _instance._sceneFader = obj;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (_instance == null) return;

        if (!_instance._orbsList.Contains(orb))
        {
            _instance._orbsList.Add(orb);
        }

        UIManager.UpdateOrbUI(_instance._orbsList.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!_instance._orbsList.Contains(orb)) return;
        _instance._orbsList.Remove(orb);

        if(_instance._orbsList.Count==0)
            _instance._lockedDoor.Open();
        UIManager.UpdateOrbUI(_instance._orbsList.Count);
    }

    public static void PlayerDied()
    {
        _instance._sceneFader.FadeOut();
        _instance._deathNum++;
        UIManager.UpdateDeathUI(_instance._deathNum);
        _instance.Invoke("RestartScene", 1.0f);

    }

    void RestartScene()
    {
        _instance._orbsList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void PlayerWin()
    {
        _instance._gameIsOver = true;
        UIManager.DisplayGameOver();

        AudioManager.PlayerWinAudio();
       
        StartNextLevel();      
    }

    static void StartNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      
        //if (SceneManager.GetActiveScene().buildIndex != 4)
        //{
            InitData();
       // }     
    }

    static void InitData()
    {
        _instance._gameIsOver = false;
        _instance._gameTime = 0;
        _instance._deathNum = 0;

        UIManager.UpdateOrbUI(_instance._orbsList.Count);
        UIManager.UpdateDeathUI(_instance._deathNum);
        UIManager.HideGameOver();
        ClearDeathPos();
    }

    static void ClearDeathPos()
    {
        _instance._deathPosList = GameObject.FindObjectsOfType<DeathPos>();
        for (int i = 0; i < _instance._deathPosList.Length; i++)
        {
            Destroy(_instance._deathPosList[i].gameObject);
        }
    }
    public static bool GameOver()
    {
        return _instance._gameIsOver;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (_gameIsOver) return;
        _gameTime += Time.deltaTime;
        UIManager.UpdateGameTimeUI(_gameTime);
    }


}

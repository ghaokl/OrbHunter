using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneZoon : MonoBehaviour
{
    private int _playerLayer;
    public bool IsLeft;
    public bool IsRight;
    // Start is called before the first frame update
    void Start()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerLayer)
        {
            if (IsLeft)
            {
                SceneManager.LoadScene("GameMainSceneLevel1");
            }
            if (IsRight)
            {
                SceneManager.LoadScene("GameMainSceneLevel1");
            }
        }

          


    }
}

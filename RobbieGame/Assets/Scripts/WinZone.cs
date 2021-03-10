using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private int _playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerLayer)
          
        GameManager.PlayerWin();


    }


}

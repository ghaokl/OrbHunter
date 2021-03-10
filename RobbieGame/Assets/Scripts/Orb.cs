using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private int _player;
    public GameObject ExploseVFXPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _player = LayerMask.NameToLayer("Player");

        GameManager.RegisterOrb(this);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _player)
        {
            Instantiate(ExploseVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);

            AudioManager.PlayOrbAudio();
            GameManager.PlayerGrabbedOrb(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

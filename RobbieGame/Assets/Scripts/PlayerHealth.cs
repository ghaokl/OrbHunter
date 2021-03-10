using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public GameObject DeathVFXPrefab;
    public GameObject DeathPosPrefab;

    private int _trapsLayer;
    // Start is called before the first frame update
    void Start()
    {
        _trapsLayer = LayerMask.NameToLayer("Traps");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _trapsLayer)
        {
            Instantiate(DeathVFXPrefab, transform.position, transform.rotation);
            Instantiate(DeathPosPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(-45, 90)));

            gameObject.SetActive(false);
            AudioManager.PlayDeathAudio();

            GameManager.PlayerDied();           
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

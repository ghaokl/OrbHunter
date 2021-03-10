using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbGameDegreeSet : MonoBehaviour
{
    private int _player;
    public GameObject ExploseVFXPrefab;
    public GameObject LeftDoor;
    public GameObject RightDoor;

    private StartSceneDoor _leftDoor;
    private StartSceneDoor _rightDoor;

    public bool IsLeft;
    public bool IsRight;

    // Start is called before the first frame update
    void Start()
    {
        _player = LayerMask.NameToLayer("Player");
        _leftDoor = LeftDoor.GetComponent<StartSceneDoor>();
        _rightDoor = RightDoor.GetComponent<StartSceneDoor>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _player)
        {
            Instantiate(ExploseVFXPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);

            AudioManager.PlayOrbAudio();
            if (IsLeft)
            {
                _leftDoor.Open();
            }
            if (IsRight)
            {
                _rightDoor.Open();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}

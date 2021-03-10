using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private Animator _anim;
    private int _openID;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _openID = Animator.StringToHash("Open");
        GameManager.REgisterDoor(this);
    }

    public void Open()
    {
        _anim.SetTrigger(_openID);

        AudioManager.PlayDoorOpenAudio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

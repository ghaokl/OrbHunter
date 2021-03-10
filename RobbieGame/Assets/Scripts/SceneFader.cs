using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    private Animator _anim;
    private int _faderID;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _faderID = Animator.StringToHash("Fade");

        GameManager.RegisterSceneFader(this);
    }

    public void FadeOut()
    {
        _anim.SetTrigger(_faderID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

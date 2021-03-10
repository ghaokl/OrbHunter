using UnityEngine;

public class AutoSpikes : MonoBehaviour
{
	public float ActiveDuration = 2f;

	Animator _anim;
	AudioSource _audioSource;
	int _activeParamID = Animator.StringToHash("Active");
	float _deactivationTime;
	bool _playerInRange;
	bool _trapActive;
	int _playerLayer;

	void Start ()
	{
		_playerLayer = LayerMask.NameToLayer("Player");

		_anim = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
	}
	
	void Update ()
	{
		if (_trapActive && !_playerInRange && Time.time >= _deactivationTime)
		{
			_trapActive = false;
			_anim.SetBool(_activeParamID, false);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == _playerLayer)
		{
			_playerInRange = true;
			_trapActive = true;
			_anim.SetBool(_activeParamID, true);
			_audioSource.Play();
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == _playerLayer)
		{
			_playerInRange = false;
			_deactivationTime = Time.time + ActiveDuration;
		}
	}
}

using UnityEngine;

public class FallingBlock : MonoBehaviour
{
	public FallingBlockCollision Block;

	Animator _anim;
	BoxCollider2D _box;
	AudioSource _audioSource;
	int _playerLayer;
	int _fallParamID;


	void Start()
	{
		_anim = GetComponent<Animator>();
		_box = GetComponent<BoxCollider2D>();
		_audioSource = GetComponent<AudioSource>();

		_playerLayer = LayerMask.NameToLayer("Player");
		_fallParamID = Animator.StringToHash("Activate");
	}

	public void Fall()
	{
		Block.Fall();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer != _playerLayer)
			return;

		_box.enabled = false;
		_audioSource.Play();
		_anim.SetTrigger(_fallParamID);
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITooltip
{
    public float ShootCooldown = 3.0f, AmmoSpeed = 5.0f;

    [Header("Objects")]
    [SerializeField]
    private AudioClip _shootSound;

    public EnemySt EnemyStruct = new EnemySt();

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _ammoPrefab;

    [SerializeField]
    private Transform _shootPosition;

    [SerializeField]
    private LineRenderer _lineRenderer;


    [SerializeField]
    private Animator _animator;

    private CooldownManager _cooldownManager = new CooldownManager();


    void Update()
    {
        var hit = Physics2D.Raycast(_shootPosition.position, transform.localScale.x > 0 ? Vector2.right : Vector2.left);

        if (hit)
        {
            if (EnemyStruct.isEnemy && hit.transform.gameObject.CompareTag("Player"))
            {
                Shoot(hit);

                SetLineRendererColor(Color.red);
            }
            else
            {
                SetLineRendererColor(Color.white);
            }

            _lineRenderer.SetPosition(1, Vector3.Lerp(_lineRenderer.GetPosition(1), new Vector3(Mathf.Abs(hit.point.x - _lineRenderer.transform.position.x), Mathf.Abs(hit.point.y - _lineRenderer.transform.position.y), 0), Time.deltaTime * 10.0f));
        }
        else
        {
            SetLineRendererColor(Color.white);

            _lineRenderer.SetPosition(1, Vector3.Lerp(_lineRenderer.GetPosition(1), Vector2.right * 1000.0f, Time.deltaTime * 5.0f));
        }
    }

    void SetLineRendererColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    IEnumerator ShootEffect()
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();

        Vector3[] positions = new Vector3[2];

        _lineRenderer.GetPositions(positions);

        float distance = Vector3.Distance(positions[0], positions[1]);

        Debug.Log("distance: " + distance);

        while (distance > 0.01f)
        {
            _lineRenderer.SetPosition(0, Vector3.MoveTowards(positions[0], positions[1], Time.deltaTime * 5.0f));  

            _lineRenderer.GetPositions(positions);

            distance = Vector3.Distance(positions[0], positions[1]);

            yield return waitForEndOfFrame;
        }

        _lineRenderer.SetPosition(0, Vector3.zero);

        _lineRenderer.SetPosition(1, Vector3.zero);
    }

    void Shoot(RaycastHit2D hit)
    {
        if (_cooldownManager.IsInCooldown("shoot"))
            return;

        _cooldownManager.SetCooldown("shoot", ShootCooldown);

        _animator.SetTrigger("Shoot");

        StartCoroutine(ShootEffect());

        _audioSource.PlayOneShot(_shootSound);
    }

    public void ShootTrigger()
    {
        var ammo = Instantiate(_ammoPrefab, _shootPosition.position, _shootPosition.rotation)
                            .GetComponent<Ammo>();

        ammo.Shoot(transform.localScale.x > 0 ? Vector2.right : Vector2.left);
    }

    public void OnHover()
    {
        TooltipManager.Instance.ShowTooltip(EnemyStruct.isEnemy ? "KORUMALI ADRES" : "HACKED");
    }

    private bool _playingMinigameCurrently = false, _winBefore = false;

    public void OnClick()
    {
        if (_winBefore || _playingMinigameCurrently)
            return;

        _playingMinigameCurrently = true;

        MinigameTwo.CreateMinigame(() =>
        {
            if (ConsolePanel.Instance != null)
            {
                ConsolePanel.Instance.AddVariable("enemy", EnemyStruct, null);

                _winBefore = true;
            }

            _playingMinigameCurrently = false;

            Debug.Log("plr won");
        },
        () =>
        {
            _playingMinigameCurrently = false;

            Debug.Log("plr lost");
        });
    }
}

[Serializable]
public class EnemySt
{
	public bool _isEnemy = true;

	[Call]
	public bool isEnemy
	{
		get
		{
			return _isEnemy;
		}

		set
		{
			Debug.Log("set isEnemy called!");

            _isEnemy = value;

            if (!value)
            {
                Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Enemy").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
            }
		}
	}

}
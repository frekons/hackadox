using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float ShootCooldown = 3.0f, AmmoSpeed = 5.0f;

    public EnemySt EnemyStruct = new EnemySt();

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
        var hit = Physics2D.Raycast(_shootPosition.position, _shootPosition.right);

        if (hit != default && hit.transform != default)
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

            _lineRenderer.SetPosition(1, Vector3.Lerp(_lineRenderer.GetPosition(1), new Vector3(hit.point.x, hit.point.y, 0) - _lineRenderer.transform.position, Time.deltaTime * 10.0f));
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
    }

    public void ShootTrigger()
    {
        var ammo = Instantiate(_ammoPrefab, _shootPosition.position, _shootPosition.rotation)
                            .GetComponent<Ammo>();

        ammo.Shoot(_shootPosition.rotation * Vector2.right);
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
		}
	}

}
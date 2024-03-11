﻿using UnityEngine;
using Managers;
using Monsters;
using static Models.GameSettings;

public class Monster : MonoBehaviour, IDamageable, IMovable {
	public bool IsAlive { get; private set; }
	public Transform Transform => transform;

	private MonsterSettings m_monsterSettings;
	
	private float m_currentHp;
	private float m_speed;
	
	private void Awake() {
		MonsterPoolManager.Instance.AddMonster(this);
	}

	private void OnDestroy() {
		MonsterPoolManager.Instance.RemoveMonster(this);
	}
	
	public void Init(MonsterSettings monsterSettings) {
		m_monsterSettings = monsterSettings;
		IsAlive = true;
		gameObject.SetActive(true);
		InitializeParameters();
	}

	private void InitializeParameters() {
		m_currentHp = m_monsterSettings.m_maxHP;
		m_speed = m_monsterSettings.m_speed;
	}

	public void ApplyDamage(float damage) {
		m_currentHp -= damage;
		if (m_currentHp <= 0) {
			Kill();
		}
	}

	public void Kill() {
		IsAlive = false;
		gameObject.SetActive(false);
	}

	public void Move(Transform target) {
		var translation = target.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);	
	}
}

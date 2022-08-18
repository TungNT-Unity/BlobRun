using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform projectilePrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private float delayWave = 0f;
    [SerializeField] private int projectileNumber = 1;
    [SerializeField] private float minAngel = 0f;
    [SerializeField] private float maxAngle = 0f;

    public void Fire()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            float angle = maxAngle - minAngel;
            float angleStep = angle / projectileNumber;
            for (int j = 0; j < projectileNumber; j++)
            {
                Transform cloneProjectile = Instantiate(projectilePrefabs);
                cloneProjectile.position = firePoint.position;
                cloneProjectile.rotation = Quaternion.AngleAxis(minAngel + angleStep * j, Vector3.up);
                cloneProjectile.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(delayWave);
        }
    }
}

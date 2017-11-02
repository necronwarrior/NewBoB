using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPlane : MonoBehaviour {

    public float health = 100.0f;
    public float damagePerSecond = 20.0f;
    private float initialHealth;


    private GeneralPlane adversary;


    private UnityEngine.Object explosionPrefab;
    

    protected void Awake() {
        explosionPrefab = Resources.Load("Prefabs/Explosion");

    }

    // Use this for initialization
    protected void Start()
    {
        adversary = null;
        initialHealth = health;

    }

    // Update is called once per frame
    protected void Update()
    {
        if (adversary != null) {
            DealDamage();
        }
    }

    public void TakeDamage(float damage)
    {

        this.health -= damage;
        if (this.health <= 0.0f)
        {
            Die();
        }
    }

    public void StartDealingDamage(GeneralPlane otherPlane) {
        adversary = otherPlane;
    }

    public void StopDealingDamage() {
        adversary = null;
    }

    public void Repair() {
        this.health = initialHealth;
    }



    void DealDamage()
    {

        adversary.TakeDamage(damagePerSecond * Time.deltaTime);

    }


    void Die()
    {

        GameObject explosion = (GameObject)Instantiate(explosionPrefab);

        explosion.transform.position = transform.position;

        Destroy(gameObject, 0.2f);

    }

}

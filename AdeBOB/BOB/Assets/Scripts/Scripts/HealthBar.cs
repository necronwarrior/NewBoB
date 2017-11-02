using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{


    public bool displayWhenFull;
    public bool destroyWhenZero;
    public GameObject healthBarBack;
    public GameObject healthBar;
    public float currentHealth = 100;
    public float desiredHealth;
    public float fullHealth;



    // Use this for initialization
    void Start()
    {
        desiredHealth = currentHealth;
        fullHealth = desiredHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        DisplayHealth();

    }

    //takeing damage function 
    public void TakeDamage(float damage)
    {
        desiredHealth -= damage;
        //Debug.Log("health updated");
        if (desiredHealth < 0)
        {
            //Debug.Log("tried to destroy");
            if (destroyWhenZero)
            {
                //Destory Object
                Destroy(gameObject);
                Destroy(healthBar.transform.parent.gameObject);
            }
        }
    }


    //updates the visual of the health
    void UpdateHealthBar()
    {
        if (desiredHealth != currentHealth)
        {
            currentHealth = Mathf.Lerp(currentHealth, desiredHealth, Time.deltaTime * 3);
        }
        //change scale of health
        ScaleHealth();

    }

    //displays the health bar if it should
    void DisplayHealth()
    {
        if (!displayWhenFull)
        {
            //if health still full
            if (currentHealth == fullHealth)
            {
                healthBar.SetActive(false);
                healthBarBack.SetActive(false);
            }
            else
            {
                healthBar.SetActive(true);
                healthBarBack.SetActive(true);
                displayWhenFull = true;
            }
        }


    }

    void ScaleHealth()
    {
        float scaledHealth = currentHealth / fullHealth;
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(scaledHealth, 1, 1);
    }
}
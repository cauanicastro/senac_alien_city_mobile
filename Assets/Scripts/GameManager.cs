using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Math;

[System.Serializable]
public class GameManager
{
    [Header("HUD")]
    public HudController hudController;

    [Header("Player props")]
    public GameObject player;
    public PlayerController playerController;
    public Transform playerPos; 
    public float playerHealth = 100;
    public float playerMaxHealth = 100;
    public float playerLives = 3;
    public float playerGems = 0;

    [Header("Stage 1 props")]

    private static GameManager instance;

    private GameManager() {}

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }

    public float UpdateHealthbar(float value)
    {
        hudController.UpdateHealthBar(value);
        return value;
    }

    public void DealDamage(float amount)
    {
        playerHealth = UpdateHealthbar(Max(playerHealth - amount, 0));
        if (playerHealth == 0)
        {
            LifeDecrease();
        }
    }

    public void DealRecover(float amount)
    {
        playerHealth = UpdateHealthbar(Min(playerHealth + amount, playerMaxHealth));
    }

    public void LifeIncrease()
    {
        ++playerLives;
    }

    private void LifeDecrease()
    {
        --playerLives;
        //TODO: check if lives are under 0
        playerController.StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        playerController.isDead = true;
        player.GetComponent<Animator>().SetTrigger("Morre");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        hudController.RemoveLife();

        playerHealth = playerMaxHealth;
        playerGems = 0;
    }

    private void GameOver()
    {
        //TODO: implement gameover logic
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public Transform healthBar;

    private const float healthMinValue = 0;
    private const float healthbarMinValue = 0;
    private const float healthbarMaxValue = 1;
    private GameManager gm;

    private Stack<GameObject> livesStack;
    public float maxLives;
    public GameObject lifeToSpawn;
    public Vector3 livesPosition;

    public Text gemsCount;

    public Vector3 keysPosition;


    private void Start()
    {
        gm = GameManager.GetInstance();
        gm.hudController = this;

        livesStack = new Stack<GameObject>();
        for (int count = 0; count < gm.playerLives; count++)
        {
            AddLife();
        }
    }

    private void Update()
    {
        gemsCount.text = gm.playerGems.ToString();
    }

    public void RenderLives()
    {
        livesStack.Clear();
    }

    public void RemoveLife()
    {
        if (livesStack.Count > 0)
        {
            Destroy(livesStack.Pop());
        }
    }

    public void AddLife()
    {
        if (livesStack.Count < maxLives)
        {
            Vector3 pos = GetNextLifePosition();
            GameObject life = Instantiate(lifeToSpawn);
            life.transform.SetParent(transform, false);
            life.GetComponent<RectTransform>().localPosition = pos;
            livesStack.Push(life);
        }
    }

    public Vector3 GetNextLifePosition()
    {
        if (livesStack.Count == 0)
        {
            return livesPosition;
        }
        RectTransform lastLife = livesStack.Peek().GetComponent<RectTransform>();
        return new Vector3(lastLife.localPosition.x + lastLife.rect.width + 1, lastLife.localPosition.y, 0);
    }

    public void UpdateHealthBar(float newValue)
    {
        healthBar.localScale = new Vector3(Normalize(newValue), transform.localScale.y, transform.localScale.z);
    }



    private float Normalize(float value)
    {
        //Max health of user can change later on
        return (healthbarMaxValue - healthbarMinValue) / (gm.playerMaxHealth - healthMinValue) * (value - gm.playerMaxHealth) + healthbarMaxValue;
    }
}

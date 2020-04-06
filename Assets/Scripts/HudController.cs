using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    private GameManager gm;

    private Stack<GameObject> livesStack;
    public float maxLives;
    public GameObject lifeToSpawn;
    public Vector3 livesPosition;

    public Text gemsCount;

    private Stack<GameObject> keysStack;
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

        keysStack = new Stack<GameObject>();
        gm.playerKeys.ForEach(key => AddKey(key));
    }

    private void Update()
    {
        if (gemsCount)
        {
            gemsCount.text = gm.playerGems.ToString();
        }
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
            Vector2 defaults = new Vector2(0, 1);
            GameObject life = Instantiate(lifeToSpawn);
            RectTransform lifeRect = life.GetComponent<RectTransform>();
            life.transform.SetParent(transform, false);
            lifeRect.anchorMin = defaults;
            lifeRect.anchorMax = defaults;
            lifeRect.pivot = defaults;
            lifeRect.anchoredPosition = pos;
            livesStack.Push(life);
        }
    }

    public KeyScOb AddKey(KeyScOb keyScOb)
    {
        Vector3 pos = GetNextKeyPosition();
        GameObject key = Instantiate(keyScOb.objImage);
        key.transform.SetParent(transform, false);
        key.GetComponent<RectTransform>().localPosition = pos;
        keysStack.Push(key);
        return keyScOb;
    }

    public Vector3 GetNextLifePosition()
    {
        if (livesStack.Count == 0)
        {
            return livesPosition;
        }
        RectTransform lastLife = livesStack.Peek().GetComponent<RectTransform>();
        return new Vector3(lastLife.anchoredPosition.x + lastLife.rect.width + 1, lastLife.anchoredPosition.y, 0);
    }

    public Vector3 GetNextKeyPosition()
    {
        if (keysStack.Count == 0)
        {
            return keysPosition;
        }
        RectTransform lastKey = keysStack.Peek().GetComponent<RectTransform>();
        return new Vector3(lastKey.localPosition.x - (lastKey.rect.width / 2), lastKey.localPosition.y, 0);
    }
}

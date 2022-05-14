using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public Controller cl;

    public GameObject[] enemies;

    public float speed;
    public float frequency;

    public List<GameObject> enemiesInScene = new List<GameObject>();

    private float dirX;
    private float dirY;

    public TextMeshProUGUI scoreText;

    private int score;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void Update()
    {
        if(enemiesInScene.Count != 0)
        {
            Vector2 dir = enemiesInScene[0].GetComponent<Rigidbody2D>().velocity;
            Vector2 pos = enemiesInScene[0].transform.position;
            if (Mathf.Abs(Mathf.Abs(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - Mathf.Abs(Mathf.Atan2(-pos.y, -pos.x) * Mathf.Rad2Deg)) > 0.1f)
            {
                if (cl.currentShape == enemiesInScene[0].name)
                {
                    StartCoroutine(ScaleDown(enemiesInScene[0]));
                    score += 1000;
                    scoreText.text = score.ToString();
                }
                StartCoroutine(FadeOut(enemiesInScene[0]));
                enemiesInScene.RemoveAt(0);
            }
        }
    }

    public IEnumerator SpawnEnemy()
    {
        int randEnemy = Random.Range(0, enemies.Length);
        GameObject curEnemy = Instantiate(enemies[randEnemy]);
        curEnemy.name = enemies[randEnemy].name;
        dirX = Random.Range(-1.0f,1.0f);
        if(Random.Range(0,2) == 1)
        {
            dirY = 1 - Mathf.Abs(dirX);
        }
        else
        {
            dirY = -1 + Mathf.Abs(dirX);
        }
        Vector2 dir = new Vector2(dirX, dirY);
        curEnemy.transform.position = dir * 12;
        curEnemy.GetComponent<Rigidbody2D>().velocity = -dir * speed;
        enemiesInScene.Add(curEnemy);
        yield return new WaitForSeconds(frequency);
        StartCoroutine(SpawnEnemy());
    }

    public IEnumerator ScaleDown(GameObject enemy)
    {
        enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float elapsedTime = 0;
        Vector3 startScale = enemy.transform.localScale;
        while (elapsedTime < 0.4f)
        {
            enemy.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, (elapsedTime / 0.4f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(enemy);
    }

    public IEnumerator FadeOut(GameObject enemy)
    {
        float elapsedTime = 0;
        Color32 startColor = enemy.GetComponent<SpriteRenderer>().color;
        while (elapsedTime < 0.4f)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color32.Lerp(startColor, new Color32(startColor.r, startColor.g, startColor.b, 0), (elapsedTime / 0.4f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(enemy);
    }
}

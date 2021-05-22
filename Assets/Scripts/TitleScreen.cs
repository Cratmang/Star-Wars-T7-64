using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleScreen : MonoBehaviour
{
    public GameObject starPrefab;

    public float minStarSpeed, maxStarSpeed, starSpawnRate, spawnX, minSpawnY, maxSpawnY, minSpawnZ, maxSpawnZ, TCStart, TCEnd, TCStartTime, TCEndTime, TCFadeStartTime, TCFadeEndTime;
    float timer;

    public Text textCrawl;
    public Color textColor, fade;
    
    // Update is called once per frame
    void Update()
    {
        float z = Random.Range(minSpawnZ, maxSpawnZ);
        float x = spawnX * Mathf.Ceil(z/10);
        Vector3 vec = new Vector3(x, Random.Range(minSpawnY, maxSpawnY), z);
        TitleStar newStar = Instantiate(starPrefab, vec, new Quaternion(0,0,0,0)).GetComponent<TitleStar>();
        newStar.speed = new Vector3(Random.Range(minStarSpeed, maxStarSpeed), 0, 0);
        
        timer += Time.deltaTime;
        if (timer >= TCStartTime) {
            textCrawl.transform.localPosition = new Vector2(0, ((timer-TCStartTime)/(TCEndTime-TCStartTime))*(TCEnd-TCStart) + TCStart);
            if (timer >= TCFadeStartTime) {
                textCrawl.color = Color.Lerp(textColor, fade, (timer-TCFadeStartTime)/TCFadeEndTime);
            }
            
            if (timer >= TCEndTime) {
                timer = 0;
            }
        } else {
            textCrawl.transform.localPosition = new Vector2(0, TCStart);
            textCrawl.color = textColor;
        }
    }

    

}

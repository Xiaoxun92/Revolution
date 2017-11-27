using UnityEngine;

public class BlackCover : MonoBehaviour {

    public GameObject player;

    [Header("玩家火焰点燃后，逐渐照亮环境需要的时间")]
    public float fadeTime;
	
	void Update () {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        float alpha = sr.color.a;
		if (player.GetComponent<Player>().burning)
            alpha -= Time.deltaTime / fadeTime;
        else
            alpha += Time.deltaTime / fadeTime;
        alpha = Mathf.Clamp01(alpha);
        sr.color = new Color(0, 0, 0, alpha);
    }
}

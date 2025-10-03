using UnityEngine;

public class WallDamageView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private Sprite[] crackSprites;
    [SerializeField] private ParticleSystem breakingEffect;
    [SerializeField] private ParticleSystem breakedEffect;
    [SerializeField] private ParticleSystemRenderer[] breakEffectRenderers;

    public Vector2Int Position { get; private set; } = new Vector2Int(-1, -1);

    private Texture2D breakEffectTex;

    public void Show(Vector2Int position, float addBreakingRate, Vector2 dir)
    {
        Vector3Int basePos = (Vector3Int)(position * MapExtension.TILE_SIZE);
        Vector3Int addPos = new Vector3Int(MapExtension.TILE_SIZE / 2, MapExtension.TILE_SIZE / 2);
        transform.position = basePos + addPos;
        
        int idx = Mathf.FloorToInt(Mathf.Lerp(0, crackSprites.Length, addBreakingRate));
        if(crackSprites.Length > idx)
        {
            rend.sprite = crackSprites[idx];
        }
        Position = position;

        breakingEffect.transform.eulerAngles = new Vector3(0, 0, dir.GetAim());
        breakingEffect.Play();
    }

    public void ShowBreakEffect()
    {
        breakedEffect.Play();
	}

    public void SetBreakEffectTex(Texture2D tex)
    {
        if(breakEffectTex != null)
        {
            return;
		}
        breakEffectTex = tex;
        foreach (var rend in breakEffectRenderers)
        {
            rend.material.SetTexture("_MainTex", tex);
		}
	}

    public void Active()
    {
        if (gameObject.activeInHierarchy) return;
        gameObject.SetActive(true);
        rend.gameObject.SetActive(true);
    }

    public void Inactive()
    {
        if (!gameObject.activeInHierarchy) return;
        gameObject.SetActive(false);
        Position = new Vector2Int(-1, -1);
        breakEffectTex = null;
	}

    public void HideCrack()
    {
        rend.gameObject.SetActive(false);
    }
}

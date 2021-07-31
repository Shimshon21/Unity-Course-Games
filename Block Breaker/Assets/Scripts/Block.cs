using UnityEngine;

public class Block : MonoBehaviour
{

    // Config params
    [SerializeField] AudioClip destroyedSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;

    // Cached reference
    Level level;

    // State varibles
    [SerializeField] uint timeHit = 0; // Serlize for debug porpuses only, remove later.


    const string BREAKABLE = "Breakable";

    private void Start()
    {
        CountBreakableBlocks();

    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();

        if (tag == BREAKABLE)
        {
            level.CountBlock();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == BREAKABLE)
        {
            HandleHit();

        }

    }

    // Handles ball hit breakable Block.
    private void HandleHit()
    {
        timeHit++;

        uint maxHits = (uint)hitSprites.Length + 1;

        if (timeHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
     
    }

    private void ShowNextHitSprite()
    {
        uint spriteIndex = timeHit - 1;

        if (hitSprites[spriteIndex])
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.Log("Block sprite is missing from array in" + gameObject.name);
        }
            
     }

    private void DestroyBlock()
    {
        AudioSource.PlayClipAtPoint(destroyedSound, Camera.main.transform.position);

        FindObjectOfType<GamesSession>().AddScore();

        level.DecBreakableBlocks();

        Destroy(gameObject);

        TriggerSparklesVFX();
    }


    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX,transform.position,transform.rotation);

        Destroy(sparkles, 2f);

    }
}

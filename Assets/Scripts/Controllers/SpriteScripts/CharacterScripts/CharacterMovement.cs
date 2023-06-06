using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : SpriteMovement
{
    public override void Start()
    {
        base.Start();
    }
    public void OnFootstep()
    {
        string key;
        Collider2D collider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y), 
            LayerMask.GetMask("Tilemap"), 0, 10);
        key = collider.name;
        AudioSource sound = AudioManager.GetSound(key + " Step");
        if (!sound.isPlaying)// || sound.time / sound.clip.length > 0.3f)
        {
            AudioManager.PlaySound(key + " Step", 0.97f, 1.03f);
        }
    }
}

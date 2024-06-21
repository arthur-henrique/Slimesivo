using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingConsumable : MonoBehaviour, IConsumable
{
    private bool _isConsumed = false;
    [SerializeField] private AudioClip[] healingSounds;
    public void Consume()
    {
        if (!_isConsumed)
        {
            _isConsumed = true;
            GameManager.instance.ConsumeHealing();
            SoundFXManager.Instance.PlayRandomSoundFXClip(healingSounds, transform, 1f);
            Destroy(gameObject);
        }
        
    }
}

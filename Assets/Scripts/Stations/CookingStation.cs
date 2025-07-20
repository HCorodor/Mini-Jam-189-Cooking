using UnityEngine;

public class CookingStation : Station
{
    [Header("Cooking SFX")]
    [SerializeField] private AudioClip _cookingLoopSound;
    private AudioSource _cookingLoopSource;

    private bool _wasPlayingLastFrame = false;

    private void Start()
    {
        if (_cookingLoopSound != null)
        {
            _cookingLoopSource = gameObject.AddComponent<AudioSource>();
            _cookingLoopSource.clip = _cookingLoopSound;
            _cookingLoopSource.loop = true;
            _cookingLoopSource.playOnAwake = false;
            _cookingLoopSource.volume = SoundManager.Instance != null ? SoundManager.Instance.SFXVolume : 1f;

            Debug.Log($"{gameObject.name} | Cooking AudioSource initialized with clip: {_cookingLoopSound.name} at volume {_cookingLoopSource.volume}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} | No cooking sound assigned!");
        }
    }

    public override void ManualUpdate()
    {
        base.ManualUpdate();

        if (currentState == StationState.Preparing)
        {
            if (!_cookingLoopSource.isPlaying)
            {
                _cookingLoopSource.pitch = Random.Range(0.95f, 1.05f);
                _cookingLoopSource.Play();
                Debug.Log($"{gameObject.name} | Started cooking loop. Pitch: {_cookingLoopSource.pitch}");
            }

            _wasPlayingLastFrame = true;
        }
        else if (_wasPlayingLastFrame)
        {
            if (_cookingLoopSource != null && _cookingLoopSource.isPlaying)
            {
                _cookingLoopSource.Stop();
                Debug.Log($"{gameObject.name} | Stopped cooking loop after preparation.");
            }

            _wasPlayingLastFrame = false;
        }
    }

    protected override void OnPreparationFinished()
    {
        base.OnPreparationFinished();
        if (_cookingLoopSource != null && _cookingLoopSource.isPlaying)
        {
            _cookingLoopSource.Stop();
        }
        Debug.Log("Cooking done!");
    }

    protected override bool RequiresPlayerNearby()
    {
        return false;
    }
}

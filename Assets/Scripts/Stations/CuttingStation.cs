using UnityEngine;

public class CuttingStation : Station
{
    [Header("Cutting SFX")]
    [SerializeField] private AudioClip _cuttingLoopSound;
    private AudioSource _cuttingLoopSource;

    private bool _wasPlayingLastFrame = false;

    private void Start()
    {
        if (_cuttingLoopSound != null)
        {
            _cuttingLoopSource = gameObject.AddComponent<AudioSource>();
            _cuttingLoopSource.clip = _cuttingLoopSound;
            _cuttingLoopSource.loop = true;
            _cuttingLoopSource.playOnAwake = false;
            _cuttingLoopSource.volume = SoundManager.Instance != null ? SoundManager.Instance.SFXVolume : 1f;

            Debug.Log($"{gameObject.name} | Cutting AudioSource initialized with clip: {_cuttingLoopSound.name} at volume {_cuttingLoopSource.volume}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} | No cutting sound assigned!");
        }
    }

    public override void ManualUpdate()
    {
        base.ManualUpdate();

        if (currentState == StationState.Preparing)
        {
            bool isPlayerNearby = PlayerIsNearby();
            Debug.Log($"{gameObject.name} | Player nearby: {isPlayerNearby}");

            if (isPlayerNearby)
            {
                if (!_cuttingLoopSource.isPlaying)
                {
                    _cuttingLoopSource.pitch = Random.Range(0.95f, 1.05f);
                    _cuttingLoopSource.Play();
                    Debug.Log($"{gameObject.name} | Starting cutting loop. Pitch: {_cuttingLoopSource.pitch}");
                }
            }
            else
            {
                if (_cuttingLoopSource.isPlaying)
                {
                    _cuttingLoopSource.Pause();
                    Debug.Log($"{gameObject.name} | Paused cutting loop due to player walking away.");
                }
            }

            _wasPlayingLastFrame = _cuttingLoopSource.isPlaying;
        }
        else if (_wasPlayingLastFrame)
        {
            if (_cuttingLoopSource != null && _cuttingLoopSource.isPlaying)
            {
                _cuttingLoopSource.Stop();
                Debug.Log($"{gameObject.name} | Stopped cutting loop after preparation.");
            }

            _wasPlayingLastFrame = false;
        }
    }

    protected override void OnPreparationFinished()
    {
        base.OnPreparationFinished();
        if (_cuttingLoopSource != null && _cuttingLoopSource.isPlaying)
        {
            _cuttingLoopSource.Stop();
        }
        Debug.Log("Cutting done!");
    }
}

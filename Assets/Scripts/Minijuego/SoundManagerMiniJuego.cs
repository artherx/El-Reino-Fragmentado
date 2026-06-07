using UnityEngine;

public class SoundManagerMiniJuego : MonoBehaviour
{
    public static SoundManagerMiniJuego Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource ambientSource;

    [Header("Volúmenes")]
    [Range(0f, 1f)] public float volumenSFX = 0.8f;
    [Range(0f, 1f)] public float volumenAmbiente = 0.15f;

    [Header("Sonidos del Minijuego")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoCaja;
    public AudioClip sonidoPuas;
    public AudioClip sonidoSpikeEnemy;
    public AudioClip sonidoGanar;
    public AudioClip ruidoAmbiental;

    private void Awake()
    {
        Instance = this;

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (ambientSource == null)
            ambientSource = gameObject.AddComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = volumenSFX;

        ambientSource.playOnAwake = false;
        ambientSource.loop = true;
        ambientSource.volume = volumenAmbiente;
    }

    private void Start()
    {
        if (ruidoAmbiental != null)
        {
            ambientSource.clip = ruidoAmbiental;
            ambientSource.volume = volumenAmbiente;
            ambientSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, volumenSFX);
    }

    public void PlaySalto()
    {
        PlaySFX(sonidoSalto);
    }

    public void PlayCaja()
    {
        PlaySFX(sonidoCaja);
    }

    public void PlayPuas()
    {
        PlaySFX(sonidoPuas);
    }

    public void PlaySpikeEnemy()
    {
        PlaySFX(sonidoSpikeEnemy);
    }

    public void PlayGanar()
    {
        PlaySFX(sonidoGanar);
    }
}
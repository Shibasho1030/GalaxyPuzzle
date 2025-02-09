using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSource2;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private KeySettingManager keySettingManager;
    public Slider volumeSlider;
    public Toggle muteToggle;
    public GameObject settingUI;
    public bool isSettingUIActive = false;


    //public KeyCode toggleMuteKey = KeyCode.M;
    //public KeyCode volumeUpKey = KeyCode.N;
    //public KeyCode volumeDownKey = KeyCode.B;
    public KeyCode toggleUIKey = KeyCode.Escape;

    public float volumeStep = 0.05f;
    
    float nextKeyVolumeInterval = 0.2f; 

    float nextKeyVolumeTimer; 

     
    void Start()
    {
        volumeSlider.value = audioSource.volume;
        muteToggle.isOn = audioSource.mute;

        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteToggle.onValueChanged.AddListener(SetMute);

        settingUI.SetActive(false);
 
        nextKeyVolumeTimer = Time.time;
    }


    void Update()
    {
        if (Input.GetKeyDown(keySettingManager.toggleMuteKey))
        {
            muteToggle.isOn = !muteToggle.isOn;
            SetMute(muteToggle.isOn);
        }

        if (Input.GetKey(keySettingManager.volumeUpKey) && (Time.time > nextKeyVolumeTimer) || Input.GetKeyDown(keySettingManager.volumeUpKey))
        {
            nextKeyVolumeTimer = Time.time + nextKeyVolumeInterval;
            AdjustVolume(volumeStep);
        }

        if (Input.GetKey(keySettingManager.volumeDownKey) && (Time.time > nextKeyVolumeTimer) || Input.GetKeyDown(keySettingManager.volumeDownKey))
        {
            nextKeyVolumeTimer = Time.time + nextKeyVolumeInterval;
            AdjustVolume(-volumeStep);
        }

        if (Input.GetKeyDown(toggleUIKey))
        {
            ToggleSettingUI();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetMute(bool isMuted)
    {
        audioSource.mute = isMuted;
        audioSource2.mute = isMuted;
    }

    public void AdjustVolume(float adjustment)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume + adjustment, 0f, 1f);

        volumeSlider.value = audioSource.volume;
    }

    public void ToggleSettingUI()
    {
        if (!keySettingManager.isWaitingForInput)
        {
            isSettingUIActive = !isSettingUIActive;
            settingUI.SetActive(isSettingUIActive);
            soundManager.UiSound();
            if(gameManager.gameStart && !gameManager.gameOver)
            {
                if (spawner.NewBlocks.Length != 7)
                {
                    gameManager.bgmObject.SetActive(!isSettingUIActive);
                }
                
                if (spawner.NewBlocks.Length == 7)
                {
                    gameManager.bgm2Object.SetActive(!isSettingUIActive);
                }
                
            }
            keySettingManager.isWaitingForInput = false;
        }
        else if (keySettingManager.isWaitingForInput)
        {
            soundManager.UiSound();
            keySettingManager.isWaitingForInput = false;
        }
    }



}

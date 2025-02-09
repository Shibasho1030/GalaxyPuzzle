using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySettingManager : MonoBehaviour
{
    //public AudioManager audioManager;
    //public GamaManager gamaManager;

    //現在のキーを表示するためのテキスト変数
    public TMP_Text moveLeftKeyText;
    public TMP_Text moveRightKeyText;
    public TMP_Text rotateRightKeyText;
    public TMP_Text quicklyDownKeyText;
    public TMP_Text moveDownKeyText;
    public TMP_Text toggleMuteKeyText;
    public TMP_Text volumeUpKeyText;
    public TMP_Text volumeDownKeyText;

    //キー変更用のボタン
    public Button changeMoveLeftKeyButton;
    public Button changeMoveRightKeyButton;
    public Button changeRotateRightKeyButton;
    public Button changeQuicklyDownKeyButton;
    public Button changeMoveDownKeyButton;
    public Button changeToggleMuteKeyButton;
    public Button changeVolumeUpKeyButton;
    public Button changeVolumeDownKeyButton;

    //現在変更中のアクション名
    private string currentChangingKey = "";

    //入力可能かどうかのbool型
    public bool isWaitingForInput = false;

     private const string MoveLeftKeyPref = "MoveLeftKey";
     private const string MoveRightKeyPref = "MoveRightKey";
     private const string RotateRightKeyPref = "RotateRightKey";
     private const string QuicklyDownKeyPref = "QuicklyDownKey";
     private const string MoveDownKeyPref = "MoveDownKey";
     private const string ToggleMuteKeyPref = "ToggleMuteKey";
     private const string VolumeUpKeyPref = "VolumeUpKey";
     private const string VolumeDownKeyPref = "VolumeDownKey";
    

    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode rotateRightKey = KeyCode.Space;
    public KeyCode quicklyDownKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode volumeUpKey = KeyCode.UpArrow;
    public KeyCode volumeDownKey = KeyCode.DownArrow;
    public KeyCode toggleMuteKey = KeyCode.M;
    

    void Start()
    {
        LoadKeySettings();

        UpdateKeyDisplay();

        changeMoveLeftKeyButton.onClick.AddListener(() => StartKeyChange("MoveLeftKey"));
        changeMoveRightKeyButton.onClick.AddListener(() => StartKeyChange("MoveRightKey"));
        changeRotateRightKeyButton.onClick.AddListener(() => StartKeyChange("RotateRightKey"));
        changeQuicklyDownKeyButton.onClick.AddListener(() => StartKeyChange("QuicklyDownKey"));
        changeMoveDownKeyButton.onClick.AddListener(() => StartKeyChange("MoveDownKey"));
        changeVolumeUpKeyButton.onClick.AddListener(() => StartKeyChange("VolumeUpKey"));
        changeVolumeDownKeyButton.onClick.AddListener(() => StartKeyChange("VolumeDownKey"));
        changeToggleMuteKeyButton.onClick.AddListener(() => StartKeyChange("ToggleMuteKey"));

    }


    void Update()
    {
        if (isWaitingForInput && Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        //isWaitingForInput = false;
                        currentChangingKey = "";
                        UpdateKeyDisplay();
                        //gameObject.SettingPanel.Select();
                        break;
                    }
                    SetKey(currentChangingKey, key);//新しいキーを設定
                    isWaitingForInput = false;
                    currentChangingKey = ""; 
                    UpdateKeyDisplay();
                }    
            }
                
        
        }
    }



    void StartKeyChange(string actionName)
    {
        isWaitingForInput = true;
        currentChangingKey = actionName;
        if(actionName == "MoveLeftKey") moveLeftKeyText.text = "Press a new key...";
        if(actionName == "MoveRightKey") moveRightKeyText.text = "Press a new key...";
        if(actionName == "RotateRightKey") rotateRightKeyText.text = "Press a new key...";
        if(actionName == "QuicklyDownKey") quicklyDownKeyText.text = "Press a new key...";
        if(actionName == "MoveDownKey") moveDownKeyText.text = "Press a new key...";
        if(actionName == "VolumeUpKey") volumeUpKeyText.text = "Press a new key...";
        if(actionName == "VolumeDownKey") volumeDownKeyText.text = "Press a new key...";
        if(actionName == "ToggleMuteKey") toggleMuteKeyText.text = "Press a new key...";
    }

    void SetKey(string actionName, KeyCode newKey)
    {
        if (actionName == "MoveLeftKey")
        {
            moveLeftKey = newKey;
            PlayerPrefs.SetString(MoveLeftKeyPref, newKey.ToString());
        }
        else if(actionName == "MoveRightKey")
        {
            moveRightKey = newKey;
            PlayerPrefs.SetString(MoveRightKeyPref, newKey.ToString());
        }
        else if (actionName == "RotateRightKey")
        {
            rotateRightKey = newKey;
            PlayerPrefs.SetString(RotateRightKeyPref, newKey.ToString());
        }
        else if (actionName == "QuicklyDownKey")
        {
            quicklyDownKey = newKey;
            PlayerPrefs.SetString(QuicklyDownKeyPref, newKey.ToString());
        }
        else if (actionName == "MoveDownKey")
        {
            moveDownKey = newKey;
            PlayerPrefs.SetString(MoveDownKeyPref, newKey.ToString());
        }
        else if (actionName == "VolumeUpKey")
        {
            volumeUpKey = newKey;
            PlayerPrefs.SetString(VolumeUpKeyPref, newKey.ToString());         
        }
        else if (actionName == "VolumeDownKey")
        {
            volumeDownKey = newKey;
            PlayerPrefs.SetString(VolumeDownKeyPref, newKey.ToString());
        }
        else if (actionName == "ToggleMuteKey")
        {
            toggleMuteKey = newKey;
            PlayerPrefs.SetString(ToggleMuteKeyPref, newKey.ToString());
        }

    }

    void LoadKeySettings()
    {
        if (PlayerPrefs.HasKey(MoveLeftKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(MoveLeftKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                moveLeftKey = loadedKey;
            }
        }
        if (PlayerPrefs.HasKey(MoveRightKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(MoveRightKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                moveRightKey = loadedKey;
            }
        }
        if (PlayerPrefs.HasKey(RotateRightKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(RotateRightKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                moveRightKey = loadedKey;
            }
        }
        if (PlayerPrefs.HasKey(QuicklyDownKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(QuicklyDownKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                quicklyDownKey = loadedKey;
            }
        }
        if (PlayerPrefs.HasKey(MoveDownKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(MoveDownKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                moveDownKey = loadedKey;
            }
        }
        if (PlayerPrefs.HasKey(VolumeUpKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(VolumeUpKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                volumeUpKey = loadedKey;
            }
        }
        if (PlayerPrefs.HasKey(VolumeDownKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(VolumeDownKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                volumeDownKey = loadedKey;
            }
        }
        if (PlayerPrefs.HasKey(ToggleMuteKeyPref))
        {
            string savedKey = PlayerPrefs.GetString(ToggleMuteKeyPref);
            if (System.Enum.TryParse(savedKey, out KeyCode loadedKey))
            {
                toggleMuteKey = loadedKey;
            }
        }
    }

    void UpdateKeyDisplay()
    {
        moveLeftKeyText.text = $"{moveLeftKey}";
        moveRightKeyText.text = $"{moveRightKey}";
        rotateRightKeyText.text = $"{rotateRightKey}";
        quicklyDownKeyText.text = $"{quicklyDownKey}";
        moveDownKeyText.text = $"{moveDownKey}";
        volumeUpKeyText.text = $"{volumeUpKey}";
        volumeDownKeyText.text = $"{volumeDownKey}";
        toggleMuteKeyText.text = $"{toggleMuteKey}";
    }


}

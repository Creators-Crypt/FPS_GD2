using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("References")]
    private EnemyAI enemyAI;

    [Header("Menus")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject MenuOpening;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [Header("Win Conditions")]
    [SerializeField] private int enemiesKilled;
    [SerializeField, Range(1, 10)] private int enemyCount = 5;

    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;

    float timeScaleOrig = 1f;

    private void OnEnable() {
        enemyAI.OnKilled += EnemyAI_OnKilled;
    }
    private void OnDisable() {
        enemyAI.OnKilled -= EnemyAI_OnKilled;
    }
    private void EnemyAI_OnKilled(int amount) {
        enemiesKilled += amount;

        if (enemiesKilled >= enemyCount) SetWin();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timeScaleOrig = Time.timeScale;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (menuActive == null) {
                Pause();
                menuActive = menuPause;
                menuActive.SetActive(true);

            } else if (menuActive == menuPause) {
                Unpause();
            }
        }
    }

    public void Pause() {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause() {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
    public void SetWin() { 
        menuWin.SetActive(true);
        Pause();
    }
    public void SetLose() { 
        menuLose.SetActive(true); 
        Pause();
    }
}
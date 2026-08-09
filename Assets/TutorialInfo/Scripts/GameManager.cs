using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject MenuOpening; 
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;

    float timeScaleOrig;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timeScaleOrig = Time.timeScale; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(menuActive == null)
            {
                Pause();
                menuActive = menuPause;
                menuActive.SetActive(true); 

            }   
            else if(menuActive == menuPause)
            {
                Unpause(); 
            }    
        }    
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }    

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null; 
    }
}

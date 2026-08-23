using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnPoint;

    private HealthSystem healthSystem;

    private void Start()
    {
        if(player != null)
        {
            healthSystem = player.GetComponent<HealthSystem>();
        }
        spawnPlayer();
    }

    public void spawnPlayer()
    {
        if (player == null || spawnPoint == null)
            return;

        CharacterController controller = player.GetComponent<CharacterController>();

        if(controller != null)
        {
            controller.enabled = false;
        }

        player.position = spawnPoint.position;
        player.rotation = spawnPoint.rotation;

        if(controller != null)
        {
            controller.enabled = true;
        }
    }

    public void respawnPlayer()
    {
        spawnPlayer();

        if(healthSystem != null)
        {
            healthSystem.ResetHealth();
        }

        GameManager.Instance.RespawnGame();
    }

    public void setSpawnPoint(Transform newSpawnPoint)
    {
        if(newSpawnPoint != null)
        {
            spawnPoint = newSpawnPoint;
        }
    }
}

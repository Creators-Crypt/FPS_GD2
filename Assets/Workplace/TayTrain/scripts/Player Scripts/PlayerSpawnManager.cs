using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
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
}

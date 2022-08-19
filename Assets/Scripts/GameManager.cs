using UnityEngine;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public  GameObject player;
    public static Vector3 playerSpawnPos = new(-6.5f, -3f, 0.5f);

    public void Death(Vector3 spawnPos, GameObject target)
    {
        Respawn(spawnPos, target);
        Destroy(target);
    }

    public void Respawn(Vector3 spawnPos, GameObject target)
    {
        Instantiate(target, spawnPos, transform.rotation);
        transform.eulerAngles = new(0, 90, 0);
    }
}
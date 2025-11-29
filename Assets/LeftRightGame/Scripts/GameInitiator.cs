using UnityEngine;

namespace LeftRightGame
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private GameObject gamePrefab;
        [SerializeField] private float delay;

        private void Start()
        {
            Invoke(nameof(SpawnGamePrefab), delay);
        }

        private void SpawnGamePrefab()
        {
            Instantiate(gamePrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
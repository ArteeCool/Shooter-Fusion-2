using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Code.Networking
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        
        private Dictionary<PlayerRef, NetworkObject> _players;
        
        private void Start()
        {
            Server.Instance.RegisterPlayerSpawner(this);

            _players = new Dictionary<PlayerRef, NetworkObject>();
        }

        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;
            
            var spawnPosition =  new Vector3();

            for (int i = 0; i < 3; i++)
            {
                spawnPosition[i] = Random.Range(-25.0f, 25.0f);
            }
            
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            
            _players.Add(player, networkPlayerObject);
        }

        public void DespawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (_players.TryGetValue(player, out var networkObject))
            {
                runner.Despawn(networkObject);
                _players.Remove(player);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Networking
{
    public class Server : MonoBehaviour, INetworkRunnerCallbacks
    {
        #region ServerSetup

        public static Server Instance;

        private RoomController _roomController;
        private PlayerSpawner _playerSpawner;
        
        private NetworkRunner _runner;

        private async void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            
            var result = await _runner.JoinSessionLobby(SessionLobby.ClientServer, "MainGameLobby");
            
            if (!result.Ok) 
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            }
        }
        
        async void StartGame(GameMode mode, String roomName)
        {
            if (!_runner.IsCloudReady) return;
                
            var scene = SceneRef.FromIndex(1);
            var sceneInfo = new NetworkSceneInfo();

            if (scene.IsValid)
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
                    
            await _runner.StartGame(new StartGameArgs
            {
                GameMode = mode,
                SessionName = roomName,
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            if (_roomController == null) return;
            
            _roomController.UpdateRooms(sessionList);
        }

        public void ConnectServer(String roomName)
        {
            StartGame(GameMode.Client, roomName);
        }

        public void HostServer(String roomName)
        {
            StartGame(GameMode.Host, roomName);
        }

        public void RegisterRoomController(RoomController newRoomController)
        {
            if (_roomController != null) return;
            
            _roomController = newRoomController;
        }
        
        public void RegisterPlayerSpawner(PlayerSpawner playerSpawner)
        {
            if (_playerSpawner != null) return;
            
            _playerSpawner = playerSpawner;
        }
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (_runner.State != NetworkRunner.States.Running) return;
            if (_playerSpawner == null) return;
            
            _playerSpawner.SpawnPlayer(runner, player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_runner.State != NetworkRunner.States.Running) return;
            if (_playerSpawner == null) return;
            
            _playerSpawner.DespawnPlayer(runner, player);
        }

        #endregion

        #region UnusedCallbacks
        
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
            
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
            
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            
        }

        public void OnConnectedToServer()
        {
            OnConnectedToServer(null);
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            
        }
        
        #endregion
    }
}

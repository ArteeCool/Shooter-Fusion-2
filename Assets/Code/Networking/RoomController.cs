using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Networking
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private GameObject _sessionPrefab;

        [SerializeField] private Transform _contentGameObject;
        
        [SerializeField] private TMP_InputField _roomNameInputField;
        
        private List<GameObject> _rooms;

        private void Start()
        {
            Server.Instance.RegisterRoomController(this);
            _rooms = new List<GameObject>();
        }

        public void UpdateRooms(List<SessionInfo> sessionList)
        {
            _rooms.ForEach(Destroy);

            for (int i = 0; i < sessionList.Count; i++)
            {
                var sessionInfo = sessionList[i];
                var session = Instantiate(_sessionPrefab, _contentGameObject);
                _rooms.Add(session);

                session.transform.GetChild(0).GetComponent<TMP_Text>().text = sessionInfo.Name;
                session.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";
                session.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    Server.Instance.ConnectServer(sessionInfo.Name);
                });
            }
        }

        public void HostRoom()
        {
            String input = _roomNameInputField.text;
            
            if (!String.IsNullOrWhiteSpace(input))
                Server.Instance.HostServer(_roomNameInputField.text);
        }
    }
}

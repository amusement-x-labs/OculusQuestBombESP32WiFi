using System;
using Cysharp.Threading.Tasks;
using Meta.Net.NativeWebSocket;
using UnityEngine;

namespace Core.Scripts
{
    public class WebBasedController : MonoBehaviour
    {
        public event Action OnWinState;
        public event Action OnLostState;

        [SerializeField] private string _socketUrl = "ws://192.168.100.18:5000/ws";
    
        private WebSocket _websocket;

        private async void Start()
        {
            await SetupWebSocket();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            _websocket?.DispatchMessageQueue();
#endif
        }

        private async void OnDestroy()
        {
            if (_websocket == null) 
                return;
        
            Debug.Log("Close WEB SOCKET ON DESTROY");
            await _websocket.Close();
        }

        private async void OnApplicationQuit()
        {
            if (_websocket == null) 
                return;
        
            Debug.Log("Close WEB SOCKET ON APP QUITE");
            await _websocket.Close();
        }

        private async UniTask SetupWebSocket()
        {
            _websocket = new WebSocket(_socketUrl);

            if (_websocket == null)
            {
                Debug.LogError("Cannot create WebSocket!");
                return;
            }

            _websocket.OnOpen += () =>
            {
                Debug.Log("_websocket is opened");
            };

            _websocket.OnError += (e) =>
            {
                Debug.LogError("_websocket error: " + e);
            };

            _websocket.OnClose += (e) =>
            {
                Debug.Log("_websocket is closed");
            };

            _websocket.OnMessage += OnMessageReceived;

            await _websocket.Connect();
        }

        private void OnMessageReceived(byte[] data)
        {
            var message = System.Text.Encoding.UTF8.GetString(data);

            message = message.TrimEnd('\0');

            if (string.Equals(message, StaticConstants.WinStatus))
            {
                Debug.Log("WIN");
                OnWinState?.Invoke();
                return;
            }

            if (string.Equals(message, StaticConstants.LostStatus))
            {
                Debug.Log("LOST");
                OnLostState?.Invoke();
                return;
            }

            Debug.LogWarning($"Unknown socket event is received: {message}");
        }
    }
}

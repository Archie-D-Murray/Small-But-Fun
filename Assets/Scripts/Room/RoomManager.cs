using UI;

using UnityEngine;

using Utilities;

namespace Rooms {
    public class RoomManager : Singleton<RoomManager> {
        [SerializeField] private Room[] _rooms;
        [SerializeField] private int _currentRoom = 0;
        [SerializeField] private GameObject WinBarrier;

        public Room CurrentRoom {
            get {
                if (_currentRoom == _rooms.Length) {
                    return null;
                }
                return _rooms[_currentRoom];
            }
        }

        public void RoomCleared() {
            _currentRoom++;
            if (_currentRoom == _rooms.Length) {
                WinBarrier.gameObject.SetActive(false);
            }
        }
    }
}
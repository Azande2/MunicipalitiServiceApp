using System.Collections.Generic;

namespace ProgPart17312.DataStructures
{
    public class ChatMessageQueue
    {
        private Queue<string> _messages = new Queue<string>();

        public void Enqueue(string message)
        {
            _messages.Enqueue(message);
        }

        public string Dequeue()
        {
            return _messages.Count > 0 ? _messages.Dequeue() : null;
        }

        public int Count => _messages.Count;

        public IEnumerable<string> GetAllMessages()
        {
            return _messages.ToArray();
        }

        public void Clear()
        {
            _messages.Clear();
        }
    }
}

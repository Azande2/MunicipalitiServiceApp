using System.Collections.Generic;

namespace ProgPart17312.DataStructures
{
    public class ChatMessageQueue
    {
        private Queue<string> queue = new Queue<string>();

        public void Enqueue(string message) => queue.Enqueue(message);
        public string Dequeue() => queue.Count > 0 ? queue.Dequeue() : null;
        public int Count => queue.Count;
        public void Clear() => queue.Clear();
        public string Peek() => queue.Count > 0 ? queue.Peek() : null;
    }
}

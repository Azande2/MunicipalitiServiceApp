using System.Collections;
using System.Collections.Generic;

namespace ProgPart17312.DataStructures
{
    public class StackLogHistory
    {
        private Stack<string> logStack = new Stack<string>();

        public List<string> GetAllLogs()
        {
            return new List<string>(logStack.ToArray()); // 🔁 Fixed name
        }

        public void Push(string message) => logStack.Push(message);
        public string Pop() => logStack.Count > 0 ? logStack.Pop() : null;
        public string Peek() => logStack.Count > 0 ? logStack.Peek() : null;
        public int Count => logStack.Count;
        public IEnumerable<string> GetAllMessages() => logStack;
    }
}

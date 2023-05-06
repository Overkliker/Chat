using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    internal class Parser
    {
        private string[] _splitMessage;

        public string sender;
        public string message;
        public int countString;

        private List<string> _splitMessages;

        public string splitMessages(int i)
        {
            string mess = _splitMessages[i];
            splitMessage(mess);
            return mess;
        }

        public List<string> getSplitMessages()
        {
            return _splitMessages;
        }

        public Parser(string message)
        {
            _splitMessages = message.Replace("\0", "")
                .Split("\n")
                .Where(element => !string.IsNullOrWhiteSpace(element)).ToList();
            countString = _splitMessages.Count();
        }

        private void splitMessage(string message)
        {
            _splitMessage = message.Replace("\0", "").Split();
            if (_splitMessage.Count() >= 4)
            {
                sender = _splitMessage[0];

                if (_splitMessage.Length > 4)
                {
                    for (int i = 3; i < _splitMessage.Length; i++)
                    {
                        this.message += " " + _splitMessage[i];
                    }
                }
                else
                {
                    this.message = _splitMessage[3];
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcedDddKernal
{
    public class DomainCommand
    {
        public string User { get; private set; }

        public DomainCommand(string user)
        {
            this.User = user;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcedDddKernal
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class EventApplier : System.Attribute
    {
    }
}

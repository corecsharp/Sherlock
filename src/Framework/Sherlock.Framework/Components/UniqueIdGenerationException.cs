using Sherlock.Framework;
using System;

namespace Sherlock.Framework.Components
{
    public class UniqueIdGenerationException : SherlockException
    {
        public UniqueIdGenerationException(string message)
            : base(message)
        {
        }
    }
}
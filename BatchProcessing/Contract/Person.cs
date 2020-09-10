using System;
using System.Collections.Generic;


namespace BatchProcessingContract
{
    public interface Person
    {
        string Id { get; }
        string Name { get; }
        DateTime DateOfBirth { get; }
        List<string> Interests { get; }
    }
}
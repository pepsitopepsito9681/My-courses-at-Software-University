﻿namespace PersonInfo
{
    public interface IPerson : IIdentifiable, IBirthable
    {
        string Name { get; }

        int Age { get; }
    }
}
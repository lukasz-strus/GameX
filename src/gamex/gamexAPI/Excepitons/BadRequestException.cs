﻿namespace gamexAPI.Excepitons;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
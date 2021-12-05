﻿namespace XTI_Core;

public sealed class ErrorList
{
    private readonly List<ErrorModel> errors = new List<ErrorModel>();

    public void Add(string message) => Add(new ErrorModel(message));
    public void Add(string message, string caption, string source) => Add(new ErrorModel(message, caption, source));
    public void Add(ErrorModel error) => errors.Add(error);

    public bool Any() => errors.Count > 0;

    public IEnumerable<ErrorModel> Errors() => errors.ToArray();

    public override string ToString()
    {
        var joinedErrors = string.Join("\r\n", errors.Select(e => e.Message));
        return $"{nameof(ErrorList)}\r\n{joinedErrors}";
    }
}
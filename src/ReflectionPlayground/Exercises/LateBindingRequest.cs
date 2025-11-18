namespace ReflectionPlayground.Exercises;

internal record LateBindingRequest(string ClassName, string MethodName, object?[] Arguments);

using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ReflectionPlayground.Attributes;
using ReflectionPlayground.Models;
using ReflectionPlayground.Services;

namespace ReflectionPlayground.Exercises;

public class ExerciseSuite
{
    public void RunAll()
    {
        WriteSectionHeader("RunOnStart Attribute (اختیاری)");
        RunStartupTasks();

        WriteSectionHeader("تمرین 1 — Reflection پایه");
        RunReflectionBasics();

        WriteSectionHeader("تمرین 2 — Late Binding & Invoke");
        RunLateBindingDemo();

        WriteSectionHeader("تمرین 3 — Developer Attribute");
        RunDeveloperAttributeDemo();

        WriteSectionHeader("تمرین 4 — Validation Attribute");
        RunValidationDemo();

        WriteSectionHeader("تمرین 5 — Logging Attribute");
        RunLoggingDemo();

        WriteSectionHeader("تمرین 6 — Mapper ساده");
        RunMapperDemo();
    }

    private static void RunReflectionBasics()
    {
        var person = new Person
        {
            FirstName = "Reza",
            LastName = "Moradi",
            Age = 30
        };

        var type = person.GetType();
        Console.WriteLine($"نام کلاس: {type.FullName}\n");

        Console.WriteLine("Property ها:");
        foreach (var property in type.GetProperties())
        {
            Console.WriteLine($" - {property.Name} ({property.PropertyType.Name})");
        }

        Console.WriteLine("\nمتدهای تعریف شده در کلاس:");
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($" - {method.Name}({parameters}) : {method.ReturnType.Name}");
        }
    }

    private static void RunLateBindingDemo()
    {
        var requests = new List<LateBindingRequest>
        {
            new("Calculator", "Add", new object?[] { 5, 9 }),
            new("Calculator", "Power", new object?[] { 2d, 8d }),
            new("Printer", "PrintWithPrefix", new object?[] { "سلام!" }),
            new("Printer", "Repeat", new object?[] { "Reflection", 2 }),
            new("Unknown", "Test", Array.Empty<object?>())
        };

        foreach (var request in requests)
        {
            InvokeDynamically(request);
        }
    }

    private static void InvokeDynamically(LateBindingRequest request)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var type = assembly
            .GetTypes()
            .FirstOrDefault(t => t.Name.Equals(request.ClassName, StringComparison.OrdinalIgnoreCase));

        if (type is null)
        {
            Console.WriteLine($"❌ نوع '{request.ClassName}' یافت نشد.");
            return;
        }

        var instance = Activator.CreateInstance(type);
        var method = type.GetMethod(request.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

        if (method is null)
        {
            Console.WriteLine($"❌ متد '{request.MethodName}' در کلاس '{type.Name}' پیدا نشد.");
            return;
        }

        try
        {
            var result = method.Invoke(instance, request.Arguments);
            if (method.ReturnType == typeof(void))
            {
                Console.WriteLine($"✅ {type.Name}.{method.Name} با موفقیت اجرا شد.");
            }
            else
            {
                Console.WriteLine($"✅ نتیجه {type.Name}.{method.Name}: {result}");
            }
        }
        catch (TargetParameterCountException)
        {
            Console.WriteLine("❌ تعداد پارامترها با متد هم‌خوانی ندارد.");
        }
    }

    private static void RunDeveloperAttributeDemo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var annotatedTypes = assembly
            .GetTypes()
            .Select(t => new
            {
                Type = t,
                Attributes = t.GetCustomAttributes<DeveloperAttribute>()
            })
            .Where(t => t.Attributes.Any())
            .ToList();

        foreach (var entry in annotatedTypes)
        {
            foreach (var attribute in entry.Attributes)
            {
                Console.WriteLine($"کلاس {entry.Type.Name} توسط {attribute.Name} (سطح: {attribute.Level}) ساخته شده است.");
            }

            foreach (var method in entry.Type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                foreach (var attribute in method.GetCustomAttributes<DeveloperAttribute>())
                {
                    Console.WriteLine($"  - متد {method.Name} توسط {attribute.Name} (سطح: {attribute.Level})");
                }
            }
        }
    }

    private static void RunValidationDemo()
    {
        var invalidModel = new UserInputModel
        {
            FirstName = string.Empty,
            Email = null,
            Biography = "", // optional
            BirthDate = null
        };

        Console.WriteLine("اعتبارسنجی داده نامعتبر:");
        ValidateModel(invalidModel);

        var validModel = new UserInputModel
        {
            FirstName = "Negar",
            Email = "negar@example.com",
            Biography = "Software engineer",
            BirthDate = new DateTime(1994, 4, 3)
        };

        Console.WriteLine("\nاعتبارسنجی داده معتبر:");
        ValidateModel(validModel);
    }

    private static void ValidateModel(object model)
    {
        var type = model.GetType();
        var hasErrors = false;

        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var attribute = property.GetCustomAttribute<MyRequiredAttribute>();
            if (attribute is null)
            {
                continue;
            }

            var value = property.GetValue(model);
            var isInvalid = value switch
            {
                null => true,
                string str => string.IsNullOrWhiteSpace(str),
                _ => false
            };

            if (isInvalid)
            {
                hasErrors = true;
                var message = attribute.ErrorMessage ?? $"Property {property.Name} is required.";
                Console.WriteLine($" - {message}");
            }
        }

        if (!hasErrors)
        {
            Console.WriteLine("✅ همه مقادیر معتبر هستند.");
        }
    }

    private static void RunLoggingDemo()
    {
        var target = new PerformanceSamples();
        var methodArguments = new Dictionary<string, object?[]>
        {
            [nameof(PerformanceSamples.SumRange)] = new object?[] { 1000 }
        };

        foreach (var method in typeof(PerformanceSamples).GetMethods(BindingFlags.Instance | BindingFlags.Public))
        {
            var attribute = method.GetCustomAttribute<LogExecutionAttribute>();
            if (attribute is null)
            {
                continue;
            }

            var args = methodArguments.TryGetValue(method.Name, out var value)
                ? value
                : Array.Empty<object?>();

            var stopwatch = Stopwatch.StartNew();
            var result = method.Invoke(target, args);
            stopwatch.Stop();

            var label = attribute.Tag is null ? method.Name : $"{method.Name} ({attribute.Tag})";
            var suffix = method.ReturnType == typeof(void) ? string.Empty : $" | نتیجه: {result}";
            Console.WriteLine($"{label} اجرا شد در {stopwatch.ElapsedMilliseconds} ms{suffix}");
        }
    }

    private static void RunMapperDemo()
    {
        var dto = new UserDto
        {
            Id = Guid.NewGuid(),
            GivenName = "Hamed",
            FamilyName = "Kiani",
            Email = "hamed@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var entity = SimpleMapper.Map<UserDto, UserEntity>(dto);

        Console.WriteLine("مقادیر UserEntity پس از نگاشت:");
        Console.WriteLine($" - Id: {entity.Id}");
        Console.WriteLine($" - FirstName: {entity.FirstName}");
        Console.WriteLine($" - LastName: {entity.LastName}");
        Console.WriteLine($" - Email: {entity.Email}");
        Console.WriteLine($" - RegisteredAt: {entity.RegisteredAt}");
    }

    private static void RunStartupTasks()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var methods = assembly
            .GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
            .Select(m => (Method: m, Attribute: m.GetCustomAttribute<RunOnStartAttribute>()))
            .Where(tuple => tuple.Attribute is not null)
            .OrderBy(tuple => tuple.Attribute!.Order)
            .ToList();

        foreach (var (method, attribute) in methods)
        {
            if (method.GetParameters().Length > 0)
            {
                Console.WriteLine($"⚠️ متد {method.Name} پارامتر دارد و اجرا نشد.");
                continue;
            }

            object? instance = null;
            if (!method.IsStatic)
            {
                instance = Activator.CreateInstance(method.DeclaringType!);
            }

            try
            {
                method.Invoke(instance, null);
                var description = attribute!.Description is null ? string.Empty : $" ({attribute.Description})";
                Console.WriteLine($"✅ {method.DeclaringType!.Name}.{method.Name}{description}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ اجرای {method.Name} با خطا مواجه شد: {ex.Message}");
            }
        }
    }

    private static void WriteSectionHeader(string title)
    {
        Console.WriteLine("\n" + new string('═', 70));
        Console.WriteLine(title);
        Console.WriteLine(new string('═', 70));
    }
}

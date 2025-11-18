namespace ReflectionPlayground.Services;

public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Multiply(int a, int b) => a * b;
    public double Power(double value, double exponent) => Math.Pow(value, exponent);
}

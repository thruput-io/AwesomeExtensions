namespace TestNamespace;

public class TargetClass
{
}

public class Usage
{
    public void M(TargetClass target)
    {
        target.ExtensionMethod{caret}();
    }
}

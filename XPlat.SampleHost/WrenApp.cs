using XPlat.Core;
using XPlat.WrenScripting;

namespace XPlat.SampleHost
{
    public class TestClass {
        public void Say(string text, int a, float b){
            System.Console.WriteLine(text);
            System.Console.WriteLine(a);
            System.Console.WriteLine(b);
        }
    }
    
    public class WrenApp : ISdlApp
    {
        public void Init()
        {
            var vm = new WrenVm();
            //fi.Create();
            //vm.RegisterType(typeof(TestClass), () => new TestClass());
            vm.Interpret("XPlat.SampleHost", @"System.print(""Hello World"")
foreign class TestClass {
    construct new() {}
    foreign Say(text,a,b)
}

class WrenClass {
    construct new() {}
    say(){
        System.print(""Hello from the other side"")
    }
}");
            vm.Interpret("main", @"import ""XPlat.SampleHost"" for TestClass
var fi = TestClass.new()
fi.Say(""Hallo"", 42, 1.5)
System.print(fi)");

            var wrenClass = vm.GetObject("XPlat.SampleHost", "WrenClass");
            var instance = wrenClass.CallForObject("new()");
            instance.Call("say()");

            vm.Dispose();
        }

        public void Update()
        {
        }
    }
}

using Mono.Cecil;

namespace Examples.Metaprogramming.Tests._.Mono.Cecil;

public class AssemblyDefinitionTests
{
    [Fact]
    public void WhenCallingProgramMain_OutToConsole()
    {
        ModuleDefinition module = new ProgramClassBuilder().Build();

        var path = typeof(AssemblyDefinitionTests).Assembly.Location;
        var newPath = TestPathUtils.GetOutPath(
            $"{Path.GetFileNameWithoutExtension(path)}.${nameof(AssemblyDefinitionTests)}.dll");

        Directory.CreateDirectory(Path.GetDirectoryName(newPath)!);
        File.Delete(newPath);
        module.Write(newPath);

        // var mock = new Mock<TextWriter>();
        // mock.Setup(x => x.WriteLine((object)"Hello Mono.Cecil World."));

        // // ConsoleHelper.RunWith(mock.Object, ()
        // //     => programClass.GetMethod("Main")!.Invoke(null, null));

        // mock.Verify(x => x.WriteLine((object)"Hello Mono.Cecil."), Times.Exactly(1));
        // mock.VerifyNoOtherCalls();
        return;
    }

}


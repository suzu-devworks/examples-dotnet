namespace Examples.IO.Tests._.System.IO;

/// <summary>
/// Tests to check the difficult-to-understand specifications contained in the <see cref="Path" />.
/// </summary>
public class PathCombineSpecificationsTests
{

    [Fact]
    public void WhenCallingCombine_WithInputStartWithYen()
    {

        Path.Combine(@"/parent/folder", @"subfolder/file.ext")
            .Is($@"/parent/folder{Path.DirectorySeparatorChar}subfolder/file.ext");

        // path1 is ignored if path2 is the root path.
        Path.Combine(@"/parent/folder", @"/subfolder/file.ext")
            .Is(@"/subfolder/file.ext");

        if (Path.DirectorySeparatorChar == '\\')
        {
            Path.Combine(@"c:\parent\folder", @"subfolder\file.ext")
                .Is(@"c:\parent\folder\subfolder\file.ext");

            // path1 is ignored if path2 start with the drive.
            Path.Combine(@"c:\parent\folder", @"c:¥subfolder\file.ext")
                .Is(@"c:¥subfolder\file.ext");

            // If path2 starts with "\", Path.IsPathRooted() as Root.
            Path.Combine(@"c:\parent\folder", @"\subfolder\file.ext")
                .Is(@"\subfolder\file.ext");

            // if path2 is Network Reference, Path.IsPathRooted() as Root.
            Path.Combine(@"c:\parent|folder", @"\\subfolder\file.ext")
                .Is(@"\\subfolder\file.ext");
        }

        return;
    }
}

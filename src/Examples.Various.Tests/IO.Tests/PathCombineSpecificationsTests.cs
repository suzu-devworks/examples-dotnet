namespace Examples.IO.Tests;

/// <summary>
/// Tests to check the difficult-to-understand specifications contained in the <see cref="Path" />.
/// </summary>
public class PathCombineSpecificationsTests
{
    public static bool IsPosixStyle => Path.DirectorySeparatorChar == '/';

    public class PosixPathCombineMethod
    {
        [Theory(
            Skip = "Is not POSIX Platform",
            SkipType = typeof(PathCombineSpecificationsTests), SkipUnless = nameof(IsPosixStyle))]
        [InlineData(@"/var/log", @"nginx/access.log", @"/var/log/nginx/access.log")]
        [InlineData(@"/var/run", @"docker-host.sock", @"/var/run/docker-host.sock")]
        [InlineData(@"/etc/hosts.deny", @"hosts.allow", @"/etc/hosts.deny/hosts.allow")]
        public void When_SpecifyingRelativePathForPath2_Then_ReturnsCombinedPath(string path1, string path2, string expected)
        {
            Assert.Equal(expected, Path.Combine(path1, path2));
        }

        [Theory(
            Skip = "Is not POSIX Platform",
            SkipType = typeof(PathCombineSpecificationsTests), SkipUnless = nameof(IsPosixStyle))]
        [InlineData(@"/etc", @"/var/log/nginx/access.log")]
        [InlineData(@"etc/sysctl.conf", @"/temp")]
        [InlineData(@"sysctl.conf", @"/etc/hosts.deny")]
        public void When_SpecifyingAbsolutePathForPath2_Then_Returns2ndPath(string path1, string path2)
        {
            Assert.Equal(path2, Path.Combine(path1, path2));
        }

        [Theory(
            Skip = "Is not POSIX Platform",
            SkipType = typeof(PathCombineSpecificationsTests), SkipUnless = nameof(IsPosixStyle))]
        [InlineData(@"/etc", @"", @"sysctl.conf", @"/etc/sysctl.conf")]
        [InlineData(@"/etc", @"sysctl.conf", @"", @"/etc/sysctl.conf")]
        [InlineData(@"", @"/etc", @"sysctl.conf", @"/etc/sysctl.conf")]
        public void When_ContainSpaceInPaths_Then_IsIgnored(string path1, string path2, string path3, string expected)
        {
            Assert.Equal(expected, Path.Combine(path1, path2, path3));
        }

        [Theory(
            Skip = "Is not POSIX Platform",
            SkipType = typeof(PathCombineSpecificationsTests), SkipUnless = nameof(IsPosixStyle))]
        [InlineData(@"/var/log", @"C:\Program Files", @"/var/log/C:\Program Files")]
        [InlineData(@"C\:", @"Users\Default", @"C\:/Users\Default")]
        public void When_SeparatorsAreMixed_Then_AreConcatenatedAsIs(string path1, string path2, string expected)
        {
            Assert.Equal(expected, Path.Combine(path1, path2));
        }

        [Theory(
            Skip = "Is not POSIX Platform",
            SkipType = typeof(PathCombineSpecificationsTests), SkipUnless = nameof(IsPosixStyle))]
        [InlineData(@"/temp", @"/", @"/")]
        [InlineData(@"/temp", @"//", @"//")]
        [InlineData(@"/temp", @"\", @"/temp/\")]
        [InlineData(@"/temp", @":", @"/temp/:")]
        [InlineData(@"/temp", @"^*&)(_=@#'\\^&#2.*(.txt", @"/temp/^*&)(_=@#'\\^&#2.*(.txt")]
        public void When_InvalidCharactersAreIncluded_Then_Permissible(string path1, string path2, string expected)
        {
            Assert.Equal(expected, Path.Combine(path1, path2));
        }
    }

    public class WindowsPathCombineMethod
    {
        [Theory(
            Skip = "Is not Windows Platform",
            SkipType = typeof(PathCombineSpecificationsTests), SkipWhen = nameof(IsPosixStyle))]
        [InlineData(@"C:\Windows", @"System32\drivers\hosts", @"C:\Windows\System32\drivers\hosts")]
        public void When_SpecifyingRelativePathForPath2_Then_ReturnsCombinedPath(string path1, string path2, string expected)
        {
            Assert.Equal(expected, Path.Combine(path1, path2));
        }

        // TODO: Add tests for Windows Path.Combine specifications.
        // When the folder name ends with ":", "\" is not added.
        // If the file name is an absolute path, the absolute path is returned.
        // If the file name starts with "\", that string is returned as is
        // If the file name is blank, the folder name is returned as is.
        // illegal character
    }
}

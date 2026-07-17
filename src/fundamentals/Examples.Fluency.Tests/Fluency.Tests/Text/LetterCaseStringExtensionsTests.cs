using Examples.Fluency.Text;

namespace Examples.Fluency.Tests.Text;

public class StringLetterCaseExtensionsTests
{
    public class ToSnakeCaseMethod
    {
        [Theory]
        [InlineData("HeSaidThatThatWasTheTheCorrectAnswer.",
                    "he_said_that_that_was_the_the_correct_answer.")]
        [InlineData("HeSaid_ThatThat_Was_TheThe_CorrectAnswer.",
                    "he_said_that_that_was_the_the_correct_answer.")]
        [InlineData("he_said_that_that_was_the_the_correct_answer.",
                    "he_said_that_that_was_the_the_correct_answer.")] // nothing changes。
        public void When_ValidValues_Then_ReturnsSnakeCaseString(string input, string expected)
        {
            // Insert '_' before uppercase letter.
            // Do not insert the beginning.
            // Change uppercase letters to lowercase letters.
            // If there is a '_' in the place where you want to insert it, do not insert it.

            Assert.Equal(expected, input.ToSnakeCase());
        }
    }

    public class ToCamelCaseMethod
    {
        [Theory]
        [InlineData("he_said_that_that_was_the_the_correct_answer.",
                    "heSaidThatThatWasTheTheCorrectAnswer.")]
        [InlineData("heSaidThatThatWasTheTheCorrectAnswer.",
                    "heSaidThatThatWasTheTheCorrectAnswer.")] // nothing changes。
        public void When_ValidValues_Then_ReturnsCamelCaseString(string input, string expected)
        {
            // Capitalize after the "_" and remove the "_".
            // The beginning remains the same.
            // If you wish to make any changes, please make the changes separately.

            Assert.Equal(expected, input.ToCamelCase());
        }
    }
}

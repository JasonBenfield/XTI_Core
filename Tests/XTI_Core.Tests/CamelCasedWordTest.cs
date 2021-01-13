using NUnit.Framework;

namespace XTI_Core.Tests
{
    public sealed class CamelCasedWordTest
    {
        [Test]
        public void ShouldSplitCamelCasedWord()
        {
            var words = new CamelCasedWord("TestText").Words();
            Assert.That(words, Is.EqualTo(new[] { "Test", "Text" }));
        }

        [Test]
        public void ShouldSplitCamelCasedWordBeginningWithUpperCase()
        {
            var words = new CamelCasedWord("BEGINTestText").Words();
            Assert.That(words, Is.EqualTo(new[] { "BEGIN", "Test", "Text" }));
        }

        [Test]
        public void ShouldSplitCamelCasedWordEndingWithUpperCase()
        {
            var words = new CamelCasedWord("TestTextEND").Words();
            Assert.That(words, Is.EqualTo(new[] { "Test", "Text", "END" }));
        }

        [Test]
        public void ShouldSplitCamelCasedWordEndingWithDigits()
        {
            var words = new CamelCasedWord("TestTextEND12").Words();
            Assert.That(words, Is.EqualTo(new[] { "Test", "Text", "END", "12" }));
        }

        [Test]
        public void ShouldSplitCamelCasedWordContainingingWithDigits()
        {
            var words = new CamelCasedWord("Test12TextEND").Words();
            Assert.That(words, Is.EqualTo(new[] { "Test", "12", "Text", "END" }));
        }

        [Test]
        public void ShouldSplitCamelCasedWordStartingWithALowerCaseWord()
        {
            var words = new CamelCasedWord("testText").Words();
            Assert.That(words, Is.EqualTo(new[] { "test", "Text" }));
        }
    }
}

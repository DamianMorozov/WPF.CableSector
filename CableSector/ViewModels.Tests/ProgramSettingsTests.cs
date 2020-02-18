// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Класс-тест "Программные настройки".
    /// </summary>
    [TestFixture]
    public class ProgramSettingsTests
    {
        /// <summary>
        /// Setup private fields.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(Setup)} start.");
            TestContext.WriteLine($@"{nameof(Setup)} complete.");
        }

        /// <summary>
        /// Reset private fields to default state.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(Teardown)} start.");
            TestContext.WriteLine($@"{nameof(Teardown)} complete.");
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
        }

        [Test]
        public void Constructor_AreEqual()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(Constructor_AreEqual)} start.");

            ProgramSettings _settings = new ProgramSettings();
            var actual = _settings.Page.Width;
            TestContext.WriteLine($"actual: {actual}");
            var expected = 800;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);
            
            actual = _settings.Page.Height;
            TestContext.WriteLine($"actual: {actual}");
            expected = 600;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            actual = _settings.Page.FontSize;
            TestContext.WriteLine($"actual: {actual}");
            expected = 18;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            TestContext.WriteLine($@"{nameof(Constructor_AreEqual)} complete.");
        }
    }
}

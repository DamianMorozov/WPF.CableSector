// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Класс-тест "Настройки окна".
    /// </summary>
    [TestFixture]
    public class WindowSettingsTests
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

            PageSettings _window = new PageSettings(800, 500, 16);
            var actual = _window.Width;
            TestContext.WriteLine($"actual: {actual}");
            var expected = 800;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);
            
            actual = _window.Height;
            TestContext.WriteLine($"actual: {actual}");
            expected = 500;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            actual = _window.FontSize;
            TestContext.WriteLine($"actual: {actual}");
            expected = 16;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            TestContext.WriteLine($@"{nameof(Constructor_AreEqual)} complete.");
        }
    }
}

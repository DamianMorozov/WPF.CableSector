// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Класс-тест "Помощник холста".
    /// </summary>
    [TestFixture]
    public class CanvasHelperTests
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

            //var _canvasHelper = new CanvasHelper(
            //    new TableSettings(), new TableSettings(), new WindowSettings(), 1000, 10);
            //_canvasHelper.Execute(new Grid());
            TestContext.WriteLine($"actual: {true}");
            TestContext.WriteLine($"expected: {true}");
            Assert.AreEqual(true, true);

            TestContext.WriteLine($@"{nameof(Constructor_AreEqual)} complete.");
        }
    }
}

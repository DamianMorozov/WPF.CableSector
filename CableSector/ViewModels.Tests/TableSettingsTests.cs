// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Класс-тест "Табличные настройки".
    /// </summary>
    [TestFixture]
    public class TableSettingsTests
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
            //
            TestContext.WriteLine($@"{nameof(Teardown)} complete.");
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
        }

        [Test]
        public void RowsCount_AreEqual()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(RowsCount_AreEqual)} start.");

            var settings = new TableSettings(1);
            var actual = settings.RowsCount;
            TestContext.WriteLine($"actual: {actual}");
            var expected = 1;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            settings = new TableSettings();
            actual = settings.RowsCount;
            TestContext.WriteLine($"actual: {actual}");
            expected = 1;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            TestContext.WriteLine($@"{nameof(RowsCount_AreEqual)} complete.");
        }

        [Test]
        public void GetWidthAll_AreEqual()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(GetWidthAll_AreEqual)} start.");

            // 4х жильный кабель.
            var settings = new TableSettings(4);
            settings.RecordsCurrent[0].AddItem("10");
            settings.RecordsCurrent[0].AddItem("20");
            settings.RecordsCurrent[0].AddItem("30");
            settings.RecordsCurrent[0].AddItem("40");
            var actual = settings.GetWidthAll(0);
            TestContext.WriteLine($"actual: {actual}");
            var expected = 100;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            TestContext.WriteLine($@"{nameof(GetWidthAll_AreEqual)} complete.");
        }

        [Test]
        public void CalcMinMax_AreEqual()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(CalcMinMax_AreEqual)} start.");

            // 4х жильный кабель.
            var settings = new TableSettings(4);
            settings.RecordsCurrent[0].AddItem("10");
            settings.RecordsCurrent[0].AddItem("20");
            settings.RecordsCurrent[0].AddItem("30");
            settings.RecordsCurrent[0].AddItem("40");
            settings.RecordsCurrent[1].AddItem("50");
            settings.RecordsCurrent[1].AddItem("50");
            settings.RecordsCurrent[1].AddItem("100");
            settings.RecordsCurrent[1].AddItem("50");
            settings.RecordsCurrent[2].AddItem("10");
            settings.RecordsCurrent[2].AddItem("10");
            settings.RecordsCurrent[2].AddItem("10");
            settings.RecordsCurrent[2].AddItem("20");
            settings.Update();
            Assert.AreEqual(1, settings.CalcMaxRow);
            Assert.AreEqual(2, settings.CalcMinRow);
            Assert.AreEqual(4, settings.CalcMaxColCount);
            Assert.AreEqual(250, settings.CalcMaxSum);
            TestContext.WriteLine($@"{nameof(CalcMinMax_AreEqual)} 5.");
            Assert.AreEqual(50, settings.CalcMinSum);

            TestContext.WriteLine($@"{nameof(CalcMinMax_AreEqual)} complete.");
        }
    }
}

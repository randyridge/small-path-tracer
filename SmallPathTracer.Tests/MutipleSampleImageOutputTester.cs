using System.IO;
using System.Drawing;
using SmallPathTracer.Properties;
using NUnit.Framework;

namespace SmallPathTracer {
    public sealed class MultipleSampleImageOutputTester {
        // --- Private Constant Fields ---
        private const string OutputFileName = "MultipleSampleImageOutput.png";

        // --- Private Fields ---
        private Bitmap outputImage;
        private Bitmap referenceImage;

        // --- Public Methods ---
        [Test]
        public void output_image_data_matches_reference_image() {
            for(var x = 0; x < referenceImage.Width; x++) {
                for(var y = 0; y < referenceImage.Height; y++) {
                    Assert.That(outputImage.GetPixel(x, y), Is.EqualTo(referenceImage.GetPixel(x, y)));
                }
            }
        }

        [Test]
        public void output_image_has_correct_height() {
            Assert.That(outputImage.Height, Is.EqualTo(referenceImage.Height));
        }

        [Test]
        public void output_image_has_correct_width() {
            Assert.That(outputImage.Width, Is.EqualTo(referenceImage.Width));
        }

        [SetUp]
        public void Setup() {
            Program.Main(new[] { "1", "1", "8", OutputFileName, "0" });
            outputImage = new Bitmap(OutputFileName);
            referenceImage = Resources.MultiSampleReference;
        }

        [TearDown]
        public void Teardown() {
            outputImage.Dispose();
            referenceImage.Dispose();
            File.Delete(OutputFileName);
        }
    }
}
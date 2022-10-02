using NUnit.Framework;
using System.Drawing;
using System;

namespace StoriesUnitTests
{
    // N.B. These unit tests will succeed only on Windows because Environment.NewLine is "\r\n", and
    //      because the System.Graphics library functions that are used are dependant on the Windows OS.

    public class StringWrappingTests
    {
        Graphics g;
        [SetUp]
        public void Setup()
        {

            Bitmap bitmap = new Bitmap(1, 1);

            // Create a graphics object to measure the text's width and height.
            g = Graphics.FromImage(bitmap);
        }

        [Test]
        public void TestTypical()
        {
            string actualResult =
                SaveFileController.WrapTextWithGraphics(
                    Graphics.FromImage(new Bitmap(1, 1)),
                    "Hello this is a very long string, how long can it go? No one can say... okay, let's finish here. ",
                    300,
                    new Font("Calibri Light", 20, FontStyle.Regular, GraphicsUnit.Pixel)
                );

            Assert.AreEqual("Hello this is a very long string, how \r\nlong can it go? No one can say... \r\nokay, let's finish here. ", actualResult);
        }

        [Test]
        public void TestEmpty()
        {
            string actualResult =
                SaveFileController.WrapTextWithGraphics(
                    g,
                    string.Empty,
                    1000,
                    new Font("Calibri Light", 20, FontStyle.Regular, GraphicsUnit.Pixel)
                );

            Assert.AreEqual(string.Empty, actualResult);
        }

        [Test]
        public void TestNull()
        {
            string actualResult =
                SaveFileController.WrapTextWithGraphics(
                    g,
                    null,
                    1000,
                    new Font("Calibri Light", 20, FontStyle.Regular, GraphicsUnit.Pixel)
                );

            Assert.AreEqual(string.Empty, actualResult);
        }

        [Test]
        public void TestParagraphs()
        {
            string actualResult =
                SaveFileController.WrapTextWithGraphics(
                    g,
                    "The purpose of this challenge is to gather further evidence of the true abilities and qualifications of potential candidates for " +
                    "employment in Crawford Technologies software engineering division than would normally be possible via a simple interview." +
                    Environment.NewLine +
                    "The prospective employee is asked to take the enclosed project requirements and return to Crawford a functionally complete, working " +
                    "piece of software, for review by Crawford Technologies Management and Technical staff. Crawford’s staff will use this to evaluate " +
                    "the design decisions made by the prospective employee, as well as the craftsmanship and quality of the code and the project returned.  " +
                    "This project will have a significant impact on the applicant selection process. Please take this opportunity to demonstrate for " +
                    "Crawford Technologies your skills in software engineering." +
                    Environment.NewLine +
                    "The Challenge" +
                    Environment.NewLine +
                    "John enjoys writing stories and sharing them with his friend Pat. Pat thinks his writing should be preserved for others to see and " +
                    "enjoy. To this end, s / he has decided to build a website devoted to storing and preserving the stories for others to see. In addition, " +
                    "Pat would like this website to return a copy of his writing embedded in a graphic (*.png) image. This embedded image should display the " +
                    "stories enclosed in a rectangular box. (note, you do not need to write stories, you just need to show your ability with brief text " +
                    "paragraphs.) Pat also wants to track how popular the different stories are, so she is going to log story views and downloads.S / he " +
                    "will add a simple chart to each story’s page to show the data. ",
                    1000,
                    new Font("Calibri Light", 20, FontStyle.Regular, GraphicsUnit.Pixel)
                );

            string expectedResult =
                "The purpose of this challenge is to gather further evidence of the true abilities and qualifications of potential candidates \r\nfor " +
                "employment in Crawford Technologies software engineering division than would normally be possible via a simple \r\ninterview.\r\n" +
                Environment.NewLine +
                "The prospective employee is asked to take the enclosed project requirements and return to Crawford a functionally \r\ncomplete, working " +
                "piece of software, for review by Crawford Technologies Management and Technical staff. Crawford’s \r\nstaff will use this to evaluate " +
                "the design decisions made by the prospective employee, as well as the craftsmanship and \r\nquality of the code and the project returned.  " +
                "This project will have a significant impact on the applicant selection \r\nprocess. Please take this opportunity to demonstrate for " +
                "Crawford Technologies your skills in software engineering.\r\n" +
                Environment.NewLine +
                "The Challenge\r\n" +
                Environment.NewLine +
                "John enjoys writing stories and sharing them with his friend Pat. Pat thinks his writing should be preserved for others to \r\nsee and " +
                "enjoy. To this end, s / he has decided to build a website devoted to storing and preserving the stories for others \r\nto see. In addition, " +
                "Pat would like this website to return a copy of his writing embedded in a graphic (*.png) image. This \r\nembedded image should display the " +
                "stories enclosed in a rectangular box. (note, you do not need to write stories, you \r\njust need to show your ability with brief text " +
                "paragraphs.) Pat also wants to track how popular the different stories are, \r\nso she is going to log story views and downloads.S / he " +
                "will add a simple chart to each story’s page to show the data. ";

            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void TestParagraphsLongWord()
        {
            string actualResult =
                SaveFileController.WrapTextWithGraphics(
                    g,
                    "The purpose of this challenge is_to_gather_further_evidence_of_the_true_abilities_and_qualifications_of potential candidates for " +
                    "employment in Crawford Technologies software engineering division than would normally be possible via a simple interview." +
                    Environment.NewLine +
                    "The prospective employee is asked to take the enclosed project requirements and return to Crawford a functionally complete, working " +
                    "piece of software, for review by Crawford Technologies Management and Technical staff. Crawford’s staff will use this to evaluate " +
                    "the design decisions made by the prospective employee, as well_as_the_craftsmanship_and_quality_of_the_code_and_the_project_returned._" +
                    "This_project_will_have_a_significant_impact_on the applicant selection process. Please take this opportunity to demonstrate for " +
                    "Crawford Technologies your skills in software engineering." +
                    Environment.NewLine +
                    "The Challenge" +
                    Environment.NewLine +
                    "John enjoys writing stories and sharing them with his friend Pat. Pat thinks his writing should be preserved for others to see and " +
                    "enjoy. To this end, s / he has decided to build a website devoted to storing and preserving the stories for others to see. In addition, " +
                    "Pat would like this website to return_a_copy_of_his_writing_embedded_in_a_graphic_(*.png)_image._This embedded image should display the " +
                    "stories enclosed in a rectangular box. ",
                    300,
                    new Font("Calibri Light", 20, FontStyle.Regular, GraphicsUnit.Pixel)
                );

            string expectedResult =
                "The purpose of this challenge \r\nis_to_gather_further_evidence_of\r\n_the_true_abilities_and_qualificat\r\nions_of potential candidates for \r\n" +
                "employment in Crawford \r\nTechnologies software engineering \r\ndivision than would normally be \r\npossible via a simple interview.\r\n" +
                Environment.NewLine +
                "The prospective employee is asked \r\nto take the enclosed project \r\nrequirements and return to \r\nCrawford a functionally complete, \r\nworking " +
                "piece of software, for \r\nreview by Crawford Technologies \r\nManagement and Technical staff. \r\nCrawford’s staff will use this to \r\nevaluate " +
                "the design decisions \r\nmade by the prospective \r\nemployee, as \r\nwell_as_the_craftsmanship_and_\r\nquality_of_the_code_and_the_pr\r\noject_returned._" +
                "This_project_will_\r\nhave_a_significant_impact_on the \r\napplicant selection process. Please \r\ntake this opportunity to \r\ndemonstrate for " +
                "Crawford \r\nTechnologies your skills in software \r\nengineering.\r\n" +
                Environment.NewLine +
                "The Challenge\r\n" +
                Environment.NewLine +
                "John enjoys writing stories and \r\nsharing them with his friend Pat. \r\nPat thinks his writing should be \r\npreserved for others to see and \r\n" +
                "enjoy. To this end, s / he has \r\ndecided to build a website devoted \r\nto storing and preserving the \r\nstories for others to see. In \r\naddition, " +
                "Pat would like this \r\nwebsite to \r\nreturn_a_copy_of_his_writing_e\r\nmbedded_in_a_graphic_(*.png)_\r\nimage._This embedded image \r\nshould display the " +
                "stories enclosed \r\nin a rectangular box. ";

            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void TestShortParagraphs()
        {
            string actualResult =
                SaveFileController.WrapTextWithGraphics(
                    g,
                    "The Challenge" +
                    Environment.NewLine +
                    "John enjoys writing stories and sharing them with his friend Pat.Pat thinks his writing should be preserved for " +
                    "others to see and enjoy. To this end, s / he has decided to build a website devoted to storing and preserving the " +
                    "stories for others to see. In addition, Pat would like this website to return a copy of his writing embedded in a " +
                    "graphic (*.png) image. This embedded image should display the stories enclosed in a rectangular box. (note, you do not " +
                    "need to write stories, you just need to show your ability with brief text paragraphs.) Pat also wants to track how " +
                    "popular the different stories are, so she is going to log story views and downloads. S / he will add a simple chart to " +
                    "each story’s page to show the data." +
                    Environment.NewLine +
                    "These are Pat’s requirements." +
                    Environment.NewLine +
                    "1) Web site for stories" +
                    Environment.NewLine +
                    "2) Ability to submit and display stories" +
                    Environment.NewLine +
                    "3) Ability to download a graphic(*.png) with stories embedded" +
                    Environment.NewLine +
                    "4) log views and downloads" +
                    Environment.NewLine +
                    "5) Display a simple filtered chart of views and downloads over time ",
                    1000,
                    new Font("Calibri Light", 20, FontStyle.Regular, GraphicsUnit.Pixel)
                );

            string expectedResult =
                "The Challenge\r\n" +
                Environment.NewLine +
                "John enjoys writing stories and sharing them with his friend Pat.Pat thinks his writing should be preserved for " +
                "others to \r\nsee and enjoy. To this end, s / he has decided to build a website devoted to storing and preserving the " +
                "stories for others \r\nto see. In addition, Pat would like this website to return a copy of his writing embedded in a " +
                "graphic (*.png) image. This \r\nembedded image should display the stories enclosed in a rectangular box. (note, you do not " +
                "need to write stories, you \r\njust need to show your ability with brief text paragraphs.) Pat also wants to track how " +
                "popular the different stories are, \r\nso she is going to log story views and downloads. S / he will add a simple chart to " +
                "each story’s page to show the data.\r\n" +
                Environment.NewLine +
                "These are Pat’s requirements.\r\n" +
                Environment.NewLine +
                "1) Web site for stories\r\n" +
                Environment.NewLine +
                "2) Ability to submit and display stories\r\n" +
                Environment.NewLine +
                "3) Ability to download a graphic(*.png) with stories embedded\r\n" +
                Environment.NewLine +
                "4) log views and downloads\r\n" +
                Environment.NewLine +
                "5) Display a simple filtered chart of views and downloads over time ";

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
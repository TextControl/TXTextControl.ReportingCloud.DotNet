using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXTextControl.ReportingCloud;
using System.IO;
using System.Collections.Generic;

namespace UnitTestsCoreWrapper
{
    [TestClass]
    public class UnitTest1
    {
        string sUsername = "";
        string sPassword = "";
        Uri uriBasePath = new Uri("https://api.reporting.cloud/");

        [TestMethod()]
        public void GetDocumentTrackedChangesTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/tracked.tx");

                List<TrackedChange> trackedChanges = rc.Processing.Review.GetTrackedChanges(bDocument);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void RemoveDocumentTrackedChangeTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/tracked.tx");

                List<TrackedChange> trackedChanges = rc.Processing.Review.GetTrackedChanges(bDocument);

                int numTrackedChanges = trackedChanges.Count;

                TrackedChangeModifiedDocument modifiedDocument =
                    rc.Processing.Review.RemoveTrackedChange(bDocument, trackedChanges[0].Id, true);

                if (modifiedDocument.Removed == true)
                {
                    List<TrackedChange> trackedChangesModified =
                        rc.Processing.Review.GetTrackedChanges(modifiedDocument.Document);

                    Assert.IsFalse(trackedChanges.Count == trackedChangesModified.Count);
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void ReportingCloudAPIKeyTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);
                string sAPIKey;
                bool bKeyCreated = false;

                // create a new key, if no keys exist
                if (rc.GetAccountAPIKeys().Count == 0)
                {
                    sAPIKey = rc.CreateAccountAPIKey();
                    bKeyCreated = true;
                }
                else
                    sAPIKey = rc.GetAccountAPIKeys()[0].Key;

                // create new instance with new API Key
                ReportingCloud rc2 = new ReportingCloud(sAPIKey, uriBasePath);

                // check account settings
                var accountSettings = rc2.GetAccountSettings();

                Assert.IsFalse(accountSettings.MaxDocuments == 0);

                // remove created key
                if (bKeyCreated == true)
                    rc.DeleteAccountAPIKey(sAPIKey);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetAccountAPIKeysTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // create a new key
                string sAPIKey = rc.CreateAccountAPIKey();

                // get all keys
                List<APIKey> lAPIKeys = rc.GetAccountAPIKeys();

                // check, if at least 1 key is in list
                Assert.IsFalse(lAPIKeys.Count == 0);

                // clean up
                rc.DeleteAccountAPIKey(sAPIKey);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void AvailableDictionariesTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                string[] saDictionaries = rc.GetAvailableDictionaries();

                // check, if images are created
                Assert.IsFalse(saDictionaries.Length == 0);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void ShareDocumentTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                List<Template> lTemplates = rc.ListTemplates();

                string sSharedHash = rc.ShareDocument(lTemplates[0].TemplateName);

                // check, if images are created
                Assert.IsFalse(sSharedHash.Length == 0);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public void UploadTemplateTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void FindAndReplaceDocumentTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/replace_template.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // create a new FindAndReplaceBody object
                FindAndReplaceBody body = new FindAndReplaceBody();
                body.FindAndReplaceData = new List<string[]>()
                {
                    new string[] { "%%TextToReplace%%", "ReplacedString" },
                    new string[] { "%%SecondTextToReplace%%", "ReplacedString2" }
                };

                // merge the document
                byte[] results = rc.FindAndReplaceDocument(body, sTempFilename, ReturnFormat.HTML);

                string bHtmlDocument = System.Text.Encoding.UTF8.GetString(results);

                // check whether the created HTML contains the test string
                Assert.IsTrue(bHtmlDocument.Contains("ReplacedString"));

                // delete the template
                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void MergeDocumentTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                List<Invoice> invoices = new List<Invoice>();

                // create dummy data
                Invoice invoice = new Invoice();
                invoice.yourcompany_companyname = "Text Control, LLC";
                invoice.invoice_no = "Test_R667663";
                invoice.billto_name = "<html><strong>Test</strong> <em>Company</em></html>";

                Invoice invoice2 = new Invoice();
                invoice2.yourcompany_companyname = "Text Control 2, LLC";
                invoice2.invoice_no = "Test_R667663";
                invoice2.billto_name = "<html><strong>Test</strong> <em>Company</em></html>";

                invoices.Add(invoice);
                invoices.Add(invoice2);

                // create a new MergeBody object
                MergeBody body = new MergeBody();
                body.MergeData = invoices;

                MergeSettings settings = new MergeSettings();
                settings.Author = "Text Control GmbH";
                settings.MergeHtml = true;
                settings.Culture = "de-DE";

                body.MergeSettings = settings;

                // merge the document
                List<byte[]> results = rc.MergeDocument(body, sTempFilename, ReturnFormat.HTML, false, false);

                string bHtmlDocument = System.Text.Encoding.UTF8.GetString(results[0]);

                // check whether the created HTML contains the test string
                Assert.IsTrue(bHtmlDocument.Contains("Test_R667663"));

                // delete the template
                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void AppendDocumentTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // create a new MergeBody object
                AppendBody body = new AppendBody();

                body.Documents.Add(new AppendDocument()
                {
                    Document = File.ReadAllBytes("documents/invoice.tx"),
                DocumentDivider = DocumentDivider.None
                });

                body.Documents.Add(new AppendDocument()
                {
                    Document = File.ReadAllBytes("documents/sample_docx.docx"),
                    DocumentDivider = DocumentDivider.NewSection
                });

                DocumentSettings settings = new DocumentSettings();
                settings.Author = "Text Control GmbH";

                body.DocumentSettings = settings;

                // append the documents
                byte[] results = rc.AppendDocument(body, ReturnFormat.HTML, true);

                string bHtmlDocument = System.Text.Encoding.UTF8.GetString(results);

                // check whether the created HTML contains the test string
                Assert.IsTrue(bHtmlDocument.Contains("<title>ReportingCloud Test Mode</title>"));
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void ConvertDocumentTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");

                byte[] bHtml = rc.ConvertDocument(bDocument, ReturnFormat.HTML);

                Assert.IsTrue(System.Text.Encoding.UTF8.GetString(bHtml).Contains("INVOICE"));
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetTemplateInfoTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/test.doc");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".doc";
                rc.UploadTemplate(sTempFilename, bDocument);

                // get template information
                TemplateInfo templateInfo = rc.GetTemplateInfo(sTempFilename);

                // check, if images are created
                Assert.IsFalse(templateInfo.TemplateName == "");

                // delete temp file
                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetSuggestionsTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                string[] saSuggestions = rc.GetSuggestions("dooper", rc.GetAvailableDictionaries()[0], 10);

                // check, if images are created
                Assert.IsFalse(saSuggestions.Length == 0);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetAccountSettingsTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                AccountSettings settings = rc.GetAccountSettings();

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // check, if the count went up
                Assert.AreEqual(settings.UploadedTemplates + 1, rc.GetTemplateCount());

                // delete temp document
                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetTemplateCountTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // store current template number
                int iTemplateCount = rc.GetTemplateCount();

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // check, if the count went up
                Assert.AreEqual(iTemplateCount + 1, rc.GetTemplateCount());

                // delete temp document
                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void DownloadTemplateTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload local test document
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // download document
                byte[] bTemplate = rc.DownloadTemplate(sTempFilename);

                // compare documents
                Assert.IsNotNull(bTemplate);

                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetTemplatePageCountTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // check, if the count went up
                Assert.AreEqual(1, rc.GetTemplatePageCount(sTempFilename));

                // delete temp document
                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void ListTemplatesTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // list all templates
                List<Template> templates = rc.ListTemplates();
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void ListFonts()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // list all templates
                string[] fonts = rc.ListFonts();

                foreach (string font in fonts)
                {
                    Console.WriteLine(font);
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetDocumentThumbnailsTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");

                // create thumbnails
                List<string> images = rc.GetDocumentThumbnails(bDocument, 20, 1, 1, ImageFormat.PNG);

                // check, if images are created
                Assert.IsFalse((images.Count == 0));
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod()]
        public void GetTemplateThumbnailsTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // create thumbnails
                List<string> images = rc.GetTemplateThumbnails(sTempFilename, 20, 1, 1, ImageFormat.PNG);

                // check, if images are created
                Assert.IsFalse((images.Count == 0));

                // delete temp file
                rc.DeleteTemplate(sTempFilename);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        public class Invoice
        {
            public string yourcompany_companyname { get; set; }
            public string invoice_no { get; set; }
            public string billto_name { get; set; }
        }
    }
}

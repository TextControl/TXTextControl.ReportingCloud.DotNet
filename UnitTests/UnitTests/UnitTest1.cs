using TXTextControl.ReportingCloud;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace TXTextControl.ReportingCloud.Tests
{
    [TestClass()]
    public class ReportingCloudUnitTest
    {
        string sUsername = "username";
        string sPassword = "password";
        Uri uriBasePath = new Uri("https://api.reporting.cloud/");

        [TestMethod()]
        public void ReportingCloudTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);
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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // create thumbnails
                List<System.Drawing.Image> images = rc.GetTemplateThumbnails(sTempFilename, 20, 1, 1, ImageFormat.PNG);

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

        [TestMethod()]
        public void MergeDocumentTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // create dummy data
                Invoice invoice = new Invoice();
                invoice.yourcompany_companyname = "Text Control, LLC";
                invoice.invoice_no = "Test_R667663";

                // create a new MergeBody object
                MergeBody body = new MergeBody();
                body.MergeData = invoice;

                // merge the document
                List<byte[]> results = rc.MergeDocument(body, sTempFilename, ReturnFormat.HTML);

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
        public void UploadTemplateTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                // template exists?
                Assert.IsTrue(rc.TemplateExists(sTempFilename), "Template doesn't exist");

                rc.DeleteTemplate(sTempFilename);
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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

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
        public void GetAccountSettingsTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

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
        public void GetTemplatePageCountTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

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
        public void DeleteTemplateTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

                // upload test document
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, bDocument);

                if (rc.TemplateExists(sTempFilename) == true)
                {
                    // delete template
                    rc.DeleteTemplate(sTempFilename);

                    // check, if template has been deleted
                    Assert.IsFalse(rc.TemplateExists(sTempFilename), "Template is not deleted.");
                }
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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

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
        public void ListTemplatesTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword);

                // list all templates
                List<Template> templates = rc.ListTemplates();
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
    }

    public class Invoice
    {
        public string yourcompany_companyname { get; set; }
        public string invoice_no { get; set; }
    }
}

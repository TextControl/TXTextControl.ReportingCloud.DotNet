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
        Uri uriBasePath = new Uri("http://api.reporting.cloud");

        [TestMethod()]
        public void ReportingCloudTest()
        {
            try
            {
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);
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
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

                // create dummy data
                Invoice invoice = new Invoice();
                invoice.yourcompany_companyname = "Text Control, LLC";
                invoice.invoice_no = "Test_R667663";

                // create a new MergeBody object
                MergeBody body = new MergeBody();
                body.MergeData = invoice;

                // merge the document
                List<string> results = rc.MergeDocument(body, sTempFilename, ReturnFormat.HTML);

                // check whether the created HTML contains the test string
                Assert.IsTrue(results[0].Contains("Test_R667663"));

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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

                // download document
                string sTemplate = rc.DownloadTemplate(sTempFilename);

                // compare documents
                Assert.AreEqual(Convert.ToBase64String(bDocument), sTemplate, "Documents do not match");

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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");

                string sHtml = rc.ConvertDocument(Convert.ToBase64String(bDocument), ReturnFormat.HTML);

                Assert.IsTrue(sHtml.Contains("INVOICE"));
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
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

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
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload 1 more document with unique file name
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload test document
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

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
                ReportingCloud rc = new ReportingCloud(sUsername, sPassword, uriBasePath);

                // upload local test document
                byte[] bDocument = File.ReadAllBytes("documents/invoice.tx");
                string sTempFilename = "test" + Guid.NewGuid().ToString() + ".tx";
                rc.UploadTemplate(sTempFilename, Convert.ToBase64String(bDocument));

                // download document
                string sTemplate = rc.DownloadTemplate(sTempFilename);

                // compare documents
                Assert.AreEqual(Convert.ToBase64String(bDocument), sTemplate, "Documents do not match");

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
    }

    public class Invoice
    {
        public string yourcompany_companyname { get; set; }
        public string invoice_no { get; set; }
    }
}

/*-------------------------------------------------------------------------------------------------------------
 * module:			ReportingCloud .NET Wrapper (sync version)
 *
 * copyright:		© Text Control GmbH
 * version:			Reporting Cloud 1.0
 *-----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
#if NET45
using System.Net.Http.Formatting;
#endif

/// <summary>
/// This namespace contains classes for the Text Control ReportingCloud .NET wrapper
/// </summary>
namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** ReportingCloud **
    // This class contains the implementation of the ReportingCloud wrapper for .NET
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>Text Control ReportingCloud .NET wrapper class.</summary>
    public partial class ReportingCloud
    {
        /// <summary>
        /// This method returns a list of thumbnails of a specific template in the template storage.
        /// </summary>
        /// <param name="document">The document that should be used to create page thumbnails.</param>
        /// <param name="zoomFactor">The desired zoom factor of the thumbnails.</param>
        /// <param name="fromPage">The first page of the template that should be created as thumbnails.</param>
        /// <param name="toPage">The last page of the template that should be created as thumbnails.</param>
        /// <param name="imageFormat">The image format of the returned thumbnail images.</param>
#if NET45
    public List<System.Drawing.Image> GetDocumentThumbnails(byte[] document, int zoomFactor = 100,
        int fromPage = 1, int toPage = 0, ImageFormat imageFormat = ImageFormat.PNG)
    {
        // create a new list of System.Drawing.Image
        List<System.Drawing.Image> lImageThumbnails = new List<System.Drawing.Image>();

        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response =
                client.PostAsync("v1/document/thumbnails?zoomFactor=" + zoomFactor +
                "&fromPage=" + fromPage.ToString() +
                "&toPage=" + toPage.ToString() +
                "&imageFormat=" + imageFormat.ToString(), document, Helpers.TypeFormatter()).Result;

            // if sucessful, return the image list
            if (response.IsSuccessStatusCode)
            {
                List<string> results = response.Content.ReadAsAsync<List<string>>().Result;

                // create images from the Base64 encoded images
                foreach (string thumbnail in results)
                {
                    using (var ms = new MemoryStream(System.Convert.FromBase64String(thumbnail)))
                    {
                        lImageThumbnails.Add(System.Drawing.Image.FromStream(ms));
                    }
                }

                return lImageThumbnails;
            }
            else
            {
                // throw exception with the message from the endpoint
                throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
#else
        public List<string> GetDocumentThumbnails(byte[] document, int zoomFactor = 100,
            int fromPage = 1, int toPage = 0, ImageFormat imageFormat = ImageFormat.PNG)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response =
                    client.PostAsync("v1/document/thumbnails?zoomFactor=" + zoomFactor +
                                    "&fromPage=" + fromPage.ToString() +
                                    "&toPage=" + toPage.ToString() +
                                    "&imageFormat=" + imageFormat.ToString(), document, Helpers.TypeFormatter()).Result;

                // if sucessful, return the image list
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<string>>().Result;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }
#endif

        /// <summary>
        /// This method replaces strings in a document.
        /// </summary>
        /// <param name="findAndReplaceBody">The FindAndReplaceBody object contains the replace data, optionally a template and merge settings.</param>
        /// <param name="templateName">The name of the template in the template storage.</param>
        /// <param name="returnFormat">The document format of the resulting document.</param>
        /// <param name="test">Specifies whether it is a test run or not. A test run is not counted against the quota and created documents contain a watermark.</param>
        public byte[] FindAndReplaceDocument(FindAndReplaceBody findAndReplaceBody,
            string templateName = null,
            ReturnFormat returnFormat = ReturnFormat.PDF,
            bool test = false)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                // FindAndReplaceBody is posted as a JSON object
                HttpResponseMessage response = client.PostAsync("v1/document/findandreplace?templateName=" + templateName +
                "&returnFormat=" + returnFormat.ToString() + "&test=" + test.ToString(),
                findAndReplaceBody, Helpers.TypeFormatter()).Result;

                // if successful, return the document list
                if (response.IsSuccessStatusCode)
                {
                    return Convert.FromBase64String(response.Content.ReadAsAsync<string>().Result);
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method returns the hash value that is used to share documents using the portal.
        /// </summary>
        /// <param name="templateName">The name of the template in the template storage.</param>
        public string ShareDocument(string templateName = null)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response = client.GetAsync("v1/document/share?templateName=" + templateName).Result;

                // if successful, return the document list
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<string>().Result;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method merges a template with data.
        /// </summary>
        /// <param name="mergeBody">The MergeBody object contains the data, optionally a template and merge settings.</param>
        /// <param name="templateName">The name of the template in the template storage.</param>
        /// <param name="returnFormat">The document format of the resulting document.</param>
        /// <param name="append">Specifies whether the resulting documents should be appended or not.</param>
        /// <param name="test">Specifies whether it is a test run or not. A test run is not counted against the quota and created documents contain a watermark.</param>
        public List<byte[]> MergeDocument(MergeBody mergeBody,
            string templateName = null,
            ReturnFormat returnFormat = ReturnFormat.PDF,
            bool append = true,
            bool test = false)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                // MergeBody is posted as a JSON object
                HttpResponseMessage response = client.PostAsync("v1/document/merge?templateName=" + templateName +
                    "&returnFormat=" + returnFormat.ToString() +
                    "&append=" + append.ToString() + "&test=" + test.ToString(), mergeBody, Helpers.TypeFormatter()).Result;

                // if sucessful, return the image list
                if (response.IsSuccessStatusCode)
                {
                    List<byte[]> bResults = new List<byte[]>();

                    foreach (string sResult in response.Content.ReadAsAsync<List<string>>().Result)
                    {
                        bResults.Add(Convert.FromBase64String(sResult));
                    }

                    return bResults;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method appends documents to a single resulting document
        /// </summary>
        /// <param name="appendBody">The AppendBody object contains the documents for the append process.</param>
        /// <param name="returnFormat">The document format of the resulting document.</param>
        /// <param name="test">Specifies whether it is a test run or not. A test run is not counted against the quota and created documents contain a watermark.</param>
        public byte[] AppendDocument(AppendBody appendBody,
            ReturnFormat returnFormat = ReturnFormat.PDF,
            bool test = false)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                // MergeBody is posted as a JSON object
                HttpResponseMessage response = client.PostAsync("v1/document/append?returnFormat=" + returnFormat.ToString() +
                    "&test=" + test.ToString(), appendBody, Helpers.TypeFormatter()).Result;

                // if sucessful, return the image list
                if (response.IsSuccessStatusCode)
                {
                    string sResult = response.Content.ReadAsAsync<string>().Result;
                    return Convert.FromBase64String(sResult);
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method converts a document to another format.
        /// </summary>
        /// <param name="document">The source document data as a byte array.</param>
        /// <param name="returnFormat">The document format of the resulting document.</param>
        /// <param name="test">Specifies whether it is a test run or not. A test run is not counted against the quota and created documents contain a watermark.</param>
        public byte[] ConvertDocument(byte[] document, ReturnFormat returnFormat, bool test = false)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                // Template is posted as a JSON object
                HttpResponseMessage response = client.PostAsync("v1/document/convert?returnFormat=" + returnFormat + "&test=" + test.ToString(),
                    Convert.ToBase64String(document), Helpers.TypeFormatter()).Result;

                // if sucessful, return the document list
                if (response.IsSuccessStatusCode)
                {
                    return Convert.FromBase64String(response.Content.ReadAsAsync<string>().Result);
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }
    }
}

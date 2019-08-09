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
        /// <param name="templateName">The name of the template that should be used to create the thumbnails.</param>
        /// <param name="zoomFactor">The desired zoom factor of the thumbnails.</param>
        /// <param name="fromPage">The first page of the template that should be created as thumbnails.</param>
        /// <param name="toPage">The last page of the template that should be created as thumbnails.</param>
        /// <param name="imageFormat">The image format of the returned thumbnail images.</param>
#if NET45
    public List<System.Drawing.Image> GetTemplateThumbnails(string templateName, int zoomFactor,
    int fromPage = 1, int toPage = 0, ImageFormat imageFormat = ImageFormat.PNG)
    {
        // create a new list of System.Drawing.Image
        List<System.Drawing.Image> lImageThumbnails = new List<System.Drawing.Image>();

        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response =
                client.GetAsync("v1/templates/thumbnails?templateName=" + templateName +
                "&zoomFactor=" + zoomFactor +
                "&fromPage=" + fromPage.ToString() +
                "&toPage=" + toPage.ToString() +
                "&imageFormat=" + imageFormat.ToString()).Result;

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
        public List<string> GetTemplateThumbnails(string templateName, int zoomFactor,
            int fromPage = 1, int toPage = 0, ImageFormat imageFormat = ImageFormat.PNG)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response =
                    client.GetAsync("v1/templates/thumbnails?templateName=" + templateName +
                                    "&zoomFactor=" + zoomFactor +
                                    "&fromPage=" + fromPage.ToString() +
                                    "&toPage=" + toPage.ToString() +
                                    "&imageFormat=" + imageFormat.ToString()).Result;

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
        /// This method uploads a template to the template storage.
        /// </summary>
        /// <param name="templateName">The destination name of the template in the template storage.</param>
        /// <param name="template">The template data encoded as a Base64 string.</param>
        public void UploadTemplate(string templateName, byte[] template)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                // Template is posted as a JSON object
                HttpResponseMessage response = client.PostAsync("v1/templates/upload?templateName=" + templateName,
                    Convert.ToBase64String(template), Helpers.TypeFormatter()).Result;

                // throw exception with the message from the endpoint
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method returns template information.
        /// </summary>
        public TemplateInfo GetTemplateInfo(string templateName)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint
                // no query parameters
                HttpResponseMessage response = client.GetAsync("v1/templates/info?templateName=" + templateName).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<TemplateInfo>().Result;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method returns the number of templates in the template storage.
        /// </summary>
        public int GetTemplateCount()
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint
                // no query parameters
                HttpResponseMessage response = client.GetAsync("v1/templates/count").Result;

                // return an int, if successful
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method checks whether a template exists or not in the template storage.
        /// </summary>
        /// <param name="templateName">The source document data encoded as a Base64 string.</param>
        public bool TemplateExists(string templateName)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint
                // no query parameters
                HttpResponseMessage response = client.GetAsync("v1/templates/exists?templateName=" + templateName).Result;

                // return an int, if successful
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<bool>().Result;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method deletes a template from the template storage.
        /// </summary>
        /// <param name="templateName">The name of the template in the template storage.</param>
        public void DeleteTemplate(string templateName)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response = client.DeleteAsync("v1/templates/delete/?templateName=" + templateName).Result;

                // throw exception with the message from the endpoint
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method returns a template from the template storage as a byte array.
        /// </summary>
        /// <param name="templateName">The name of the template in the template storage.</param>
        public byte[] DownloadTemplate(string templateName)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response = client.GetAsync("v1/templates/download/?templateName=" + templateName).Result;

                // return an the document, if successful
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
        /// This method returns the number of pages of a template in the template storage.
        /// </summary>
        /// <param name="templateName">The name of the template in the template storage.</param>
        public int GetTemplatePageCount(string templateName)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response = client.GetAsync("v1/templates/pagecount/?templateName=" + templateName).Result;

                // return an the document, if successful
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<int>().Result;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /// <summary>
        /// This method lists all templates stored in the template storage.
        /// </summary>
        public List<Template> ListTemplates()
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
            {
                // set the endpoint
                HttpResponseMessage response = client.GetAsync("v1/templates/list/").Result;

                // return an the list, if succuessful
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<Template>>().Result;
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

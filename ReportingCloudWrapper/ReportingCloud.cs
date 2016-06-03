/*-------------------------------------------------------------------------------------------------------------
 * module:			ReportingCloud .NET Wrapper (sync version)
 *
 * copyright:		© Text Control GmbH
 * version:			Reporting Cloud 1.0
 *-----------------------------------------------------------------------------------------------------------*/

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace TXTextControl.ReportingCloud
{
    /*-------------------------------------------------------------------------------------------------------
    // ** ReportingCloud **
    // This class contains the implementation of the ReportingCloud wrapper for .NET
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>Text Control ReportingCloud .NET Wrapper class.</summary>
    public class ReportingCloud
    {
    /*-------------------------------------------------------------------------------------------------------
    // ** ReportingCloud Constructor **
    // Only registered users with a valid Username, Password are allowed
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// ReportingCloud constructor. Use your ReportingCloud credentials and the Web API base URL
    /// to create a new instance of ReportingCloud.
    /// </summary>
    /// <param name="Username">The username (e-mail address) of your ReportingCloud account.</param>
    /// <param name="Password">The password of your ReportingCloud account.</param>
    /// <param name="WebApiBaseUrl">The Web API base URL of ReportingCloud. This Base URL is listed here: http://api.reporting.cloud/documentation/reference/</param>
    public ReportingCloud(string Username, string Password, Uri WebApiBaseUrl)
    {
        m_sUsername = Username;
        m_sPassword = Password;
        m_sWebApiBaseUrl = WebApiBaseUrl;
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** Member fields **
    *-----------------------------------------------------------------------------------------------------*/
    private string m_sUsername;
    private string m_sPassword;
    private Uri m_sWebApiBaseUrl;
    JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();

    /*-------------------------------------------------------------------------------------------------------
    // ** Properties **
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// The username (e-mail address) of your ReportingCloud account.
    /// </summary>
    public string Username
    {
        get { return m_sUsername; }
        set { m_sUsername = value; }
    }

    /// <summary>
    /// The password of your ReportingCloud account.
    /// </summary>
    public string Password
    {
        get { return m_sPassword; }
        set { m_sPassword = value; }
    }

    /// <summary>
    /// The Web API base URL of ReportingCloud. This Base URL is listed here: http://api.reporting.cloud/documentation/reference/
    /// </summary>
    public Uri WebApiBaseUrl
    {
        get { return m_sWebApiBaseUrl; }
        set { m_sWebApiBaseUrl = value; }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** GetTemplateThumbnails **
    // This method implements the "v1/templates/thumbnails" Web API call
    //
    // Parameters:
    //  - string Filename
    //  - int ZoomFactor
    //  - int FromPage
    //  - int ToPage
    //
    // Return value: A List of System.Drawing.Image
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns a list of thumbnails of a specific template.
    /// </summary>
    /// <param name="TemplateName">The name of the template that should be used to create the thumbnails.</param>
    /// <param name="ZoomFactor">The desired zoom factor of the thumbnails.</param>
    /// <param name="FromPage">The first page of the template that should be created as thumbnails.</param>
    /// <param name="ToPage">The last page of the template that should be created as thumbnails.</param>
    /// <param name="ImageFormat">The image format of the returned thumbnail images.</param>
    public List<System.Drawing.Image> GetTemplateThumbnails(string TemplateName, int ZoomFactor,
        int FromPage = 1, int ToPage = 0, ImageFormat ImageFormat = ImageFormat.PNG)
    {
        // create a new list of System.Drawing.Image
        List<System.Drawing.Image> lImageThumbnails = new List<System.Drawing.Image>();

        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response =
                client.GetAsync("v1/templates/thumbnails?templateName=" + TemplateName +
                "&zoomFactor=" + ZoomFactor +
                "&fromPage=" + FromPage.ToString() +
                "&toPage=" + ToPage.ToString() +
                "&imageFormat=" + ImageFormat.ToString()).Result;

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

    /*-------------------------------------------------------------------------------------------------------
    // ** MergeDocument **
    // This method implements the "v1/document/merge" Web API call
    //
    // Parameters:
    //  - MergeBody MergeBody
    //  - string TemplateName (default = null)
    //  - ReturnFormat ReturnFormat (default = PDF)
    //  - bool Append (default = true)
    //
    // Return value: A List of strings (the documents as Base64 encoded strings)
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method merges a template with data.
    /// </summary>
    /// <param name="MergeBody">The MergeBody object contains the data, optionally a template and merge settings.</param>
    /// <param name="TemplateName">The name of the template in the template storage.</param>
    /// <param name="ReturnFormat">The document format of the resulting document.</param>
    /// <param name="Append">Specifies whether the resulting documents should be appended or not.</param>
    public List<string> MergeDocument(MergeBody MergeBody,
        string TemplateName = null,
        ReturnFormat ReturnFormat = ReturnFormat.PDF,
        bool Append = true)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            // MergeBody is posted as a JSON object
            HttpResponseMessage response = client.PostAsync("v1/document/merge?templateName=" + TemplateName +
                "&returnFormat=" + ReturnFormat.ToString() +
                "&append=" + Append.ToString(), MergeBody, formatter).Result;

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

    /*-------------------------------------------------------------------------------------------------------
    // ** UploadTemplate **
    // This method implements the "v1/templates/upload" Web API call
    //
    // Parameters:
    //  - string TemplateName 
    //  - string Template
    //
    // Return value: void
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method uploads a template to the template storage.
    /// </summary>
    /// <param name="TemplateName">The destination name of the template in the template storage.</param>
    /// <param name="Template">The template data encoded as a Base64 string.</param>
    public void UploadTemplate(string TemplateName, string Template)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            // Template is posted as a JSON object
            HttpResponseMessage response = client.PostAsync("v1/templates/upload?templateName=" + TemplateName,
                Template, formatter).Result;

            // throw exception with the message from the endpoint
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** ConvertDocument **
    // This method implements the "v1/document/convert" Web API call
    //
    // Parameters:
    //  - ReturnFormat ReturnFormat 
    //  - string Template
    //
    // Return value: void
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method converts a document to another format.
    /// </summary>
    /// <param name="Document">The source document data encoded as a Base64 string.</param>
    /// <param name="ReturnFormat">The document format of the resulting document.</param>
    public string ConvertDocument(string Document, ReturnFormat ReturnFormat)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            // Template is posted as a JSON object
            HttpResponseMessage response = client.PostAsync("v1/document/convert?returnFormat=" + ReturnFormat,
                Document, formatter).Result;

            // if sucessful, return the image list
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

    /*-------------------------------------------------------------------------------------------------------
    // ** GetAccountSettings **
    // This method implements the "v1/account/settings" Web API call
    //
    // Return value: AccountSettings object
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns the account settings.
    /// </summary>
    public AccountSettings GetAccountSettings()
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint
            // no query parameters
            HttpResponseMessage response = client.GetAsync("v1/account/settings").Result;

            // return an ActionSettings object, if sucessful
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<AccountSettings>().Result;
            }
            else
            {
                // throw exception with the message from the endpoint
                throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** CountTemplates **
    // This method implements the "v1/templates/count" Web API call
    //
    // Return value: int
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns the number of templates in the template storage.
    /// </summary>
    public int GetTemplateCount()
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** TemplateExists **
    // This method implements the "v1/templates/exists" Web API call
    //
    // Return value: bool
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method checks whether a template exists or not in the template storage.
    /// </summary>
    /// <param name="TemplateName">The source document data encoded as a Base64 string.</param>
    public bool TemplateExists(string TemplateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint
            // no query parameters
            HttpResponseMessage response = client.GetAsync("v1/templates/exists?templateName=" + TemplateName).Result;

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

    /*-------------------------------------------------------------------------------------------------------
    // ** DeleteTemplate **
    // This method implements the "v1/templates/delete" Web API call
    //
    // Parameters:
    //  - string TemplateName
    // Return value: void
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method delete a template from the template storage.
    /// </summary>
    /// <param name="TemplateName">The name of the template in the template storage.</param>
    public void DeleteTemplate(string TemplateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response = client.DeleteAsync("v1/templates/delete/?templateName=" + TemplateName).Result;

            // throw exception with the message from the endpoint
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** DownloadTemplate **
    // This method implements the "v1/templates/download" Web API call
    //
    // Parameters:
    //  - string TemplateName
    // Return value: string (the document as a Base64 string)
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns a template from the template storage as a Base64 encoded string.
    /// </summary>
    /// <param name="TemplateName">The name of the template in the template storage.</param>
    public string DownloadTemplate(string TemplateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response = client.GetAsync("v1/templates/download/?templateName=" + TemplateName).Result;

            // return an the document, if successful
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

    /*-------------------------------------------------------------------------------------------------------
    // ** GetTemplatePageCount **
    // This method implements the "v1/templates/pagecount" Web API call
    //
    // Parameters:
    //  - string TemplateName
    // Return value: int
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns the number of pages of a template in the temnplate storage.
    /// </summary>
    /// <param name="TemplateName">The name of the template in the template storage.</param>
    public int GetTemplatePageCount(string TemplateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response = client.GetAsync("v1/templates/pagecount/?templateName=" + TemplateName).Result;

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

    /*-------------------------------------------------------------------------------------------------------
    // ** Templates **
    // This method implements the "v1/templates/list" Web API call
    //
    // Return value: List of Template objects
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method lists all templates stored in the template storage.
    /// </summary>
        public List<Template> ListTemplates()
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** Helpers **
    *-----------------------------------------------------------------------------------------------------*/

    /*-------------------------------------------------------------------------------------------------------
    // ** CreateHttpClient **
    // This factory method creates and returns a HttpClient object with the
    // proper headers, the base URL and the authorization
    *-----------------------------------------------------------------------------------------------------*/
    private HttpClient CreateHttpClient()
    {
        // add the CamelCasePropertyNamesContractResolver to convert
        // the .NET parameter to JSON camelCase formatting
        formatter.SerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        HttpClient client = new HttpClient();

        client.BaseAddress = m_sWebApiBaseUrl;
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodeTo64(m_sUsername + ":" + m_sPassword));

        return client;
    }
        
    /*-------------------------------------------------------------------------------------------------------
    // ** EncodeTo64 **
    // This static method encodes a string to Base64
    *-----------------------------------------------------------------------------------------------------*/
    static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** Template **
    // This class implements the structure of the returned Web Api Template object
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of a template in the template storage.
    /// </summary>
    /// <member name="TemplateName">The name of the template in the template storage.</member>
    /// <param name="Modified">The name of the template in the template storage.</param>
    /// <param name="Size">The name of the template in the template storage.</param>
    public class Template
    {
        public string TemplateName { get; set; }
        public DateTime Modified { get; set; }
        public long Size { get; set; }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** AccountSettings **
    // This class implements the structure of the returned Web Api AccountSettings object
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud account settings.
    /// </summary>
    public class AccountSettings
    {
        public string SerialNumber { get; set; }
        public int CreatedDocuments { get; set; }
        public int UploadedTemplates { get; set; }
        public int MaxDocuments { get; set; }
        public int MaxTemplates { get; set; }
        public DateTime? ValidUntil { get; set; }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** MergeBody **
    // This class implements the structure request object MergeBody
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud merge body used in the Merge method.
    /// </summary>
    public class MergeBody
    {
        public object MergeData { get; set; }
        public string Template { get; set; }
        public MergeSettings MergeSettings { get; set; }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** MergeSettings **
    // This class implements the structure request object MergeSettings
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This class provides the structure of the ReportingCloud MergeSettings used in the Merge method.
    /// </summary>
    public class MergeSettings
    {
        public bool? RemoveEmptyFields { get; set; }
        public bool? RemoveEmptyBlocks { get; set; }
        public bool? RemoveEmptyImages { get; set; }
        public bool? RemoveTrailingWhitespace { get; set; }

        public string Author { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatorApplication { get; set; }
        public string DocumentSubject { get; set; }
        public string DocumentTitle { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string UserPassword { get; set; }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** ReturnFormat **
    // This enum lists all possible return formats for the Merge method
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This enumeration provides the supported document return formats.
    /// </summary>
    public enum ReturnFormat
    {
        PDF,
        PDFA,
        DOC,
        DOCX,
        RTF,
        TX,
        HTML
    }

    /*-------------------------------------------------------------------------------------------------------
   // ** ImageFormat **
   // This enum lists all possible image formats for the thumbnails
   *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This enumeration provides the supported image formats.
    /// </summary>
    public enum ImageFormat
    {
        JPG,
        PNG,
        BMP,
        GIF
    }
}

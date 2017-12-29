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
    /// <param name="username">The username (e-mail address) of your ReportingCloud account.</param>
    /// <param name="password">The password of your ReportingCloud account.</param>
    /// <param name="webApiBaseUrl">The Web API base URL of ReportingCloud. This Base URL is listed here: http://api.reporting.cloud/documentation/reference/</param>
    public ReportingCloud(string username, string password, Uri webApiBaseUrl)
    {
        m_sUsername = username;
        m_sPassword = password;
        m_sWebApiBaseUrl = webApiBaseUrl;
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** ReportingCloud Constructor **
    // Only registered users with a valid Username, Password are allowed
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// ReportingCloud constructor. Use your ReportingCloud credentials
    /// to create a new instance of ReportingCloud.
    /// </summary>
    /// <param name="username">The username (e-mail address) of your ReportingCloud account.</param>
    /// <param name="password">The password of your ReportingCloud account.</param>
    public ReportingCloud(string username, string password)
    {
        m_sUsername = username;
        m_sPassword = password;
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** ReportingCloud Constructor **
    // Only registered users with a valid Username, Password are allowed
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// ReportingCloud constructor. Use your ReportingCloud credentials
    /// to create a new instance of ReportingCloud.
    /// </summary>
    /// <param name="apikey"> An active and valid API Key that should be used for the transactions.</param>
    public ReportingCloud(string apikey)
    {
        m_sAPIKey = apikey;
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** ReportingCloud Constructor **
    // Only registered users with a valid Username, Password are allowed
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// ReportingCloud constructor. Use your ReportingCloud credentials
    /// to create a new instance of ReportingCloud.
    /// </summary>
    /// <param name="apikey"> An active and valid API Key that should be used for the transactions.</param>
    /// <param name="webApiBaseUrl">The Web API base URL of ReportingCloud. This Base URL is listed here: http://api.reporting.cloud/documentation/reference/</param>
    public ReportingCloud(string apikey, Uri webApiBaseUrl)
    {
        m_sAPIKey = apikey;
        m_sWebApiBaseUrl = webApiBaseUrl;
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** Member fields **
    *-----------------------------------------------------------------------------------------------------*/
    private string m_sUsername;
    private string m_sPassword;
    private string m_sAPIKey;

    private Uri m_sWebApiBaseUrl = new Uri("https://api.reporting.cloud");
    private JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();

    /*-------------------------------------------------------------------------------------------------------
    // ** Properties **
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// The username (e-mail address) of the ReportingCloud account.
    /// </summary>
    public string Username
    {
        get { return m_sUsername; }
        set { m_sUsername = value; }
    }

    /// <summary>
    /// An active and valid API Key that should be used for the transactions.
    /// </summary>
    public string APIKey
    {
        get { return m_sAPIKey; }
        set { m_sAPIKey = value; }
    }

    /// <summary>
    /// The password of the ReportingCloud account.
    /// </summary>
    public string Password
    {
        get { return m_sPassword; }
        set { m_sPassword = value; }
    }

    /// <summary>
    /// The Web API base URL of ReportingCloud. Available URLs are listed here: http://api.reporting.cloud/documentation/reference/
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
    /// This method returns a list of thumbnails of a specific template in the template storage.
    /// </summary>
    /// <param name="templateName">The name of the template that should be used to create the thumbnails.</param>
    /// <param name="zoomFactor">The desired zoom factor of the thumbnails.</param>
    /// <param name="fromPage">The first page of the template that should be created as thumbnails.</param>
    /// <param name="toPage">The last page of the template that should be created as thumbnails.</param>
    /// <param name="imageFormat">The image format of the returned thumbnail images.</param>
    public List<System.Drawing.Image> GetTemplateThumbnails(string templateName, int zoomFactor,
    int fromPage = 1, int toPage = 0, ImageFormat imageFormat = ImageFormat.PNG)
    {
        // create a new list of System.Drawing.Image
        List<System.Drawing.Image> lImageThumbnails = new List<System.Drawing.Image>();

        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** FindAndReplaceDocument **
    // This method implements the "v1/document/findandreplace" Web API call
    //
    // Parameters:
    //  - FindAndReplaceBody FindAndReplaceBody
    //  - string TemplateName (default = null)
    //  - ReturnFormat ReturnFormat (default = PDF)
    //
    // Return value: A List of byte[]
    *-----------------------------------------------------------------------------------------------------*/
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
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            // FindAndReplaceBody is posted as a JSON object
            HttpResponseMessage response = client.PostAsync("v1/document/findandreplace?templateName=" + templateName +
            "&returnFormat=" + returnFormat.ToString() + "&test=" + test.ToString(),
            findAndReplaceBody, formatter).Result;

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

    /*-------------------------------------------------------------------------------------------------------
    // ** ShareDocument **
    // This method implements the "v1/document/share" Web API call
    //
    // Parameters:
    //  - string TemplateName (default = null)
    //
    // Return value: string
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns the hash value that is used to share documents using the portal.
    /// </summary>
    /// <param name="templateName">The name of the template in the template storage.</param>
    public string ShareDocument(string templateName = null)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** GetAccountAPIKeys **
    // This method implements the "v1/account/apikeys" Web API call
    //
    // Return value: APIKey[]
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns the available API Keys for the current account
    /// </summary>
    public List<APIKey> GetAccountAPIKeys()
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response = client.GetAsync("v1/account/apikeys").Result;

            // if successful, return the document list
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<APIKey>>().Result;
            }
            else
            {
                // throw exception with the message from the endpoint
                throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** CreateAccountAPIKey **
    // This method implements the PUT "v1/account/apikey" Web API call
    //
    // Return value: string
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method creates and returns a valid API Key for the current account
    /// </summary>
    public string CreateAccountAPIKey()
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response = client.PutAsync("v1/account/apikey", null).Result;

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

    /*-------------------------------------------------------------------------------------------------------
    // ** DeleteAccountAPIKey **
    // This method implements the DELETE "v1/account/apikey" Web API call
    //
    // Return value: bool
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method deletes an available API Key that belongs to the current account
    /// </summary>
    /// <param name="key">The API Key that should be deleted.</param>
    public bool DeleteAccountAPIKey(string key)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            HttpResponseMessage response = client.DeleteAsync("v1/account/apikey?key=" + key).Result;

            // if successful, return the document list
            if (response.IsSuccessStatusCode)
            {
                return true;
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
    // Return value: A List of byte[]
    *-----------------------------------------------------------------------------------------------------*/
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
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            // MergeBody is posted as a JSON object
            HttpResponseMessage response = client.PostAsync("v1/document/merge?templateName=" + templateName +
                "&returnFormat=" + returnFormat.ToString() +
                "&append=" + append.ToString() + "&test=" + test.ToString(), mergeBody, formatter).Result;

            // if sucessful, return the image list
            if (response.IsSuccessStatusCode)
            {
                List<byte[]> bResults = new List<byte[]>();

                foreach (string sResult in response.Content.ReadAsAsync<List<string>>().Result)
                {
                    bResults.Add(Convert.FromBase64String(sResult));
                }

                return bResults;
                //return response.Content.ReadAsAsync<List<string>>().Result;
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
    /// <param name="templateName">The destination name of the template in the template storage.</param>
    /// <param name="template">The template data encoded as a Base64 string.</param>
    public void UploadTemplate(string templateName, byte[] template)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            // Template is posted as a JSON object
            HttpResponseMessage response = client.PostAsync("v1/templates/upload?templateName=" + templateName,
                Convert.ToBase64String(template), formatter).Result;

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
    /// <param name="document">The source document data as a byte array.</param>
    /// <param name="returnFormat">The document format of the resulting document.</param>
    /// <param name="test">Specifies whether it is a test run or not. A test run is not counted against the quota and created documents contain a watermark.</param>
    public byte[] ConvertDocument(byte[] document, ReturnFormat returnFormat, bool test = false)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint and pass the query paramaters
            // Template is posted as a JSON object
            HttpResponseMessage response = client.PostAsync("v1/document/convert?returnFormat=" + returnFormat + "&test=" + test.ToString(),
                Convert.ToBase64String(document), formatter).Result;

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

    /*-------------------------------------------------------------------------------------------------------
    // ** GetTemplateInfo **
    // This method implements the "v1/templates/info" Web API call
    //
    // Return value: TemplateInfo object
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns template information.
    /// </summary>
    public TemplateInfo GetTemplateInfo(string templateName)
    {
    // create a new HttpClient using the Factory method CreateHttpClient
    using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** CheckText **
    // This method implements the "v1/proofing/check" Web API call
    //
    // Return value: List<IncorrectWord>
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method checks text for spelling errors.
    /// </summary>
    /// <param name="text">Specifies the text to spell check.</param>
    /// <param name="language">The language that is used to spell check the specified text.</param>
    public List<IncorrectWord> CheckText(string text, string language)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint
            // no query parameters
            HttpResponseMessage response = client.GetAsync("v1/proofing/check?text=" + text + "&language=" + language).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<IncorrectWord>>().Result;
            }
            else
            {
                // throw exception with the message from the endpoint
                throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** GetAvailableDictionaries **
    // This method implements the "v1/proofing/availabledictionaries" Web API call
    //
    // Return value: string[]
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns the available dictionaries.
    /// </summary>
    public string[] GetAvailableDictionaries()
    {
    // create a new HttpClient using the Factory method CreateHttpClient
    using (HttpClient client = CreateHttpClient())
    {
        // set the endpoint
        // no query parameters
        HttpResponseMessage response = client.GetAsync("v1/proofing/availabledictionaries").Result;

        if (response.IsSuccessStatusCode)
        {
            return response.Content.ReadAsAsync<string[]>().Result;
        }
        else
        {
            // throw exception with the message from the endpoint
            throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
        }
    }
    }

    /*-------------------------------------------------------------------------------------------------------
    // ** GetSuggestions **
    // This method implements the "v1/proofing/suggestions" Web API call
    //
    // Return value: string[]
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns suggestions for a misspelled word.
    /// </summary>
    /// <param name="word">Specifies the incorrect word that has to be determined for suggestions.</param>
    /// <param name="language">The language that is used to create suggestions for the specified incorrect word.</param>
    /// <param name="max">Specifies the maximum number of suggestions that has to be determined.</param>
    public string[] GetSuggestions(string word, string language, int max)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint
            // no query parameters
            HttpResponseMessage response = client.GetAsync("v1/proofing/suggestions?word=" + word + "&language=" + language + "&max=" + max).Result;

                if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<string[]>().Result;
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
    /// <param name="templateName">The source document data encoded as a Base64 string.</param>
    public bool TemplateExists(string templateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** DeleteTemplate **
    // This method implements the "v1/templates/delete" Web API call
    //
    // Parameters:
    //  - string TemplateName
    // Return value: void
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method deletes a template from the template storage.
    /// </summary>
    /// <param name="templateName">The name of the template in the template storage.</param>
    public void DeleteTemplate(string templateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** DownloadTemplate **
    // This method implements the "v1/templates/download" Web API call
    //
    // Parameters:
    //  - string TemplateName
    // Return value: byte[]
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns a template from the template storage as a byte array.
    /// </summary>
    /// <param name="templateName">The name of the template in the template storage.</param>
    public byte[] DownloadTemplate(string templateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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

    /*-------------------------------------------------------------------------------------------------------
    // ** GetTemplatePageCount **
    // This method implements the "v1/templates/pagecount" Web API call
    //
    // Parameters:
    //  - string TemplateName
    // Return value: int
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method returns the number of pages of a template in the template storage.
    /// </summary>
    /// <param name="templateName">The name of the template in the template storage.</param>
    public int GetTemplatePageCount(string templateName)
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
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
    // ** ListFonts **
    // This method implements the "v1/fonts/list" Web API call
    //
    // Return value: string array of font names
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method lists all available fonts.
    /// </summary>
    public string[] ListFonts()
    {
        // create a new HttpClient using the Factory method CreateHttpClient
        using (HttpClient client = CreateHttpClient())
        {
            // set the endpoint
            HttpResponseMessage response = client.GetAsync("v1/fonts/list/").Result;

            // return an the list, if succuessful
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<string[]>().Result;
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

        if (m_sAPIKey != null)
        {
            client.DefaultRequestHeaders.Add("Authorization", "ReportingCloud-APIKey " + m_sAPIKey);
        }
        else
        {
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodeTo64(m_sUsername + ":" + m_sPassword));
        }

        return client;
    }
        
    /*-------------------------------------------------------------------------------------------------------
    // ** EncodeTo64 **
    // This static method encodes a string to Base64
    *-----------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// This method encodes a string to a Base64 string.
    /// </summary>
    /// <param name="toEncode">The string to encode.</param>
    /// <returns></returns>
    static public string EncodeTo64(string toEncode)
    {
        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
        return System.Convert.ToBase64String(toEncodeAsBytes);
    }

    }
}

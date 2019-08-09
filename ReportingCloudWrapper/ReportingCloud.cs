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
        internal string m_sUsername = null;
        internal string m_sPassword = null;
        internal string m_sAPIKey = null;

        internal Uri m_sWebApiBaseUrl = new Uri("https://api.reporting.cloud");

        public Processing Processing => new Processing(this);

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

        /// <summary>
        /// This method returns the available API Keys for the current account
        /// </summary>
        public List<APIKey> GetAccountAPIKeys()
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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

        /// <summary>
        /// This method creates and returns a valid API Key for the current account
        /// </summary>
        public string CreateAccountAPIKey()
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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

        /// <summary>
        /// This method deletes an available API Key that belongs to the current account
        /// </summary>
        /// <param name="key">The API Key that should be deleted.</param>
        public bool DeleteAccountAPIKey(string key)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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

        /// <summary>
        /// This method checks text for spelling errors.
        /// </summary>
        /// <param name="text">Specifies the text to spell check.</param>
        /// <param name="language">The language that is used to spell check the specified text.</param>
        public List<IncorrectWord> CheckText(string text, string language)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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

        /// <summary>
        /// This method returns the available dictionaries.
        /// </summary>
        public string[] GetAvailableDictionaries()
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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

        /// <summary>
        /// This method returns suggestions for a misspelled word.
        /// </summary>
        /// <param name="word">Specifies the incorrect word that has to be determined for suggestions.</param>
        /// <param name="language">The language that is used to create suggestions for the specified incorrect word.</param>
        /// <param name="max">Specifies the maximum number of suggestions that has to be determined.</param>
        public string[] GetSuggestions(string word, string language, int max)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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

        /// <summary>
        /// This method returns the account settings.
        /// </summary>
        public AccountSettings GetAccountSettings()
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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

        /// <summary>
        /// This method lists all available fonts.
        /// </summary>
        public string[] ListFonts()
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(m_sWebApiBaseUrl, m_sUsername, m_sPassword, m_sAPIKey))
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
    }
}

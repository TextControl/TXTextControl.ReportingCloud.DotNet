using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TXTextControl.ReportingCloud
{
    public class Review
    {
        ReportingCloud m_Parent;

        public Review(ReportingCloud Parent)
        {
            m_Parent = Parent;
        }

        /*-------------------------------------------------------------------------------------------------------
        // ** GetDocumentTrackedChanges **
        // This method implements the "v1/processing/review/trackedchanges" Web API call
        //
        // Parameters:
        //  - byte[] document
        //
        // Return value: List<TrackedChange>
        *-----------------------------------------------------------------------------------------------------*/
        /// <summary>
        /// This method returns all tracked changes objects in a document.
        /// </summary>
        /// <param name="document">The document that should be used to return the tracked changes.</param>
        public List<TrackedChange> GetTrackedChanges(byte[] document)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(
                m_Parent.m_sWebApiBaseUrl, 
                m_Parent.m_sUsername, 
                m_Parent.m_sPassword, 
                m_Parent.m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response = client.PostAsync("v1/processing/review/trackedchanges", document, Helpers.TypeFormatter()).Result;

                // if successful, return the document list
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<TrackedChange>>().Result;
                }
                else
                {
                    // throw exception with the message from the endpoint
                    throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        /*-------------------------------------------------------------------------------------------------------
        // ** RemoveTrackedChange **
        // This method implements the "v1/processing/review/removetrackedchange" Web API call
        //
        // Parameters:
        //  - byte[] document
        //  - int id
        //  - bool accept
        //
        // Return value: ModifiedDocument
        *-----------------------------------------------------------------------------------------------------*/
        /// <summary>
        /// This method removes a specific tracked change from a document and returns the modified document.
        /// </summary>
        /// <param name="document">The document that should be used to return the tracked changes.</param>
        /// <param name="id">Specifies the id of the tracked changes that should be removed.</param>
        /// <param name="accept">Specifies whether the tracked changed should be accepted or not (reject).</param>
        public TrackedChangeModifiedDocument RemoveTrackedChange(byte[] document, int id, bool accept)
        {
            // create a new HttpClient using the Factory method CreateHttpClient
            using (HttpClient client = Helpers.CreateHttpClient(
               m_Parent.m_sWebApiBaseUrl,
               m_Parent.m_sUsername,
               m_Parent.m_sPassword,
               m_Parent.m_sAPIKey))
            {
                // set the endpoint and pass the query paramaters
                HttpResponseMessage response = client.PostAsync("v1/processing/review/removetrackedchange?id=" + id + "&accept=" + accept, document, Helpers.TypeFormatter()).Result;

                // if successful, return the document list
                if (response.IsSuccessStatusCode)
                {
                    var returnValue = response.Content.ReadAsAsync<object>().Result;

                    TrackedChangeModifiedDocument modifiedDocument = new TrackedChangeModifiedDocument()
                    {
                        Removed = (bool)((JObject)returnValue)["Removed"],
                        Document = Convert.FromBase64String((string)((JObject)returnValue)["Document"])
                    };

                    return modifiedDocument;
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
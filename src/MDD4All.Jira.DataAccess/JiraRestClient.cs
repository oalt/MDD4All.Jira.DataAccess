using MDD4All.Jira.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jira3 = MDD4All.Jira.DataModels.V3;

namespace MDD4All.Jira.DataAccess
{
    /// <summary>
    /// Client to access Jira data using the Jira Cloud REST API 
    /// https://developer.atlassian.com/cloud/jira/software/rest/intro/
    /// </summary>
    public class JiraRestClient
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private string _url;

        public JiraRestClient(string serverURL,
                              string userName,
                              string apiKey)
        {
            _url = serverURL;

            _httpClient.DefaultRequestHeaders.Accept.Clear();

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            byte[] credentials = Encoding.ASCII.GetBytes($"{userName}:{apiKey}");

            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

            _httpClient.DefaultRequestHeaders.Add("Authorization", authenticationHeaderValue.ToString());
        }

        public async Task<string> GetJiraIssueAsync(string issueID)
        {
            string response = await _httpClient.GetStringAsync(_url + "/rest/api/3/issue/" + issueID + "?expand=names,changelog");

            return response;
        }

        public async Task<IssueSearchResponse> GetIssuesByJQL(string jql)
        {
            IssueSearchResponse result = null;

            string restString = "";

            string encodedJql = Uri.EscapeDataString(jql);

            restString = _url + "/rest/api/2/search?jql=" + encodedJql;

            try
            {

                string response = await _httpClient.GetStringAsync(restString);

                result = JsonConvert.DeserializeObject<IssueSearchResponse>(response);

            }
            catch (Exception exception)
            {
                Debug.WriteLine("Error getting data from Jira. JQL:  " + jql);
                Debug.WriteLine(exception);
            }

            return result;
        }

        public async Task<ProjectSearchResponse> GetJiraProjectsAsync()
        {
            string response = await _httpClient.GetStringAsync(_url + "/rest/api/2/project/search");

            ProjectSearchResponse result = JsonConvert.DeserializeObject<ProjectSearchResponse>(response);

            return result;
        }

        public async Task<Project> GetJiraProjectByKeyAsync(string projectKey)
        {
            Project result = null;

            try
            {
                string response = await _httpClient.GetStringAsync(_url + "/rest/api/2/project/" + projectKey);

                result = JsonConvert.DeserializeObject<Project>(response);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            return result;
        }


        public async Task<List<Jira3.Status>> GetStatusesAsync()
        {
            List<Jira3.Status> result = new List<Jira3.Status>();

            try
            {
                string response = await _httpClient.GetStringAsync(_url + "/rest/api/3/status");

                result = JsonConvert.DeserializeObject<List<Jira3.Status>>(response);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            return result;
        }

    }
}

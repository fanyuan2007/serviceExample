using BlastService.Private.Controllers;
using BlastService.Private.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

using BlastService.Private.ModelContract;

namespace BlastService.Private.Tests.IntegrationTests
{
    internal class TestHttpClient
    {
        protected static HttpClient Client;
        private string BaseUrl { get; set; }

        public TestHttpClient()
        {
            Client = new HttpClient();
            BaseUrl = "https://localhost:61901";
        }

        public void DisposeClient()
        {
            Client.Dispose();
        }

        // Add functions to operate on Project (GET, POST, DELETE)
        // Each function takes a ProjectRequest and returns a ProjectResponse if applicable and a status code

        /// <summary>
        /// Get all projects from the server through the provided route
        /// </summary>
        /// <param name="route"></param>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public List<ProjectResponse> GetAllProjectsTest(string route, out HttpResponseMessage httpResponse)
        {
            httpResponse = new HttpResponseMessage();
            var responses = new List<ProjectResponse>();
            var serviceUri = String.Format("{0}/{1}/Projects", BaseUrl, route);
            try
            {
                httpResponse = Client.GetAsync(serviceUri).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var resultContent = httpResponse.Content.ReadAsStringAsync().Result;
                    if(resultContent != string.Empty)
                    {
                        responses = JsonConvert.DeserializeObject<List<ProjectResponse>>(resultContent);
                    }

                    return responses;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// Get a project by id from the server through the provided route
        /// </summary>
        /// <param name="route"></param>
        /// <param name="projectId"></param>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public ProjectResponse GetProjectById(string route, Guid projectId, out HttpResponseMessage httpResponse)
        {
            httpResponse = new HttpResponseMessage();
            var serviceUri = String.Format("{0}/{1}/Projects/{2}", BaseUrl, route, projectId);

            try
            {
                httpResponse = Client.GetAsync(serviceUri).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var resultContent = httpResponse.Content.ReadAsStringAsync().Result;
                    var response = JsonConvert.DeserializeObject<ProjectResponse>(resultContent);
                    return response;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Delete a project by id in the server if it exists
        /// </summary>
        /// <param name="route"></param>
        /// <param name="projectId"></param>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public bool DeleteProjectById(string route, Guid projectId, out HttpResponseMessage httpResponse)
        {
            httpResponse = new HttpResponseMessage();
            var serviceUri = String.Format("{0}/{1}/Projects/{2}", BaseUrl, route, projectId);

            try
            {
                httpResponse = Client.GetAsync(serviceUri).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    httpResponse = Client.DeleteAsync(serviceUri).Result;
                    return true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        /// <summary>
        /// Add a project to the server through the provided route
        /// </summary>
        /// <param name="route">The provided route in the server</param>
        /// <param name="project">The project to be added</param>
        /// <param name="httpResponse">output the response of the http request</param>
        /// <returns>OK if succeed, other codes for various issues</returns>
        public HttpStatusCode PostProjectTest(string route, ProjectRequest project, out HttpResponseMessage httpResponse)
        {
            httpResponse = new HttpResponseMessage();
            var serviceUri = String.Format("{0}/{1}/Projects", BaseUrl, route);
            try
            {
                var json = JsonConvert.SerializeObject(project);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = Client.PostAsync(serviceUri, content).Result;
                return httpResponse.StatusCode;
            }
            catch (WebException we)
            {
                Console.WriteLine(we.Message);
                return ((HttpWebResponse)we.Response).StatusCode;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return httpResponse.StatusCode;
            }
        }

        /// <summary>
        /// Add a pattern to a specific project in the server through the route
        /// </summary>
        /// <param name="route">The provided route in the server</param>
        /// <param name="pattern">The pattern to be added</param>
        /// <param name="projectId">The guid of the project where the pattern adds to</param>
        /// <param name="httpResponse">output the response of the http request</param>
        /// <returns></returns>
        public HttpResponseMessage PostPatternTest(string route, PatternRequest pattern, Guid projectId)
        {
            var httpResponse = new HttpResponseMessage();
            var serviceUri = String.Format("{0}/{1}/Projects/{2}/patterns", BaseUrl, route, projectId.ToString());
            try
            {
                httpResponse = Client.GetAsync(serviceUri).Result;
                // Check if the uri is valid (either route incorrect or the project doesn't exist)
                if (httpResponse.IsSuccessStatusCode)
                {
                    var json = JsonConvert.SerializeObject(pattern);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    httpResponse = Client.PostAsync(serviceUri, content).Result;
                }

                return httpResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return httpResponse;
            }
        }

        /// <summary>
        /// Get a pattern with the provided pattern id through the provided route
        /// </summary>
        /// <param name="route">The provided route in the server</param>
        /// <param name="projectId">The provided project id</param>
        /// <param name="patternId">The provided pattern id</param>
        /// <param name="httpResponse">output the response of the http request</param>
        /// <returns></returns>
        public PatternResponse GetPatternTest(string route, Guid projectId, Guid patternId, out HttpResponseMessage httpResponse)
        {
            httpResponse = new HttpResponseMessage();
            var serviceUri = String.Format("{0}/{1}/Projects/{2}/patterns/{3}", BaseUrl, route, projectId.ToString(), patternId.ToString());

            try
            {
                httpResponse = Client.GetAsync(serviceUri).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var resultContent = httpResponse.Content.ReadAsStringAsync().Result;
                    var pattern = JsonConvert.DeserializeObject<PatternResponse>(resultContent);
                    
                    return pattern;
                }

                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<PatternResponse> GetAllPatternsTest(string route, Guid projectId, out HttpResponseMessage httpResponse)
        {
            httpResponse = new HttpResponseMessage();
            var responses = new List<PatternResponse>();
            var serviceUri = String.Format("{0}/{1}/Projects/{2}/patterns", BaseUrl, route, projectId.ToString());

            try
            {
                httpResponse = Client.GetAsync(serviceUri).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var resultContent = httpResponse.Content.ReadAsStringAsync().Result;
                    if (resultContent != string.Empty)
                    {
                        responses = JsonConvert.DeserializeObject<List<PatternResponse>>(resultContent);
                        return responses;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }

            return null;
        }
    }
}

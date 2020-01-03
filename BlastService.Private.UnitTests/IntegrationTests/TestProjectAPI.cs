using BlastService.Private.ModelContract;

using NUnit.Framework;

using System.Net.Http;
using System;
using System.Net;

namespace BlastService.Private.Tests.IntegrationTests
{
    [TestFixture]
    public class TestProjectAPI
    {
        private TestHttpClient _client;
        private ProjectRequest _project;
        private string _baseUrl;

        [SetUp]
        public void Setup()
        {
            _client = new TestHttpClient();
            _project = CreateProjectRequest(ProjectFields.All);
            _baseUrl = "api/v0.3";

            HttpResponseMessage httpRespM;
            _client.PostProjectTest(_baseUrl, _project, out httpRespM);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            // Use HttpClient to remove project
            HttpResponseMessage httpRespM;
            _client.DeleteProjectById(_baseUrl, _project.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NoContent);
            _client.DisposeClient();
        }

        // Each test should setup the project with some values.
        [Test]
        public void GetAllProjects_ExistingRoute_Test()
        {
            HttpResponseMessage httpRespM;
            var projectResponses = _client.GetAllProjectsTest(_baseUrl, out httpRespM);
            if (projectResponses != null)
            {
                Console.WriteLine("number of projects returned: {0}", projectResponses.Count);
            }
            // Return empty list should still be successed in http communication
            Assert.IsNotNull(projectResponses);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);
        }

        [Test]
        public void GetProjectById_Test()
        {
            HttpResponseMessage httpRespM;
            var projectResponse = _client.GetProjectById(_baseUrl, _project.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(projectResponse);
        }

        [Test]
        public void GetProjectById_NonExistProj_Test()
        {
            HttpResponseMessage httpRespM;
            var newProject = CreateProjectRequest(ProjectFields.All);

            var projectResponse = _client.GetProjectById(_baseUrl, newProject.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NotFound);
            Assert.IsNull(projectResponse);
        }

        [Test]
        public void PostProject_ExistingRoute_Test()
        {
            var newProject = CreateProjectRequest(ProjectFields.All);

            HttpResponseMessage httpRespM;
            _client.PostProjectTest(_baseUrl, newProject, out httpRespM);
            Console.WriteLine("The Status Code returned for correct route: {0}", httpRespM.StatusCode);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);
            if (httpRespM.IsSuccessStatusCode)
            {
                var isSucceed = _client.DeleteProjectById(_baseUrl, newProject.NameBasedProperties.BaseProperties.Id, out httpRespM);
                if (!isSucceed)
                {
                    Console.WriteLine("Fail to delete the newly added testing project.");
                }
            }
        }

        // Each POST test case covers omitting on required field
        [TestCase(ProjectFields.NameBasedProperties)]
        [TestCase(ProjectFields.Description)]
        [TestCase(ProjectFields.Unit)]
        [TestCase(ProjectFields.TimeZone)]
        public void PostProject_OmitRequiredField_Test(ProjectFields field)
        {
            var newProject = CreateProjectRequest(field);

            bool isEnumType = false;
            switch (field)
            {
                case ProjectFields.Unit:
                    isEnumType = true;
                    break;
                case ProjectFields.Description:
                case ProjectFields.NameBasedProperties:
                case ProjectFields.TimeZone:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Undefined project field: {0}", field));
            }

            HttpResponseMessage httpRespM;
            var statusCode = _client.PostProjectTest(_baseUrl, newProject, out httpRespM);
            Console.WriteLine("The Status Code returned for correct route: {0}", statusCode);
            // EnumType always have a default value for the field
            if (isEnumType)
            {
                Assert.IsTrue(httpRespM.IsSuccessStatusCode);
                _client.DeleteProjectById(_baseUrl, newProject.NameBasedProperties.BaseProperties.Id, out httpRespM);
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NoContent);
            }
            else
            {
                Assert.AreEqual(statusCode, HttpStatusCode.BadRequest);
            }
        }


        [TestCase(ProjectFields.LocalTransformation)]
        [TestCase(ProjectFields.Mapping)]
        public void PostProject_OmitNonRequiredField_Test(ProjectFields field)
        {
            var newProject = CreateProjectRequest(field);
            HttpResponseMessage httpRespM;
            _client.PostProjectTest(_baseUrl, newProject, out httpRespM);
            Console.WriteLine("The Status Code returned for correct route: {0}", httpRespM.StatusCode);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);
            Assert.IsTrue(httpRespM.StatusCode == HttpStatusCode.OK || httpRespM.StatusCode == HttpStatusCode.Created);

            if (httpRespM.IsSuccessStatusCode)
            {
                var isSucceed = _client.DeleteProjectById(_baseUrl, newProject.NameBasedProperties.BaseProperties.Id, out httpRespM);
                if (!isSucceed)
                {
                    Console.WriteLine("Fail to delete the newly added testing project.");
                }
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NoContent);
            }
        }

        [Test]
        public void DeleteProject()
        {
            // Use status code to verify
            var testProject = CreateProjectRequest(ProjectFields.All);
            HttpResponseMessage httpRespM;
            var isSuccess = _client.DeleteProjectById(_baseUrl, testProject.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Console.WriteLine("The Status Code returned for correct route: {0}", httpRespM.StatusCode);
            if (isSuccess)
            {
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NoContent);
            }
            // cannot find the project with the provided route or projectId
            else
            {
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NotFound);
            }
        }

        [Test]
        public void Project_EnumTypeFields_Test()
        {
            var projectAllFields = CreateProjectRequest(ProjectFields.All);
            HttpResponseMessage httpRespM;
            _client.PostProjectTest(_baseUrl, projectAllFields, out httpRespM);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);

            var response = _client.GetProjectById(_baseUrl, projectAllFields.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(Enum.IsDefined(typeof(MeasurementUnit), response.Unit));
            Assert.IsTrue(Enum.IsDefined(typeof(CoordinateConvention), response.Mapping));
            if (httpRespM.IsSuccessStatusCode)
            {
                _client.DeleteProjectById(_baseUrl, projectAllFields.NameBasedProperties.BaseProperties.Id, out httpRespM);
                Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.NoContent);
            }
        }

        [TestCase(ProjectFields.NameBasedProperties)]
        [TestCase(ProjectFields.Description)]
        [TestCase(ProjectFields.TimeZone)]
        public void Project_RequiredFields_Nullable_Test(ProjectFields field)
        {
            var projectNullableFields = CreateProjectRequest(ProjectFields.All);
            switch (field)
            {
                case ProjectFields.NameBasedProperties:
                    projectNullableFields.NameBasedProperties = null;
                    break;
                case ProjectFields.Description:
                    projectNullableFields.Description = null;
                    break;
                case ProjectFields.TimeZone:
                    projectNullableFields.TimeZone = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Undefined project field: {0}", field));
            }

            HttpResponseMessage httpRespM;
            _client.PostProjectTest(_baseUrl, projectNullableFields, out httpRespM);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.IsTrue(!httpRespM.IsSuccessStatusCode);
        }

        [TestCase(ProjectFields.LocalTransformation)]
        [TestCase(ProjectFields.Mapping)]
        public void Project_NonRequiredFields_Nullable_Test(ProjectFields field)
        {
            var projectNullableFields = CreateProjectRequest(ProjectFields.All);
            
            switch (field)
            {
                case ProjectFields.LocalTransformation:
                    projectNullableFields.LocalTransformation = null;
                    break;
                case ProjectFields.Mapping:
                    projectNullableFields.Mapping = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Undefined project field: {0}", field));
            }

            HttpResponseMessage httpRespM;
            _client.PostProjectTest(_baseUrl, projectNullableFields, out httpRespM);
            Console.WriteLine("The Status Code returned : {0}", httpRespM.StatusCode);
            Assert.IsTrue(httpRespM.IsSuccessStatusCode);

            var response = _client.GetProjectById(_baseUrl, projectNullableFields.NameBasedProperties.BaseProperties.Id, out httpRespM);
            Console.WriteLine("The Status Code returned for correct route: {0}", httpRespM.StatusCode);
            Assert.AreEqual(httpRespM.StatusCode, HttpStatusCode.OK);
        }

        private ProjectRequest CreateProjectRequest(ProjectFields omitField)
        {
            var projectRequest = new ProjectRequest();
            if (omitField != ProjectFields.NameBasedProperties)
            {
                projectRequest.NameBasedProperties = new NamedBaseProperty
                {
                    Name = "TestProject",
                    BaseProperties = new BaseProperty()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid()
                    }
                };
            }

            if (omitField != ProjectFields.Description)
            {
                projectRequest.Description = "This project is for testing purpose.";
            }

            if (omitField != ProjectFields.TimeZone)
            {
                projectRequest.TimeZone = new UTCTimeZone()
                {
                    OffsetHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Hours,
                    OffsetMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Minutes,
                    IdName = TimeZoneInfo.Local.Id
                };
            }

            if (omitField != ProjectFields.Unit)
            {
                projectRequest.Unit = MeasurementUnit.Metric;
            }

            if (omitField != ProjectFields.LocalTransformation)
            {
                projectRequest.LocalTransformation = new LocalTransformation() 
                {
                    Translation = new Transformation()
                    {
                        East = 10.0,
                        North = 10.0,
                        Elev = 10.0
                    },
                    Rotation = new Transformation()
                    {
                        East = 10.0,
                        North = 10.0,
                        Elev = 10.0
                    },
                    Scaling = new Transformation()
                    {
                        East = 10.0,
                        North = 10.0,
                        Elev = 10.0
                    }
                };
            }

            if (omitField != ProjectFields.Mapping)
            {
                projectRequest.Mapping = CoordinateConvention.NorthEastElev;
            }

            return projectRequest;
        }
    }
}
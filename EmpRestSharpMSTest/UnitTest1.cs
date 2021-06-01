using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using EmpRestSharp;


namespace EmpRestSharpMSTest
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:3000");
        }
        private IRestResponse GetEmployeeList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("Employees", Method.GET);
            //Act
            // Execute the request
            IRestResponse response = client.Execute(request);
            return response;
        }

        
        // UC1 Retrieve all employee details in the json file 
        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(6, employeeList.Count);
            foreach (Employee emp in employeeList)
            {
                Console.WriteLine("Id: " + emp.Id + "\t" + "Name: " + emp.Name + "\t" + "Salary: " + emp.Salary);
            }
        }

        //UC2 add new emp details
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            //Arrange
            ///Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("Employees", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("name", "Ritik");
            jsonObj.Add("salary", "50000");
            jsonObj.Add("id", "6");
            ///Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Ritik", employee.Name);
            Assert.AreEqual("50000", employee.Salary);
            Console.WriteLine(response.Content);
        }

        /// UC3 Ability to adding multiple employees to the json file using JSON server and returns the same

        [TestMethod]
        public void OnCallingPostAPIForAEmployeeListWithMultipleEMployees_ReturnEmployeeObject()
        {
            // Arrange
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { Name = "Radha", Salary = "85536" });
            employeeList.Add(new Employee { Name = "Watson", Salary = "120123" });
            employeeList.Add(new Employee { Name = "Christiano Ronaldo", Salary = "123456" });
            //Iterate the loop for each employee
            foreach (var emp in employeeList)
            {
                ///Initialize the request for POST to add new employee
                RestRequest request = new RestRequest("Employees", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("name", emp.Name);
                jsonObj.Add("salary", emp.Salary);
                ///Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(emp.Name, employee.Name);
                Assert.AreEqual(emp.Salary, employee.Salary);
                Console.WriteLine(response.Content);
            }
        }
        
            /// UC4 Ability to update the salary into the json file in json server
            
            [TestMethod]
            public void OnCallingPutAPI_ReturnEmployeeObject()
            {
                //Arrange
                //Initialize the request for Post to add new employee
                RestRequest request = new RestRequest("Employees", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("name", "Radha");
                jsonObj.Add("salary", "65000");
                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual("Radha", employee.Name);
                Assert.AreEqual("65000", employee.Salary);
                Console.WriteLine(response.Content);
            }
        /// UC5  Ability to delete the employee details with given id
        
        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/Employees/10", Method.DELETE);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }

    }
}

    


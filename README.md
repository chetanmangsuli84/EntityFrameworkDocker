 simple REST API that allows users to: 


•	Allows users to submit/post messages 

•	Lists received messages 

•	Retrieves a specific message on demand, and determines if it is a palindrome. 

•	Allows users to delete specific messages 

•	Build capability into your code so that it can be “observed”(Monitoring/Traceability/metrics)


This API is build using the .netcore Entityframework using the code first approach.
This service uses SQL Server container as a backend to persist the messages I have used Swagger as a UI component to test all the functionalities.
This application is containerized. using Docker. 

Image of this application  is also available on the docker hub https://hub.docker.com/repository/docker/chetanmangsuli84/messageswebapi
Image of the DB used is available https://hub.docker.com/repository/docker/chetanmangsuli84/mssql-server-linux:2017-latest

DB image can be pulled using 


docker pull chetanmangsuli84/mssql-server-linux:2017-latest

Application image can be pulled used


docker pull chetanmangsuli84/messageswebapi:latest

To run the application you can clone the code 

git clone https://github.com/chetanmangsuli84/EntityFrameworkDocker.git

Navigate to the EntityFrameworkDocker Folder and run 


  docker-compose build


  docker-compose up
  
You can now access the WEBAPI using the URL

http://localhost:32033/swagger/index.html





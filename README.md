# Shopping_Cart_Server Solution
Source code is placed at GitHub https://github.com/DevShuku/Shopping_Cart_server_V.0

Create a web application for online shopping.

📢 This project is built with **.net 8.0

Swagger Page
https://localhost:7184/swagger/v1/swagger.json

Tech stack
Dotnet core 8.0
Note ⚠️

usefull data creation script is added along > ../Properties/useful__scripts.sql

How to run the project?
On pre installed Visual Studio 2022 (It is the latest as of May,2024). Now, follow the following steps.

Open command prompt. Go to a directory where you want to clone this project. Use this command to clone the project.
  git clone https://github.com/DevShuku/Shopping_Cart_server_V.0.git
Go to the directory where you have cloned this project, open the directory Shopping_Cart_server_V.0. You will find a file with name Shopping_Cart_server_V.0.sln. Double click on this file and this project will be opened in Visual Studio.
Since there is no database connection we can skip this part else the below is a vital step
open appsettings.json file and update connection string in your local and run on the localhost after configuring the  datacontext.
  "ConnectionStrings": {

      "Eshoppingcart": "Data Source=localhost;Database={Database name};User={username};Password={password};Integrated Security=True;TrustServerCertificate=true;"
    
  }

Now you can run this project

Scope of extenstion to this projects are: 
a) Custom Exception handling pages
b) Unit test implementation. 
c) SonarQube implementation for the code quality purpose

Thanks a lot 🙂🙂

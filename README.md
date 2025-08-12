# **Niped Challenge**

In the Data folder of this repository you will find two JSON files, medical guidelines and client medical data with values that are referenced in the guidelines.

**Create a system to return a report for the given clients by combining their medical data with the guidelines.**

There is a lot of freedom in this exercise where you can choose which parts of the application you find most important and we are not looking for one specific solution. You can make it an API, webapplication, native app. Do you include security issues, and if so how? Will you create a backend system to handle the configuration of the guidelines? It's all up to you. You are also free the change the data structure of the given JSON files, either in the files or while processing/storing/handling it later in the application.

Use whatever technologies you want to show off. At Niped we use C# and MSSql on the Azure platform with communication between services through Azure ServiceBus. But do you prefer creating a web application, or native apps with Flutter and a SignalR or Python-based API, or even a WebAssembly web application? Go for it. We believe good developers can adapt to whatever technology they are interested in.

We are not expecting you to create a fully-fledged health check application, so choose wisely which parts of such a solution you want to focus on. Create what you feel shows off your programming skills best and then send in a Pull Request. You will then be invited for an interview with two Developers in which you can discuss your choices with them.

Good luck, and most importantly, have fun!

#### **Evaluation Criteria**
- Code structure, readability, testability and maintainability.
- Database design choices.
- Bonus: Security best practices and scalability considerations.

# Nader's additions

## Information
- There are three pages that have work done by me
  - Clients page (accessible via the navbar)
  - Specific Client page (accessible via the `View` button in the Clients page)
  - The Clients status page (accessible via the navbar)
- The charts are made with Chart.js
- This is not meant to be a solution that addresses all possible pitfalls, there are security and scalability concerns with this implementation

## Instructions
- The application can use either just the JSON files to get the data or a postgres database created with EntityFramework
  - To toggle between the two:
    - In docker edit the environment variable: `NipedSettings__UseDatabase`
      - `True` uses the postgres database that is created in the same docker compose, `False` only uses the Json files
    - If running from an IDE change the `NipedSettings.UseDatabase` value in the appropriate `appsettings.json`
- By default the application will available at http://localhost/
- To start the application use `docker compose up` to run everything in docker
- If you want to the application only without docker set `NipedSettings.UseDatabase` to `False` and run the application from your favourite IDE.
- To use the database with the application from your IDE then run `docker compose up postgresdb` and then run the application from your favourite IDE.


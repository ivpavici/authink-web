Management web for creating tasks and test for the AuThink apps

- Create a local database and name it [Authink.Admin.v2] 
- In Authink.Data project, there is a script AuthinkData.edmx.sql which recreates the database schema. 
- To make the web work locally with VS 2017 and newest SSMS I had to change the connection string from data source = localhost to: data source =(LocalDb)\MSSQLLocalDB;
- When web is up, to login -> username is authink, pass is marko

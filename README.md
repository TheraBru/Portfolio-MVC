# Portfolio page with .NET MVC
This repository is a project for a web based portfolio. Added is functionality for saving information about education, websites and known frameworks/languages. Information is saved in a SQLite database and connected to a MCV system through asp .NET Entity Framework.

Log in functionality is added with Identity Framework. Nuget package SendGrid is added for functionality to send email to user and admin. In order for this to work you need to add a secret api key with variable name SendGridKey.

You will also need to replace place holder information with your own email and name.

Functionality for uploading to heroku through docker is also added.
# Coding Tracker
My fourth C# application following [The C# Sharp Academy Path](https://www.thecsharpacademy.com/#console-area) and my last beginner Console app. 

Based on the [coding tracker](https://www.thecsharpacademy.com/project/13/coding-tracker) project.  
Console based CRUD application to track time spent coding.  
Developed using C#, SQLite, Dapper ORM and [ConsoleTableExt Library](https://github.com/minhhungit/ConsoleTableExt).  

# Given Requirements:
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the hours will be logged.
- [x] You need to be able to insert, delete, update and view your logged hours. 
- [x] You should handle all possible errors so that the application never crashes (Dates and Times)
- [x] The application should only be terminated when the user inserts 0. 
- [x] You can only interact with the database using SQLite and Dapper ORM.
- [x] You have to use Separation of Concerns to keep your code clean and organize.
- [x] You should adhere to the DRY principle when you can .

# Features

* SQLite database connection

	- The program uses a SQLite db connection to store and read information. 
	- If no database exists, or the correct table does not exist they will be created on program start.

* CRUD DB functions

	- From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in yyyy-MM-dd HH:mm format.
	- Time and Dates inputted are checked to make sure they are in the correct and realistic format. 

* Reporting and other data output uses ConsoleTableExt library to output in a more pleasant way

	- ![image](https://user-images.githubusercontent.com/15159720/141688462-e5dc465c-f188-4ac9-a166-397653c53c41.png)   

# Challenges
	
- It was my first time using Dapper ORM and ConsoleTableExt. I had to learn how to use these technologies in order to complete this project. 
- Dapper was easier than using ADO.NET but still I had to learn how to recieve the list in that format and parse them back to the type of list of my liking. 
- Being able to start a session (pause, restart and finish) in the console and then choosing if you want to add it to the database.
- Filter the coding records per period (days, weeks, years).
- Creating a report showing the total of the duration as well as the average duration per day.

# Areas to Improve
- I need to get better at using the same methods for different purposes, I think i did an okey job by using a lot of default parameters but it can be improved.
- Single responsibility. I did improve at this along the way, but I still have some work to do on it. I think I could have made my methods a little better so they only had a single use. 
- I need to get better at naming my methods, sometimes I don't know if my code it's readable enough for others programers.
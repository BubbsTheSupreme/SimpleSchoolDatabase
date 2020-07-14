# SimpleSchoolDatabase
This is a project that I made to practice using Entity Framework Core, 
and some additional object oriented programming concepts that I am still
a little new to.

This is not really meant to be used in an actual school, (I felt I needed to specify that) 
I themed the software around school becuase thats the first thing that came 
to mind when I thought of places that use databases.

The use is mostly from the perspective of the school administration that
handles accepting students into the school, and potentially expelling them
and keeping track of who was expelled and why.

I might evolve this project into something bigger after I learn more about
ASP.NET Core and WinForms in .NET Core 3. I have a few ideas already
after giving it some thought.

To use the software as the client/student just run it in the commandline
with no arguments. and you will be presented options to apply,
which puts the application data into the database, and the other option
is to check the status of if you the student were accepted into the school.

To use the software as an admin you run it with the commandline arguments
-a (id_of_admin) (password_of_admin) as you can see you need an id of an admin and a password.
There is a default admin with the credentials 0, "password". if needed 
the default admin can be removed after initial log in assuming you created a new one before hand

In order to actually use the database you will need SQLite installed on your computer and have the 
School.sql script in the same directory as the application or include the path when creating the database,
but thats assuming you convert it to an executable with .NET Core. If you run it from within the projects 
own directory you just need to create the database file normally with the instructions below.

## On Windows systems you want to run the command
`sqlite3 Students.db -init Students.sql`

## On Linux and Unix based systems you want run the command
`sqlite3 School.db < School.sql`

Both of these should create the School.db file and after that it should run 
after the setup with no issue.
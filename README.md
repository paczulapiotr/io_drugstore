# Silesian University of Technology Project

## Technology stack:
- .NET CORE 2.1.403
- SqlServer 13
- NodeJs v10.13.0

## Steps to start project
- Prepare environment for stack shown in `Technology Stack` section
- Clone repository
- Open Drugstore projects appsettings.json file. 
- Set SQLSERVER ConnectionString   in "WarehouseConnection" and "IdentityConnection" EXAMPLE:

> "Data Source=__`<<INSERT PATH HERE>>`__;Database=Drugstore;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true;"

> "Data Source=__`<<INSERT PATH HERE>>`__;Database=Identity;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true;"
- Build solution
- Run (double click) update_databse.cmd script to update your database
- Run (double click) update_npm.cmd script to update npm, needs NodeJs installed (might take a while)
- Run solution
















Author: Piotr Paczuła	

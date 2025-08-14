# Introduction
This ORM project started in 2007 in order to create an easy and flexible way to manage database interaction in a platform independent object oriented way.
The IT.TnDigit.ORM library is a porting in dotnet standard 2.0 to assure usage for old and new technology stacks and can be referenced through [nuget](https://www.nuget.org/packages?q=it.tndigit.orm).

The library is composed by core assemblies:
- IT.TnDigit.ORM.Interfaces
- IT.TnDigit.ORM.DataTypes 
- IT.TnDigit.ORM.ClientController 
- IT.TnDigit.ORM.DataStorage 
- IT.TnDigit.ORM.DataProviders 

and a set of platform specific implementations:
- IT.TnDigit.ORM.DataProviders.Oracle 
- IT.TnDigit.ORM.DataProviders.PostgreSQL 
- IT.TnDigit.ORM.DataProviders.SqlClient
- ...

# Getting Started
Install Data types generator application [download](http://tfs.intra.infotn.it:8080/tfs/InformaticaTrentina/IT.InfoTN.Framework/_git/IT.InfoTn.ORM?path=%2FSetup%2FRelease%2FSetup.msi&version=GBmaster&_a=contents)

Define a new configuration for the data source and project and generate all the classes you need
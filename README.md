# CSV-anonymiser
Anonymise customers and their addresses

## How to use
- In the command line, navigate to the directory where the **.csproj** file is located
- Enter command ```dotnet run "CSV anonymiser. csproj"```

## Configuration guide
- To change the input file directory, modify the ```CsvAnonymiser.Classes.FileProcessor.InputFilesDirectory``` property
- To change the input file names, modify the launchSettings.json

## Anonymisation details - fields that get anonymised

### Customers file:
- firstname
- middlename
- lastname
- dob
- gender
- password_hash

### Addresses file:
- firstname _(from custumer, if exists)_
- middlename _(from custumer, if exists)_
- lastname _(from custumer, if exists)_
- street
- city
- state
- postalcode
- telephone

### Subscriptions file:
- subscriber_email

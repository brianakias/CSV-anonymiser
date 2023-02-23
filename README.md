# CSV-anonymiser
Anonymise customers and their addresses

## Usage
Say that ```path``` is the path to the directory where the **CSV anonymiser.csproj** file is stored
- Enter command ```dotnet run --project "path\\CSV anonymiser.csproj" customers_sample.csv customer_addresses_sample.csv customer_subscriptions_sample.csv```

## Configuration guide
- To change the input file directory, modify the ```CsvAnonymiser.Classes.FileProcessor.InputFilesDirectory``` property
  - The default directory is the user's desktop folder
- To change the input file names, modify the launchSettings.json
  - The default Customers file name is: customers_sample.csv
  - The default Addresses file name is: customer_addresses_sample.csv
  - The default Subscriptions file name is: customer_subscriptions_sample.csv

## Anonymisation details - fields that get anonymised

### Customers file
- firstname
- middlename
- lastname
- dob
- gender
- password_hash

### Addresses file
- firstname _(from custumer, if exists)_
- middlename _(from custumer, if exists)_
- lastname _(from custumer, if exists)_
- street
- city
- state
- postalcode
- telephone

### Subscriptions file
- subscriber_email

# CSV-anonymiser
Anonymise customers and their addresses

- To change the input file directory, modify the CsvAnonymiser.Classes.FileProcessor.InputFilesDirectory property.
- To change the input file names, modify the launchSettings JSON File.

From the customers file I am anonymising the below:

- firstname
- middlename
- lastname
- dob
- gender
- password_hash

From the addresses file I am anonymising the below:

- firstname _(from custumer, if exists)_
- middlename _(from custumer, if exists)_
- lastname _(from custumer, if exists)_
- street
- city
- state
- postalcode
- telephone

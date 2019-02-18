# Silesian University of Technology Project

## Technology stack:
- .NET CORE 2.1.403
- SqlServer 13
- NodeJs v10.13.0

## Steps to start project
- Prepare environment for stack shown in `Technology Stack` section
- Clone repository
- Open Drugstore projects appsettings.json file. 
- Set SQLSERVER ConnectionString in "WarehouseConnection" EXAMPLE:

> "Data Source=__`<<INSERT PATH HERE>>`__;Database=Drugstore;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true;"

- Build solution
- Run (double click) update_database.cmd script to update your database
- Run (double click) update_npm.cmd script to update npm, needs NodeJs installed (might take a while)
- Run solution



## `Format plików XML`
### `Dodawanie leków do magazynu zewnętrznego`
Przykład: <br>
Pierwszy lek istnieje w bazie dlatego potrzeba podać tylko Id lek, jego nazwe oraz dodaną ilość. Jest to konieczne minimum. Drugi lek jest nowy, nie istnieje w systemie.
```xml
<?xml version="1.0"?>
<XmlMedicineSupplyModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	<Medicines>
		<XmlMedicineModel>
			<StockId>1</StockId>
			<Name>Apap</Name>
			<Quantity>10</Quantity>
		</XmlMedicineModel>
		<XmlMedicineModel>
			<Name>Nowy lek</Name>
			<IsNew>true</IsNew>
			<Quantity>100</Quantity>
			<IsRefunded>false</IsRefunded>
			<Category>Normal</Category>
		</XmlMedicineModel>
	</Medicines>
</XmlMedicineSupplyModel>
```

### `Aktualizacja sprzedanych leków w magazynie zewnętrznym`
Przykład: <br>
Należy tylko podać Id leku, oraz sprzedaną ilość. Nazwa jest opcjonalna
```xml
<?xml version="1.0"?>
<XmlMedicineSupplyModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	<Medicines>
		<XmlMedicineModel>
			<StockId>1</StockId>
			<Name>Apap</Name>
			<Quantity>10</Quantity>
		</XmlMedicineModel>
		<XmlMedicineModel>
			<StockId>2</StockId>
			<Name>Polopiryna</Name>
			<Quantity>10</Quantity>
		</XmlMedicineModel>
	</Medicines>
</XmlMedicineSupplyModel>
```















Author: Piotr Paczuła	

# Invoice API

Simple API REST to manage invoices with the purpose of gaining knowledge about ASP.NET.

## Prerequisites

- [**.NET 5.0 SDK**](https://dotnet.microsoft.com/download/dotnet/5.0)

## Configuration

If you want to use the https://free.currconv.com service to retrieve invoices in a currency other than the one in which they were saved, you will have to modify the `appsetting` file to include the API Key to access this service.

``` json
  "Config": {
    "CURRCONV_APIKEY": "XXXX"
  }
```

## How to deploy

Just execute:

``` bash
dotnet run # Includes the building step
```

You can check an Azure Pipeline example about how to generate the artifact [here](./.azure/ci.yml).

## RESTful API

### Register an invoice

Request:

`POST /api/invoice/`

``` bash
curl -i -H 'Content-Type: application/json' https://localhost:5001/api/invoice/ \
-d '{ "invoiceId": "891248192B", "supplier": "Fake supplier", "dateIssued": "2021-01-27T17:16:40", "currency": "EUR", "amount": 1000.00, "description": "New projector for confenrece room" }'
```

Response:

``` JSON
{
    "data": [
        {
            "id": "891248192B",
            "type": "invoice",
            "attributes": {
                "invoiceId": "891248192B",
                "supplier": "Fake supplier",
                "dateIssued": "2021-01-27T17:16:40",
                "currency": "EUR",
                "amount": 1000,
                "description": "New projector for confenrece room"
            }
        }
    ]
}
```

### Get invoices

Request:

`GET /api/invoice`

``` bash
curl -i -H 'Content-Type: application/json' https://localhost:5001/api/invoice/'
```

Response:

``` JSON
{
    "data": [
        {
            "id": "891248192B",
            "type": "invoice",
            "attributes": {
                "invoiceId": "891248192B",
                "supplier": "Fake supplier",
                "dateIssued": "2021-01-27T17:16:40",
                "currency": "EUR",
                "amount": 1000,
                "description": "New projector for confenrece room"
            }
        },
        ...
    ]
}
```

### Get an invoice

Request:

`GET /api/invoice/<invoiceID>`

``` bash
curl -i -H 'Content-Type: application/json' https://localhost:5001/api/invoice/891248192B'
```


Response:

``` JSON
{
    "data": [
        {
            "id": "891248192B",
            "type": "invoice",
            "attributes": {
                "invoiceId": "891248192B",
                "supplier": "Fake supplier",
                "dateIssued": "2021-01-27T17:16:40",
                "currency": "EUR",
                "amount": 1000,
                "description": "New projector for confenrece room"
            }
        }
    ]
}
```

### Delete an invoice

Request:

`DELETE /api/invoice/<invoiceID>`

``` bash
curl -X DELETE -i -H 'Content-Type: application/json' https://localhost:5001/api/invoice/891248192B'
```

Response:

``` JSON
{
    "data": [
        {
            "id": "891248192B",
            "type": "invoice"
        }
    ]
}
```

### Modify an invoice

Request:

`PUT /api/invoice/<invoiceID>`

``` bash
curl -X PUT -i -H 'Content-Type: application/json' https://localhost:5001/api/invoice/891248192B \
-d '{ "invoiceId": "891248192B", "supplier": "Fake supplier", "dateIssued": "2021-01-27T17:16:40", "currency": "EUR", "amount": 1500.00, "description": "New table for confenrece room" }'
```

Response:

``` JSON
{
    "data": [
        {
            "id": "891248192B",
            "type": "invoice",
            "attributes": {
                "invoiceId": "891248192B",
                "supplier": "Fake supplier",
                "dateIssued": "2021-01-27T17:16:40",
                "currency": "EUR",
                "amount": 1500,
                "description": "New table for confenrece room"
            }
        }
    ]
}
```

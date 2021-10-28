# Invoice API

Simple API REST to manage invoices with the purpose of gaining knowledge about ASP.NET.

## Prerequisites

- [**.NET 5.0 SDK**](https://dotnet.microsoft.com/download/dotnet/5.0)

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
    "success": true
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
    "success": true,
    "invoices": [<list of invoices>]
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
    "success": true,
    "invoices": [<requested invoice>]
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
    "success": true
}
```

### Modify an invoice

Request:

`PUT /api/invoice/<invoiceID>`

``` bash
curl -X PUT -i -H 'Content-Type: application/json' https://localhost:5001/api/invoice/891248192B \
-d '{ "invoiceId": "891248192B", "supplier": "Fake supplier", "dateIssued": "2021-01-27T17:16:40", "currency": "EUR", "amount": 1000.00, "description": "New projector for confenrece room" }'
```

Response:

``` JSON
{
    "success": true
}
```

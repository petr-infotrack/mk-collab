# Message Type Filter
## Filter Types:
- `text` filter - compares equality of a selected field and provided value
- `regex` filter - match the selected field with regex expression
- `function` filter - executes function from embedded or external function library (suitable for more complex matching)

## Filter Structure

Filter definition is stored using JSON syntax. However to simply the manual entry and readability of the definition, YAML entry will be provided as follow:

<i>Filter fields:</i>
- `active` true or false to active use of custom filters
- `filters` keyword defines root level for definition of one of more filters (at least one is required)
- `match` defines one or more conditions that applies to a particular filter (at least one is required)
- `queryType` defines type of filter (e.g. text, regex, ...)
- `queryValue` value, pattern or function name used by the filter
- `field` defines the fiels for text or regex comparison

Note: if multiple filters are defined they are processed in sequence using AND operator. 

### Example - a filter with simple text comparison condition
```JSON
{
  "active": "true",
  "filters": [{
    "match": [
      {
        "queryType": "text",
        "queryValue": "app1",
        "field": "application"
      }
    ]
  }]
}
```

### Example - a filter with multiple conditions (AND logic) 
```JSON
{
  "active": "true",
  "filters": [{
    "match": [
      {
        "queryType": "text",
        "queryValue": "app1",
        "field": "Application"
      },
      {
        "queryType": "text",
        "queryValue": "error",
        "field": "level"
      }
    ]
  }]
}
```

### Example - a filter with condition using Regex 
```JSON
{
  "active": "true",
  "filters": [{
    "match": [
      {
        "queryType": "regex",
        "queryValue": "\\{.*?\\}",
        "field": "MessageTemplate"
      }
    ]
  }]
}
```

### Example - a filter with conditon using custom function 
```JSON
{
  "active": "true",
  "filters": [{
    "match": [
      {
        "queryType": "function",
        "queryValue": "CustomFunctions|IsIISError",
      }
    ]
  }]
}
```

## Example Configuration
Excerpt from the `appsettings.json` file

```JSON
{
  // . . . previous configuration ...

"customFilters": {
    "active": "false",
    "filters": [
      {
        "name": "Example Filter",
        "applyTo": [ "map-message-type" ],
        "model": "AlertsAdmin.Elastic.Models.ElasticErrorMessage",
        "match": [
          {
            "queryType": "regex",
            "queryValue": "\\{.*?\\}",
            "field": "MessageTemplate"
          },
          {
            "queryType": "function",
            "queryValue": "PlaceholderCustomFunctions|IsIISError",
            "field": "Message"
          }
        ]
      }
    ]
  }

  // . . . other configurations ...

}
```

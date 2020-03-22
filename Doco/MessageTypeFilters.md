# Message Type Filter
## Filter Logic

### Filter Types:
- `text` filter - compares equality of the selected field and provided value
- `regex` filter - match the selected field with defined expression
- `function` - executes function from embedded function library (suitable for more complex matching)

### Filter Structure

Filter definition is stored using JSON syntax. However to simply the manual entry and readability of the definition, YAML entry will be provided as follow:

Syntax:
- `filter` keyword defines root level functionality
- `match` defines one or more filters (at least one is required)
- `queryType` defines type of filter (e.g. text, regex, ...)
- `queryValue` value, pattern or function name used by the filter
- `field` defines the fiels for text or regex comparison

- if multiple filters are defined they are processed in sequence using AND operator.  If OR operator is required, filter definition can begin with `operator` key word (e.g. operator: OR)

### Example - simple text comparison 

```YAML
filter:
    match:
    - queryType: text
      queryValue: app1
      field: application
```

is be converted into JSON, validated and stored definition:
```JSON
{
  "filter": {
    "match": [
      {
        "queryType": "text",
        "queryValue": "app1",
        "field": "application"
      }
    ]
  }
}
```


### Example - multiple text filters with AND logic 
selects applications app1 with level equals error
```YAML
filter:
    match:
    - queryType: text
      queryValue: app1
      field: application

    - queryType: text
      queryValue: error
      field: level
```
Note: keyword `operator` for AND is implied (not mandatory)

### Example - multiple text filters with OR logic 
selects applications app1 or app2
```YAML
filter:
    match:
    - queryType: text
      queryValue: app1
      field: application

    - queryType: text
      queryValue: error
      field: level
      operator: OR
```
will convert in JSON equivalent
```JSON
{
  "filter": {
    "match": [
      {
        "queryType": "text",
        "queryValue": "app1",
        "field": "application"
      },
      {
        "queryType": "text",
        "queryValue": "error",
        "field": "level",
        "operator": "OR"
      }
    ]
  }
}
```

### Example - another set of multiple filters with AND or OR logic 

```YAML
filter:
    match:
    - queryType: text
      queryValue: error
      field: level

    - queryType: regex
      queryValue:  ^{.}
      field: messageTemplate


-- OR Example with the function sytax  --

filter:
    match:
    - queryType: regex
      queryValue:  ^{.}
      field: messageTemplate

    - queryType: function
      queryValue: IsIISError
      operator: OR
```


# SchemaMapper

## Overview

SchemaMapper is a command line tool that can connect to a running instance of a PostGres database and generate a textual and visual representation of that schema, in the form of an Entity-Relationship diagram. 

The textual representation, and the rendered SVG are both saved to disk.

SchemaMapper supports customisation of the diagram output by passing a text file to the binary, containing a list of database table names to include.

## Installation

### From Source

- Clone this repo to disk
- Run `dotnet publish --output './build'`
- Execute the binary from the `./build` folder or add to your path

### Precompiled Binaries

- Clone this repo to disk
- Execute the appropriate binary in the `./build` directory or add to path

## Usage

Run the binary with no arguments to see a generated help document, showing different actions and options available.

### Generate Table List

Connects to the database and writes the list of tables to a text file on disk.  This command can be used to create a 'filter list' to customise the diagram output.

The tables will be written (one table name per line) to a file called `tables.txt` in the current directory.

```shell
schemamapper generate-table-list --connection-string "my-postgres-conn-string"
```

### Generate Textual Representation

Generates the textual representation (mermaid or erd) of the database schema and writes it to disk.

```shell
schemamapper generate-textual-representation --connection-string "my-postgres-conn-string" --
```

### Render Textual Representation

Renders a provided textual representation to SVG and writes it to disk.

```shell
schemamapper generate-table-list --connection-string "my-postgres-conn-string" --input './my-diagram.erd'
```

### Render Diagram

Renders a textual representation of the database schema, writes it to disk, then renders a SVG diagram and writes to disk. 

```shell
schemamapper generate-table-list --connection-string "my-postgres-conn-string"
```

## Limitations

- Only PostgreSQL databases are currently supported
- Only Mermaid and Erd are supported as diagramming languages currently
- Only 'public' database schema is currently supported
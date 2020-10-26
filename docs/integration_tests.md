# Running Integration Tests

The integration tests for the web API are defined in
[tests/KanbanBoard.IntegrationTests](../tests/KanbanBoard.IntegrationTests)
will test the API functionalities against a postgres database.

The strategy to execute the tests is create a exclusive postgres container for each test class and then make requests
to ensure that the API have the correct behavior.

## Requirements

-   [Docker](https://docs.docker.com/get-docker/)

## Setup

First, you will need the docker image of the database, so build from the dockerfile with the followings commands:

```bash
# Enter the database directory
$ cd database
# Build the docker image
$ docker build -t kanban-db .
```

Is important to set the image name as `kanban-db` because this is the image
that the integration tests will use to create the database containers.

With the image builded, return to the project root directory:

```bash
$ cd ..
```

## Running

To run all tests of the web API just run:

```
$ dotnet test
```

To run only the integration tests run:

```
$ dotnet test --filter TestType=Integration
```

## Running the tests only to a specific class

Since the integration tests will create a database container for each test class,
you can run only the tests of a specific class to minimize the resources consumptions.

To do so, run:

```
$ dotnet test --filter "TestType=Integration&Category=[class name]"
```

E.g:

    - Running only the UsersController tests:
    ```
    $ dotnet test --filter "TestType=Integration&Category=UsersController"
    ```

    - Running only the BoardsController tests:
    ```
    $ dotnet test --filter "TestType=Integration&Category=BoardsController"
    ```

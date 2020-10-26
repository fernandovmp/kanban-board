# Executing Postman Collection

Requirements

-   [Docker](https://docs.docker.com/get-docker/)
-   [Docker Compose](https://docs.docker.com/compose/install/)
-   [Newman](https://github.com/postmanlabs/newman)

The Postman collection defined in [docs/api/postman](./api/docs) provide a quickly
way to test some endpoints of the web API, to run these tests in an isolated environment
you will need the docker, docker-compose, and newman installed.

You can run the collection by simple run the script [postman-tests.sh](../postman-tests.sh)
on the root directory, this will require both 3000 and 5000 ports are free to be used by
docker-compose.

Example to run:

```bash
$ sh postman-tests.sh
```

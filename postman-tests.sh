#!/bin/bash

exit_if_previous_cmd_fail() {
    if [ $? -ne 0 ]
    then
        exit $?
    fi
}

docker-compose up -d
exit_if_previous_cmd_fail

seconds=15
echo "waiting $seconds seconds until the containers are ready"
sleep $seconds

postman_dir="docs/api/postman"
newman run "$postman_dir/API Tests.postman_collection.json" -e "$postman_dir/Test env.postman_environment.json"
postman_exit=$?

docker-compose down
exit_if_previous_cmd_fail
exit $postman_exit

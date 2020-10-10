# Data model

## Entities

Board: A kanban board with lists of tasks, owned by at least one user.

Lists: A list of tasks in a kanban board, can have many tasks.

Task: A kanban task, belongs to only one list and can be assigned to zero or
many users.

## Tables layout

**Board**

|    Field    |     type     | Key |               description               |
| :---------: | :----------: | :-: | :-------------------------------------: |
|     id      |    serial    | PK  |                Board Id                 |
|    title    | varchar(150) |  -  |           Title of the board            |
| created_on  | timestamp(2) |  -  |           Board creation date           |
| modified_on | timestamp(2) |  -  | Latest date when the board was modified |
| created_by  |   integer    | FK  |      User id who create the board       |

**List**

|    Field    |     type     | Key |              description               |
| :---------: | :----------: | :-: | :------------------------------------: |
|     id      |    serial    | PK  |                List Id                 |
|  board_id   |   integer    | FK  |   Board id that the list belongs to    |
|    title    | varchar(150) |  -  |           Title of the list            |
| created_on  | timestamp(2) |  -  |           List creation date           |
| modified_on | timestamp(2) |  -  | Latest date when the list was modified |

**Task**

|    Field    |     type     | Key |                description                |
| :---------: | :----------: | :-: | :---------------------------------------: |
|     id      |    serial    | PK  |                  Task Id                  |
|   summary   |     text     |  -  |            Summary of the task            |
| description |     text     |  -  |          Description of the task          |
|  tag_color  |  varchar(6)  |  -  | Hexadecimal that represents the tag color |
|  board_id   |   integer    | FK  |     Board id that the task belongs to     |
| created_on  | timestamp(2) |  -  |            Task creation date             |
| modified_on | timestamp(2) |  -  |  Latest date when the task was modified   |

**List Task**

|  Field  |  type   | Key |           description            |
| :-----: | :-----: | :-: | :------------------------------: |
| list_id | integer | PFK | List Id that the task belongs to |
| task_id | integer | PFK |             Task id              |

**User**

|  Field   |     type     | Key |        description        |
| :------: | :----------: | :-: | :-----------------------: |
|    id    |    serial    | PK  |          User Id          |
|   name   | varchar(100) |  -  |     Name of the user      |
|  email   | varchar(320) |  -  |    E-mail of the user     |
| password | varchar(75)  |  -  | Hash of the user password |

**Board Members**

|  Field   |  type   | Key |            description             |
| :------: | :-----: | :-: | :--------------------------------: |
| user_id  | integer | PFK |              User id               |
| board_id | integer | PFK |              Board id              |
| is_admin |  bool   |  -  | If the user is admin of that board |

**Assignments**

|  Field   |  type   | Key |            description             |
| :------: | :-----: | :-: | :--------------------------------: |
| user_id  | integer | PFK | User id that the task was assigned |
| board_id | integer | PFK |   Board id that the task belongs   |
| task_id  | integer | PFK |              Task id               |

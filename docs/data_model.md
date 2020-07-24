# Data model

## Entities

Board: A kanban board with lists of tasks, owned by at least one user.

Lists: A list of tasks in a kanban board, can have many tasks.

Task: A kanban task, belongs to only one list and can be assigned to zero or
many users.

## Tables layout

**Board**

|   Field    |     type     | Key |               description               |
| :--------: | :----------: | :-: | :-------------------------------------: |
|     id     |     int      | PK  |                Board Id                 |
|  summary   | varchar(150) |  -  |          Summary of the board           |
| createdOn  | timestamp(2) |  -  |           Board creation date           |
| modifiedOn | timestamp(2) |  -  | Latest date when the board was modified |
| createdBy  |     int      | FK  |      User id who create the board       |

**List**

|   Field    |     type     | Key |              description               |
| :--------: | :----------: | :-: | :------------------------------------: |
|     id     |     int      | PK  |                List Id                 |
|  boardId   |     int      | FK  |   Board id that the list belongs to    |
|  summary   |     text     |  -  |          Summary of the list           |
| createdOn  | timestamp(2) |  -  |           List creation date           |
| modifiedOn | timestamp(2) |  -  | Latest date when the list was modified |

**Task**

|    Field    |     type     | Key |                description                |
| :---------: | :----------: | :-: | :---------------------------------------: |
|     id      |     int      | PK  |                  Task Id                  |
|   summary   |     text     |  -  |            Summary of the task            |
| description |     text     |  -  |          Description of the task          |
|  tagColor   |  varchar(6)  |  -  | Hexadecimal that represents the tag color |
|  createdOn  | timestamp(2) |  -  |            Task creation date             |
| modifiedOn  | timestamp(2) |  -  |  Latest date when the task was modified   |

**List Task**

| Field  | type | Key |           description            |
| :----: | :--: | :-: | :------------------------------: |
| listId | int  | PFK | List Id that the task belongs to |
| taskId | int  | PFK |             Task id              |

**User**

|  Field   |     type     | Key |        description        |
| :------: | :----------: | :-: | :-----------------------: |
|    id    |     int      | PK  |          User Id          |
|   name   | varchar(100) |  -  |     Name of the user      |
|  email   | varchar(320) |  -  |    E-mail of the user     |
| password | varchar(75)  |  -  | Hash of the user password |

**User board**

|  Field  | type | Key |            description             |
| :-----: | :--: | :-: | :--------------------------------: |
| userId  | int  | PFK |              User id               |
| boardId | int  | PFK |              Board id              |
| isAdmin | bool |  -  | If the user is admin of that board |

**User task**

|  Field  | type | Key |            description             |
| :-----: | :--: | :-: | :--------------------------------: |
| userId  | int  | PFK | User id that the task was assigned |
| boardId | int  | PFK |   Board id that the task belongs   |
| taskId  | int  | PFK |              Task id               |

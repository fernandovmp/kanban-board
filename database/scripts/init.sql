create table if not exists Users (
    id serial,
    name varchar(100) not null,
    email varchar(320) unique not null,
    password varchar(75) not null,
    createdOn timestamp(2) not null,
    modifiedOn timestamp(2) not null,

    constraint PK_Users_id primary key (id)
);

create table if not exists Boards (
    id serial,
    title varchar(150) not null,
    createdOn timestamp(2) not null,
    modifiedOn timestamp(2) not null,
    createdBy serial not NULL,

    constraint PK_Boards_id primary key (id),
    constraint FK_Boards_createdBy foreign key (createdBy) references Users
);

create table if not exists BoardMembers (
    userId serial not null,
    boardId serial not null,
    isAdmin boolean not null,
    createdOn timestamp(2) not null,
    modifiedOn timestamp(2) not null,

    constraint PK_BoardMembers_userId_boardId primary key (userId, boardId),
    constraint FK_BoardMembers_userId foreign key (userId) references Users,
    constraint FK_BoardMembers_boardId foreign key (boardId) references Boards
);

create table if not exists Lists (
    id serial,
    boardId serial not null,
    title varchar(150) not null,
    createdOn timestamp(2) not null,
    modifiedOn timestamp(2) not null,

    constraint PK_Lists_id primary key (id),
    constraint FK_Lists_boardId foreign key (boardId) references Boards
);

create table if not exists Tasks (
    id serial,
    summary text not null,
    description text not null,
    tagColor varchar(6) not null,
    boardId serial not null,
    createdOn timestamp(2) not null,
    modifiedOn timestamp(2) not null,

    constraint PK_Tasks_id primary key (id),
    constraint FK_Tasks_boardId foreign key (boardId) references Boards
);

create table if not exists ListTasks (
    listId serial not null,
    taskId serial not null,

    constraint PK_ListTasks_listId_taskId primary key (listId, taskId),
    constraint FK_ListTasks_listId foreign key (listId) references Lists,
    constraint FK_ListTasks_taskId foreign key (taskId) references Tasks
);

create table if not exists Assignments (
    userId serial not null,
    boardId serial not null,
    taskId serial not null,

    constraint PK_Assignments_userId_boardId_taskId primary key (userId, boardId, taskId),
    constraint FK_Assignments_userId_boardId foreign key (userId, boardId) references BoardMembers,
    constraint FK_Assignments_taskId foreign key (taskId) references Tasks
);

/* VersionMigration migrating ================================================ */

CREATE TABLE "public"."VersionInfo" ("Version" bigint NOT NULL);
CREATE UNIQUE INDEX "UC_Version" ON "public"."VersionInfo" ("Version" ASC);
ALTER TABLE "public"."VersionInfo" ADD "AppliedOn" timestamp;
ALTER TABLE "public"."VersionInfo" ADD "Description" varchar(1024);

/* 202010101800: SetupDatabase migrating ===================================== */

/* CreateTable users */
CREATE TABLE "public"."users" (
    "id" serial NOT NULL,
    "name" varchar(100) NOT NULL,
    "email" varchar(320) NOT NULL,
    "password" varchar(75) NOT NULL,
    "created_on" timestamp(2) NOT NULL,
    "modified_on" timestamp(2) NOT NULL,
    CONSTRAINT "PK_users_id" PRIMARY KEY ("id")
);
/* CreateIndex users (email) */
CREATE UNIQUE INDEX "IX_users_email" ON "public"."users" ("email" ASC);

/* CreateTable boards */
CREATE TABLE "public"."boards" (
    "id" serial NOT NULL,
    "title" varchar(150) NOT NULL,
    "created_by" integer NOT NULL,
    "created_on" timestamp(2) NOT NULL,
    "modified_on" timestamp(2) NOT NULL,
    CONSTRAINT "PK_boards_id" PRIMARY KEY ("id")
);
ALTER TABLE "public"."boards" ADD CONSTRAINT "FK_boards_created_by" FOREIGN KEY ("created_by") REFERENCES "public"."users" ("id");

/* CreateTable board_members */
CREATE TABLE "public"."board_members" (
    "user_id" integer NOT NULL,
    "board_id" integer NOT NULL,
    "is_admin" boolean NOT NULL,
    "created_on" timestamp(2) NOT NULL,
    "modified_on" timestamp(2) NOT NULL
);
ALTER TABLE "public"."board_members" ADD CONSTRAINT "FK_board_members_user_id" FOREIGN KEY ("user_id") REFERENCES "public"."users" ("id");
ALTER TABLE "public"."board_members" ADD CONSTRAINT "FK_board_members_board_id" FOREIGN KEY ("board_id") REFERENCES "public"."boards" ("id");
ALTER TABLE "public"."board_members" ADD CONSTRAINT "PK_board_members_user_id_board_id" PRIMARY KEY ("user_id", "board_id");

/* CreateTable lists */
CREATE TABLE "public"."lists" (
    "id" serial NOT NULL,
    "board_id" integer NOT NULL,
    "title" varchar(150) NOT NULL,
    "created_on" timestamp(2) NOT NULL,
    "modified_on" timestamp(2) NOT NULL,
    CONSTRAINT "PK_lists_id" PRIMARY KEY ("id")
);
ALTER TABLE "public"."lists" ADD CONSTRAINT "FK_lists_board_id" FOREIGN KEY ("board_id") REFERENCES "public"."boards" ("id");

/* CreateTable tasks */
CREATE TABLE "public"."tasks" (
    "id" serial NOT NULL,
    "board_id" integer NOT NULL,
    "summary" text NOT NULL,
    "description" text NOT NULL,
    "tag_color" varchar(6) NOT NULL,
    "created_on" timestamp(2) NOT NULL,
    "modified_on" timestamp(2) NOT NULL,
    CONSTRAINT "PK_tasks_id" PRIMARY KEY ("id")
);
ALTER TABLE "public"."tasks" ADD CONSTRAINT "FK_tasks_board_id" FOREIGN KEY ("board_id") REFERENCES "public"."boards" ("id");

/* CreateTable list_tasks */
CREATE TABLE "public"."list_tasks" (
    "list_id" integer NOT NULL,
    "task_id" integer NOT NULL
);
ALTER TABLE "public"."list_tasks" ADD CONSTRAINT "FK_list_tasks_listId" FOREIGN KEY ("list_id") REFERENCES "public"."lists" ("id");
ALTER TABLE "public"."list_tasks" ADD CONSTRAINT "FK_list_tasks_taskId" FOREIGN KEY ("task_id") REFERENCES "public"."tasks" ("id");
ALTER TABLE "public"."list_tasks" ADD CONSTRAINT "PK_list_tasks_list_id_task_id" PRIMARY KEY ("list_id", "task_id");

/* CreateTable assignments */
CREATE TABLE "public"."assignments" (
    "user_id" integer NOT NULL,
    "board_id" integer NOT NULL,
    "task_id" integer NOT NULL
);
ALTER TABLE "public"."assignments" ADD CONSTRAINT "FK_assignments_taskId" FOREIGN KEY ("task_id") REFERENCES "public"."tasks" ("id");
ALTER TABLE "public"."assignments" ADD CONSTRAINT "PK_assignments_user_id_board_id_task_id" PRIMARY KEY ("user_id", "board_id", "task_id");
ALTER TABLE "public"."assignments" ADD CONSTRAINT "FK_assignments_user_id_board_id" FOREIGN KEY ("user_id","board_id") REFERENCES "public"."board_members" ("user_id","board_id");

INSERT INTO "public"."VersionInfo" ("Version","AppliedOn","Description") VALUES (202010101800,'2020-10-10T21:05:03','SetupDatabase');
/* 202010101800: SetupDatabase migrated */

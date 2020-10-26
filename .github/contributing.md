# Contributing

Thanks for considering and take the time to contribute to this project, any contribution is welcome, and I'll be glad about it.

Table of content:

-   [Issues](#issues)
-   [Pull Requests](#pull-requests)
-   [How to run](#run-the-project)

## Issues

If you find a bug or have a suggestion you can open an
[Issue](https://github.com/fernandovmp/kanban-board/issues/new).

-   By opening an issue to report a bug, explain the actual behavior and the
    expected behavior, this will help to fix it.
-   When giving a suggestion, describe mostly as detailed you can, this will help
    to understand and assess whether it is suitable.

## Pull Requests

First of all, fork this repository.

Then clone the repository.

```
git clone https://github.com/<your-username>/kanban-board.git
```

Set the workspace directory to the project directory.

```
cd kanban-board
```

Pull requests needs to target the `development` branch, to do so make sure you
are in the development branch before creating a new branch.

```
git checkout development
git pull origin development
```

In the `development` branch create a new branch to make your changes.

```
git checkout -b my-branch
```

After making your changes, push them to your Github repository.

Commit messages should follow the
[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
specification.

```
git push origin
```

Then go to [Pull Requests](https://github.com/fernandovmp/kanban-board) of this
repository and submit your pull request.

As stated before, the pull request should target the `development` branch, so make sure to set your base branch to `development`.

![](https://user-images.githubusercontent.com/45287292/88575193-d97f9b80-d019-11ea-96b1-acf2fde47bee.png)

If your changes require updating the documentation be sure to update it before
submit your pull request

## Run the project

To run the project follow these instructions in the
[README](../README.md#how-to-run) file

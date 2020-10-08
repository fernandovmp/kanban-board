import { SummarizedTask, TaskList } from '../../models';

export const removeTaskFromList = (
    lists: TaskList[],
    listId: number,
    taskId: number
) => {
    const listWithContainsTheTask = lists.find((list) => list.id === listId);
    if (!listWithContainsTheTask) {
        return lists;
    }
    const filteredLists = lists.filter((list) => list.id !== listId);
    const updatedList = {
        ...listWithContainsTheTask,
        tasks: listWithContainsTheTask.tasks.filter(
            (tasks) => tasks.id !== taskId
        ),
    };
    const finalLists = [...filteredLists, updatedList];
    return finalLists.sort((a, b) => a.id - b.id);
};

export const addTaskToList = (
    lists: TaskList[],
    listId: number,
    task: SummarizedTask
) => {
    const listWithContainsTheTask = lists.find((list) => list.id === listId);
    if (!listWithContainsTheTask) {
        return lists;
    }
    const filteredLists = lists.filter((list) => list.id !== listId);
    const updatedList = {
        ...listWithContainsTheTask,
        tasks: [...listWithContainsTheTask.tasks, task],
    };
    const finalLists = [...filteredLists, updatedList];
    return finalLists.sort((a, b) => a.id - b.id);
};

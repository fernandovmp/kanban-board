import React from 'react';
import addIcon from '../../../assets/add.svg';
import { TaskList } from '../../../models';
import { Button, ListTitle, ListWrapper } from './styles';
import { TaskCard } from './TaskCard';

interface ITaskListProps {
    taskList: TaskList;
}

export const TaskListView: React.FC<ITaskListProps> = ({ taskList }) => {
    const handleCreateTask = () => {};

    return (
        <ListWrapper>
            <ListTitle>{taskList.title}</ListTitle>
            {taskList.tasks.map((task) => (
                <TaskCard key={task.id} task={task} />
            ))}
            <Button onClick={handleCreateTask}>
                <img src={addIcon} alt="New Task" />
                New Task
            </Button>
        </ListWrapper>
    );
};
